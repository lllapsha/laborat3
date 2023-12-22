using laborat3;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lab2.Tests
{
    [TestFixture]
    public class CatalogTests
    {
        private Catalog catalog;

        [SetUp]
        public void Setup()
        {
            catalog = new Catalog();
        }

        [Test]
        public void SaveToJson_ShouldCreateJsonFile()
        {
            var track1 = new Track { Title = "Song1", Author = "Author1" };
            var track2 = new Track { Title = "Song2", Author = "Author2" };

            catalog.AddTrack(track1);
            catalog.AddTrack(track2);

            catalog.SaveToJson();

            Assert.That(File.Exists(Catalog.JsonFilePath), Is.True);

            File.Delete(Catalog.JsonFilePath);
        }

        [Test]
        public void LoadFromJson_ShouldLoadTracksFromJsonFile()
        {
            var track1 = new Track { Title = "Song1", Author = "Author1" };
            var track2 = new Track { Title = "Song2", Author = "Author2" };

            catalog.AddTrack(track1);
            catalog.AddTrack(track2);

            catalog.SaveToJson();

            catalog = new Catalog();

            catalog.LoadFromJson();

            Assert.AreEqual(2, catalog.AllTracks.Count());
        }

        [Test]
        public void SaveToXml_ShouldCreateXmlFile()
        {
            var track1 = new Track { Title = "Song1", Author = "Author1" };
            var track2 = new Track { Title = "Song2", Author = "Author2" };

            catalog.AddTrack(track1);
            catalog.AddTrack(track2);

            catalog.SaveToXml();

            Assert.IsTrue(File.Exists(Catalog.XmlFilePath));

            File.Delete(Catalog.XmlFilePath);
        }

        [Test]
        public void LoadFromXml_ShouldLoadTracksFromXmlFile()
        {
            var track1 = new Track { Title = "Song1", Author = "Author1" };
            var track2 = new Track { Title = "Song2", Author = "Author2" };

            catalog.AddTrack(track1);
            catalog.AddTrack(track2);

            catalog.SaveToXml();

            // очистка треков
            catalog = new Catalog();

            catalog.LoadFromXml();

            Assert.AreEqual(2, catalog.AllTracks.Count());
        }

        [Test]
        public void SaveToSQLite_ShouldSaveTracksToDatabase()
        {
            var track1 = new Track { Title = "Song1", Author = "Author1" };
            var track2 = new Track { Title = "Song2", Author = "Author2" };

            catalog.AddTrack(track1);
            catalog.AddTrack(track2);

            catalog.SaveToSQLite();

            // очистка треков
            catalog = new Catalog();

            catalog.LoadFromSQLite();

            Assert.AreEqual(2, catalog.AllTracks.Count());
        }
    }
}