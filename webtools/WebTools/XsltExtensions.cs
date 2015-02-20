using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Atmosphere.WebTools
{
    public class XsltExtensions
    {
        public static string Timestamp()
        {
            return String.Format("Site last updated on {0} at {1}.",
                DateTime.Now.ToLongDateString(),
                DateTime.Now.ToLongTimeString());
        }

        /// <summary>
        /// When <see cref="Level" /> is 0, this method returns "./". When <see cref="Level" /> 
        /// is 1 or greater, returns that number of "../"s appended together.
        /// </summary>
        /// <returns></returns>
        public static string PathToRoot()
        {
            if (Level <= 0) return "./";

            var path = new StringBuilder();

            for (int i = 0; i < Level; i++) path.Append("../");

            return path.ToString();
        }

        public static XPathNodeIterator InsertFileContent(string name)
        {
            string path = Path.Combine(WebTools.InsertDir.FullName, name);

            XPathDocument doc = new XPathDocument(path);

            XPathNavigator nav = doc.CreateNavigator();

            return nav.Select("/");
        }




        public static XPathNodeIterator GetPages(string subfolder)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(CurrentPagesDirectory.FullName, subfolder));

            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("root"));
                        
            Dictionary<string, int> sortMap = new Dictionary<string, int>();

            foreach (FileInfo file in dir.GetFiles("*.xml", SearchOption.TopDirectoryOnly))
            {
                XmlDocument page = new XmlDocument();
                page.Load(file.FullName);

                int sortOrder = 0;

                XmlNode node = page.SelectSingleNode("/page/@sortOrder");

                if (node != null) sortOrder = Int32.Parse(node.Value);

                sortMap.Add(file.Name, sortOrder);
            }


            using (XmlWriter writer = doc.DocumentElement.CreateNavigator().AppendChild())
            {
                foreach (string fileName in sortMap.Keys.OrderBy(x => sortMap[x]))
                {
                    writer.WriteElementString("page", fileName);
                }
            }

            return doc.DocumentElement.CreateNavigator().Select("page");
        }






        public static XPathNodeIterator GetSiblings()
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("root"));

            Dictionary<string, int> sortMap = new Dictionary<string,int>();

            foreach (FileInfo file in CurrentPagesDirectory.GetFiles("*.xml", SearchOption.TopDirectoryOnly))
            {
                if (file.FullName != CurrentPage.FullName)
                {
                    XmlDocument page = new XmlDocument();
                    page.Load(file.FullName);

                    int sortOrder = 0;

                    XmlNode node = page.SelectSingleNode("/page/@sortOrder");

                    if (node != null) sortOrder = Int32.Parse(node.Value);

                    sortMap.Add(file.Name, sortOrder);
                }
            }
            
            using (XmlWriter writer = doc.DocumentElement.CreateNavigator().AppendChild())
            {
                foreach (string fileName in sortMap.Keys.OrderBy(x => sortMap[x]))
                {
                    writer.WriteElementString("page", fileName);                    
                }
            }

            return doc.DocumentElement.CreateNavigator().Select("page");
        }




        public static string CapitalizeWord(string word)
        {
            if (String.IsNullOrEmpty(word)) return word;

            return String.Format("{0}{1}", Char.ToUpper(word[0]), word.Substring(1));
        }

        public static string TitlizeWord(string word)
        {
            if (word == null || word.Length == 0) return word;

            word = CapitalizeWord(word);

            string title = word;

            // If there's already spaces, skip this block
            if (word.IndexOf(' ') < 0)
            {
                StringBuilder sb = new StringBuilder();
                                
                for (int i = 0; i < word.Length; i++)
                {
                    if (Char.IsDigit(word[i]))
                    {
                        sb.Append(" ");

                        while (i < word.Length && Char.IsDigit(word[i]))
                        {
                            sb.Append(word[i]);
                            i++;
                        }
                    }
                    else if (Char.IsUpper(word[i]))
                    {
                        if (i > 0 && !Char.IsUpper(word[i - 1]))
                        {
                            sb.Append(" ");
                            sb.Append(word[i]);
                        }
                        else
                        {
                            sb.Append(word[i]);
                        }
                    }
                    else
                    {
                        sb.Append(word[i]);
                    }
                }

                title = sb.ToString();
            }

            return title;
        }




        public static int Level { get; set; }

        public static DirectoryInfo CurrentPagesDirectory { get; set; }

        public static FileInfo CurrentPage { get; set; }
    }
}
