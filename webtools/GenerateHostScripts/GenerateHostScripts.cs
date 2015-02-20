using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Atmosphere.WebTools
{
    class GenerateHostScripts
    {
        static List<string> _commands;
        static List<string> _cleanup;

        static void ProcessDirectory(DirectoryInfo dir, params string[] skips)
        {
            if (skips.Any(x => x == dir.Name))
            {
                WebTools.Log("Skipping: {0}", dir.Name);
                return;
            }

            _commands.Add(String.Format("mkdir \"{0}\"", dir.Name));
            _commands.Add(String.Format("cd \"{0}\"", dir.Name));

            _cleanup.Add(String.Format("cd \"{0}\"", dir.Name));


            foreach (DirectoryInfo sub in dir.GetDirectories()) ProcessDirectory(sub, skips);

            foreach (FileInfo file in dir.GetFiles())
            {
                _commands.Add(String.Format("put \"{0}\" \"{1}\"", file.FullName, file.Name));
                //_cleanup.Add(String.Format("rm \"{0}\"", file.Name));
            }

            _cleanup.Add("rm *");


            _cleanup.Add(String.Format("cd ..", dir.Name));
            _cleanup.Add(String.Format("rmdir \"{0}\"", dir.Name));

            _commands.Add(String.Format("cd .."));
        }

        static void GenerateMainScripts()
        {
            WebTools.Log("Generating host/cleanup scripts for main directory...");

            _commands = new List<string>();
            _cleanup = new List<string>();

            ProcessDirectory(WebTools.Web, WebTools.WebGalleryDir.Name, WebTools.WebCalendarsDir.Name);

            // mkdir .www
            // cd .www
            _commands.RemoveRange(0, 2);
            // cd .. ; (for .www)
            _commands.RemoveAt(_commands.Count - 1);



            // cd .www
            _cleanup.RemoveAt(0);
            // cd ..
            // rmdir .www
            _cleanup.RemoveRange(_cleanup.Count - 2, 2);



            using (var writer = new StreamWriter(@"hostweb.fat.host.psftp", false))
            {
                foreach (string cmd in _commands) writer.WriteLine(cmd);
            }
            using (var writer = new StreamWriter(@"hostweb.fat.clean.next.psftp", false))
            {
                foreach (string cmd in _cleanup) writer.WriteLine(cmd);
            }
        }

        static void GenerateGalleryScripts()
        {
            WebTools.Log("Generating host/cleanup scripts for gallery directory...");

            if (WebTools.WebGalleryDir.Exists)
            {
                _commands = new List<string>();
                _cleanup = new List<string>();

                ProcessDirectory(WebTools.WebGalleryDir);

                using (var writer = new StreamWriter(@"hostweb.fat.gallery.host.psftp", false))
                {
                    foreach (string cmd in _commands) writer.WriteLine(cmd);
                }
                using (var writer = new StreamWriter(@"hostweb.fat.gallery.clean.next.psftp", false))
                {
                    foreach (string cmd in _cleanup) writer.WriteLine(cmd);
                }
            }
            else
            {
                WebTools.Log("Could not find directory; skipping: {0}", WebTools.WebGalleryDir.FullName);
            }
        }

        static void GenerateCalendarsScripts()
        {
            WebTools.Log("Generating host/cleanup scripts for calendars directory...");
            
            if (WebTools.WebCalendarsDir.Exists)
            {
                _commands = new List<string>();
                _cleanup = new List<string>();

                ProcessDirectory(WebTools.WebCalendarsDir);

                using (var writer = new StreamWriter(@"hostweb.fat.calendars.host.psftp", false))
                {
                    foreach (string cmd in _commands) writer.WriteLine(cmd);
                }
                using (var writer = new StreamWriter(@"hostweb.fat.calendars.clean.next.psftp", false))
                {
                    foreach (string cmd in _cleanup) writer.WriteLine(cmd);
                }
            }
            else
            {
                WebTools.Log("Could not find directory; skipping: {0}", WebTools.WebCalendarsDir.FullName);
            }
        }

        static void Main(string[] args)
        {
            GenerateMainScripts();
            GenerateGalleryScripts();
            GenerateCalendarsScripts();
        }
    }
}
