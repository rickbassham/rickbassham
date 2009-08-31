using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ComputerManager
{
    public class LocalApplicationDataStorage : IStorage
    {
        private static readonly string filePath = @"ComputerManager\Settings.xml";

        private static string GetFilePath()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(folder, filePath);
        }

        public void Save(Settings settings)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));

            string path = GetFilePath();

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            using (StreamWriter writer = File.CreateText(GetFilePath()))
            {
                serializer.Serialize(writer, settings);
            }
        }

        public Settings Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));

            string path = GetFilePath();

            if (File.Exists(path))
            {
                using (StreamReader reader = File.OpenText(path))
                {
                    return (Settings)serializer.Deserialize(reader);
                }
            }

            return new Settings();
        }
    }
}
