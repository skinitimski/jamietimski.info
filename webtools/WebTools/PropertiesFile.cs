using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Atmosphere.WebTools
{
    public class PropertiesFile
    {
        public PropertiesFile(string filePath)
            : this(new FileInfo(filePath)) { }

        public PropertiesFile(FileInfo file)
        {
            Properties = new Dictionary<string, string>();

            WebTools.Log("Reading properties file: {0}", file.FullName);

            using (StreamReader reader = new StreamReader(file.FullName))
            {
                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line == String.Empty) continue;
                    if (line[0] == '#') continue;
                    if (line[0] == ';') continue;

                    string[] parts = line.Split('\t');

                    if (parts.Length != 2) throw new Exception(String.Format("Line in properties file not valid: {0}", line));

                    if (Properties.ContainsKey(parts[0])) throw new Exception(String.Format("Duplicate key '{0}' in properties file.", parts[0]));

                    Properties.Add(parts[0], parts[1]);
                }
            }
        }

        public string GetProperty(string key, string @default = null)
        {
            string value = @default;

            if (Properties.ContainsKey(key)) value = Properties[key];

            return value;
        }

        public static string GetProperty(string path, string key, string @default = null)
        {
            string value = @default;

            PropertiesFile file = new PropertiesFile(path);

            value = file.GetProperty(key, @default);

            return value;
        }


        public string this[string key]
        {
            get
            {
                if (!Properties.ContainsKey(key)) throw new ArgumentException(String.Format("Property '{0}' doesn't exist in properties file."));

                return Properties[key];
            }
        }

        private Dictionary<string, string> Properties { get; set; }
    }
}
