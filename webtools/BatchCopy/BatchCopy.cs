using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;

namespace Atmosphere.WebTools
{
    class BatchCopy
    {
        private static FileInfo _lookupFile = new FileInfo(Path.Combine(WebTools.LookupsDir.FullName, "batchCopy.lookup"));


        static void RecursivelyCopy(FileInfo file, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in target.GetDirectories())
                RecursivelyCopy(file, dir);

            WebTools.Log("Copying file {0} to {1}", file.FullName, target.FullName);
            file.CopyTo(Path.Combine(target.FullName, file.Name), true);
        }


        static void Main(string[] args)
        {
            if (!_lookupFile.Exists) throw new Exception(@"Could not find 'batchCopy.lookup' in '.\lookups' folder.");

            using (StreamReader reader = new StreamReader(_lookupFile.FullName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == "") continue;
                    if (line[0] == '#') continue;
                    if (line[0] == ';') continue;

                    string[] parts = line.Split('\t');

                    string filename = parts[0];
                    string[] destinations = new string[parts.Length - 1];
                    Array.Copy(parts, 1, destinations, 0, destinations.Length);

                    // add the web dir to the start of each destination string
                    for (int i = 0; i < destinations.Length; i++) destinations[i] = Path.Combine(WebTools.Web.FullName, destinations[i]);

                    if (File.Exists(filename))
                    {
                        if (destinations.Length == 0) RecursivelyCopy(new FileInfo(filename), WebTools.Web);

                        foreach (string destination in destinations)
                        {
                            WebTools.Log("Copying file {0} to {1}", filename, destination);

                            if (!Directory.Exists(Path.GetDirectoryName(destination)))
                                Directory.CreateDirectory(Path.GetDirectoryName(destination));

                            File.Copy(filename, destination, true);
                        }
                    }
                    else if (Directory.Exists(filename))
                    {
                        DirectoryInfo source = new DirectoryInfo(filename);

                        foreach (string destination in destinations)
                        {
                            DirectoryInfo target = new DirectoryInfo(destination);

                            WebTools.Log("Copying directory {0} to {1}",
                                source.FullName,
                                target.FullName);

                            CopyDir(source, target, true);
                        }
                    }
                    else
                    {
                        WebTools.Log("File or directory doesn't exist: {0}", filename);
                    }
                }
            }
        }

        static bool CopyDir(DirectoryInfo source, DirectoryInfo target, bool overwrite)
        {
            if (target.Exists && !overwrite) return false;
            if (!target.Exists) target.Create();

            foreach (FileInfo f in source.GetFiles()) f.CopyTo(Path.Combine(target.FullName, f.Name));
            foreach (DirectoryInfo d in source.GetDirectories()) CopyDir(d, new DirectoryInfo(Path.Combine(target.FullName, d.Name)), overwrite);

            return true;
        }
    }
}
