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
        public const string JsonFilePath = "tracks.json";
        public const string XmlFilePath = "tracks.xml";
        private const string DbConnectionString = "Data Source=tracks.db;";

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

            try
            {
                if (File.Exists(JsonFilePath))
                {
                    string jsonData = File.ReadAllText(JsonFilePath);
                    tracks.AddRange(JsonSerializer.Deserialize<List<Track>>(jsonData));
                }
                else
                {
                    throw new FileNotFoundException("JSON file not found.", JsonFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data from JSON: {ex.Message}");
                // Добавьте дополнительную обработку ошибок при необходимости
            }
        }

        public void SaveToXml()
        {
           

            try
            {
                var serializer = new XmlSerializer(typeof(List<Track>));
                using (FileStream fileStream = new FileStream(XmlFilePath, FileMode.Create))
                {
                    serializer.Serialize(fileStream, tracks);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении в XML: {ex.Message}");
            }
        }

        public void LoadFromXml()
        {
            if (File.Exists(XmlFilePath))
            {
                var serializer = new XmlSerializer(typeof(List<Track>));
                using (var stream = new StreamReader(XmlFilePath))
                {
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
                    command.CommandText = "CREATE TABLE IF NOT EXISTS Tracks (Title TEXT, Author TEXT)";
                    command.ExecuteNonQuery();

                    foreach (Track track in tracks)
                    {
                        command.CommandText = "INSERT INTO Tracks (Title, Author) VALUES (@title, @author)";
                        command.Parameters.Clear();

                        command.Parameters.AddWithValue("@title", track.Title);
                        command.Parameters.AddWithValue("@author", track.Author);

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

                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Tracks";

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string title = reader["Title"].ToString();
                            string author = reader["Author"].ToString();
                            tracks.Add(new Track { Title = title, Author = author });
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
