using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Text;

namespace Atmosphere.WebTools
{
    class TransformPages
    {
        private static FileInfo _transformLookup = new FileInfo(Path.Combine(WebTools.LookupsDir.FullName, "transform.lookup"));

        private static Dictionary<string, FileInfo> _pageToXslt;


        static void Main(string[] args)
        {
            BuildLookup();

            ProcessDirectory(WebTools.Data, WebTools.XsltDir, WebTools.Web, "index.xml", false);
            
            ProcessDirectory(WebTools.PagesDir, WebTools.XsltDir, WebTools.WebPagesDir);
        }
        
        static void ProcessDirectory(DirectoryInfo xmlDir, DirectoryInfo xsltDir, DirectoryInfo webDir, string searchPattern = "*.xml", bool recur = true)
        {
            WebTools.Log("Processing Directory: {0}", xmlDir.FullName);

            if (recur) XsltExtensions.Level++;

            XsltExtensions.CurrentPagesDirectory = xmlDir;

            foreach (FileInfo page in xmlDir.GetFiles(searchPattern))
            {
                XsltExtensions.CurrentPage = page;

                string pageName = page.Name.Substring(0, page.Name.LastIndexOf('.'));
                string xsltFilepath = Path.Combine(xsltDir.FullName, String.Format("{0}.xslt", pageName));
                string xsltFilename = String.Empty;

                XslCompiledTransform xslt = new XslCompiledTransform();

                if (_pageToXslt.ContainsKey(page.FullName)) // front door -- lookup takes precedence
                {
                    xslt.Load(_pageToXslt[page.FullName].FullName);
                    xsltFilename = _pageToXslt[page.FullName].Name;
                    xsltFilepath = null;
                }
                else if (File.Exists(xsltFilepath)) // back door
                {
                    xslt.Load(xsltFilepath);
                    xsltFilename = Path.GetFileName(xsltFilepath);
                }
                else
                {
                    WebTools.Log("  ?! Transform lookup has no entry for page: {0}.", page.FullName);
                    continue;
                }

                WebTools.Log("  Transforming {0}.xml to {0}.html using {1}...", pageName, xsltFilename);

                if (!webDir.Exists) webDir.Create();

                XPathDocument xml = new XPathDocument(page.FullName);

                string htmlFilename = Path.Combine(webDir.FullName, String.Format("{0}.html", pageName));

                using (XmlTextWriter writer = new XmlTextWriter(htmlFilename, Encoding.UTF8))
                {

                    writer.Setup();
                    
                    xslt.Transform(xml, WebTools.XsltArgList, writer);
                }
            }

            if (recur)
            {
                foreach (DirectoryInfo xmlSubDir in xmlDir.GetDirectories())
                {
                    //DirectoryInfo xsltSubDir = new DirectoryInfo(Path.Combine(xsltDir.FullName, xmlSubDir.Name));
                    DirectoryInfo webSubDir = new DirectoryInfo(Path.Combine(webDir.FullName, xmlSubDir.Name));

                    // ProcessDirectory(xmlSubDir, xsltSubDir, webSubDir);
                    ProcessDirectory(xmlSubDir, xsltDir, webSubDir, searchPattern, recur);
                }
            }

            if (recur) XsltExtensions.Level--;
        }

        static void BuildLookup()
        {
            _pageToXslt = new Dictionary<string, FileInfo>();
            
            using (StreamReader reader = new StreamReader(_transformLookup.FullName))
            {
                string line = null;
                int count = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    count++;

                    if (line == "") continue;
                    if (line[0] == '#') continue;
                    if (line[0] == ';') continue;

                    string[] parts = line.Split('\t');

                    if (parts.Length != 2)
                    {
                        string msg = String.Format("Transform lookup invalid at line {0}.", count);
                        WebTools.Log(msg);
                        throw new ArgumentException(msg, "line");
                    }

                    FileInfo xslt = new FileInfo(Path.Combine(WebTools.XsltDir.FullName, parts[1]));

                    if (!xslt.Exists)
                    {
                        string msg = String.Format("XSLT file referenced in transform lookup at line {0} does not exist.", count);
                        WebTools.Log(msg);
                        throw new IOException(msg);
                    }

                    if (parts[0].EndsWith("/*") || parts[0].EndsWith("/**"))
                    {
                        string pageDir = Path.Combine(WebTools.PagesDir.FullName, parts[0].Substring(0, parts[0].Length - 2));

                        foreach (string path in Directory.GetFiles(pageDir, "*.xml", parts[0].EndsWith("**") ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                        {
                            FileInfo page = new FileInfo(path);

                            _pageToXslt.Add(page.FullName, xslt);
                        }
                    }
                    else
                    {
                        FileInfo page = new FileInfo(Path.Combine(WebTools.PagesDir.FullName, parts[0]));

                        if (page.Exists) _pageToXslt.Add(page.FullName, xslt);
                    }
                }
            }
        }
    }
}

