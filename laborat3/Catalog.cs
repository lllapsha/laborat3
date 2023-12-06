using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Xml.Serialization;
using Microsoft.Data.Sqlite;

namespace laborat3
{
     public class Catalog
    {
        private readonly List<Track> tracks = new List<Track>();
        private const string JsonFilePath = "tracks.json";
        private const string XmlFilePath = "tracks.xml";
        private const string DbConnectionString = "Data Source=tracks.db";

        public IEnumerable<Track> AllTrack { get; set; }

        public void AddTrack(Track track)
        {
            if (tracks.Contains(track)) // если уже есть такой трек
            {
                throw new ArgumentException("Такой трек уже есть в каталоге");
            }
            tracks.Add(track);

        }
        public void RemoveTrack(Track track)
        {
            Track existingTrack = tracks.FirstOrDefault(t => t.Title == track.Title && t.Author == track.Author);

            if (existingTrack != null)
            {
                tracks.Remove(existingTrack);
            }
            else
            {
                throw new ArgumentException("Такого трека нет в каталоге");
            }

        }

        public void SearchTrack(Track track)
        {
            Track existingTrack = tracks.FirstOrDefault(t => t.Title == track.Title && t.Author == track.Author);

            if (existingTrack != null)
            {
                track = existingTrack;
            }
            else
            {
                throw new ArgumentException("Такого трека нет в каталоге");
            }
        }


        public void SaveToJson()
        {
            var json = JsonSerializer.Serialize(tracks);
            File.WriteAllText(JsonFilePath, json);
        }

        public void LoadFromJson()
        {
            if (File.Exists(JsonFilePath))
            {
                var json = File.ReadAllText(JsonFilePath);
                tracks.Clear();
                tracks.AddRange(JsonSerializer.Deserialize<List<Track>>(json));
            }
        }

        public void SaveToXml()
        {
            var serializer = new XmlSerializer(typeof(List<Task>));
            using (var stream = new StreamWriter(XmlFilePath))
            {
                serializer.Serialize(stream, tracks);
            }
        }

        public void LoadFromXml()
        {
            if (File.Exists(XmlFilePath))
            {
                var serializer = new XmlSerializer(typeof(List<Task>));
                using (var stream = new StreamReader(XmlFilePath))
                {
                    tracks.Clear();
                    tracks.AddRange((List<Track>)serializer.Deserialize(stream));
                }
            }
        }

        public void SaveToSQLite()
        {
            using (var connection = new SqliteConnection(DbConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Tracks (
                    Title TEXT,
                    Author TEXT
                )";

                    command.ExecuteNonQuery();

                    foreach (var track in tracks)
                    {
                        command.CommandText = $@"
                    INSERT INTO Tracks (Title, Author)
                    VALUES ('{track.Title}', {track.Author})";

                        command.ExecuteNonQuery();
                    }
                }
            }
        }


        public void LoadFromSQLite()
        {
            using (var connection = new SqliteConnection(DbConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Tracks";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tracks.Add(new Track(reader.GetString(0), reader.GetString(1)));
                        }
                    }
                }
            }
        }




        public IEnumerable<Track> AllTracks
        {
            get
            {
                return tracks;
            }
        }

        public object JsonConvert { get; private set; }
    }
}
