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
    class GenerateIndex
    {
        private static FileInfo _caption = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "caption.xml"));
        private static FileInfo _contact = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "contact.xml"));
        private static FileInfo _banner = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "banner.xml"));
        private static FileInfo _indexLookup = new FileInfo(Path.Combine(WebTools.LookupsDir.FullName, "index.lookup"));

        static void Main(string[] args)
        {
            if (!_caption.Exists) throw new Exception("Could not find 'caption.xml' in current folder.");
            if (!_contact.Exists) throw new Exception("Could not find 'contact.xml' in current folder.");
            if (!_banner.Exists) throw new Exception("Could not find 'banner.xml' in current folder.");
            if (!_indexLookup.Exists) throw new Exception(@"Could not find 'index.lookup' in '.\lookups' folder.");

            using (XmlTextWriter output = new XmlTextWriter(Path.Combine(WebTools.Web.FullName, "index.html"), Encoding.UTF8))
            {
                output.Setup();

                output.WriteStartElement("html");

                output.WriteHeadElement("Home");
                
                output.WriteStartElement("body");


                #region Top Row - Content

                output.WriteStartElement("table");
                output.WriteAttributeString("class", "cell");
                output.WriteAttributeString("style", "width:100%");

                output.WriteStartElement("tr");
                output.WriteAttributeString("class", "cell");


                #region Left Cell - Contact

                output.WriteStartElement("td");
                output.WriteAttributeString("width", "50%");
                output.WriteAttributeString("border", "0");
                output.WriteAttributeString("cellpadding", "0");
                output.WriteAttributeString("valign", "middle");

                WebTools.Log("Reading contact from {0}", _contact.FullName);
                using (XmlTextReader input = new XmlTextReader(_contact.FullName))
                    output.WriteNode(input, false);

                output.WriteEndElement(); // td - left cell

                #endregion Left Cell - Contact


                #region Right Cell - Content

                output.WriteStartElement("td");
                output.WriteAttributeString("width", "50%");
                output.WriteAttributeString("border", "0");
                output.WriteAttributeString("cellpadding", "0");
                output.WriteAttributeString("valign", "middle");

                // Inner table
                output.WriteStartElement("table");
                output.WriteAttributeString("class", "cell");
                output.WriteAttributeString("style", "width:100%");


                #region Image Row

                output.WriteStartElement("tr");
                output.WriteAttributeString("class", "cell");
                output.WriteAttributeString("style", "width: 100%; height: 55%;");
                output.WriteStartElement("td");

                WebTools.WriteImage(output, "profile.jpg", _caption);

                output.WriteEndElement(); // td - image cell
                output.WriteEndElement(); // tr - image row

                #endregion Image Row


                #region Pages Row

                output.WriteStartElement("tr");
                output.WriteAttributeString("class", "cell");
                output.WriteAttributeString("style", "width: 100%; height: 45%;");
                output.WriteStartElement("td");
                //output.WriteAttributeString("width", "20%");
                //output.WriteAttributeString("border", "0");
                //output.WriteAttributeString("cellpadding", "0");
                output.WriteAttributeString("valign", "middle");

                output.WriteStartElement("div");
                output.WriteElementString("h2", "Pages");
                output.WriteStartElement("span");
                output.WriteAttributeString("class", "normal");
                
                #region Link to Pages

                using (StreamReader pagesLookup = new StreamReader(_indexLookup.FullName))
                {
                    string line;
                    while ((line = pagesLookup.ReadLine()) != null)
                    {
                        if (line.Length == 0)
                        {
                            output.WriteElementString("br", "");
                            continue;
                        }
                        if (line[0] == '#') continue;
                        if (line[0] == ';') continue;

                        string[] parts = line.Split('\t');

                        if (parts.Length == 2)
                        {
                            string display = parts[0];
                            string link = parts[1];

                            WebTools.Log("Linking to page {0}", link);
                            WebTools.WriteSmallLink(output, link, parts[0], 1, false);
                        }
                    }
                }


                #endregion

                output.WriteEndElement(); // span                    
                output.WriteEndElement(); // div

                output.WriteEndElement(); // td - pages cell
                output.WriteEndElement(); // tr - pages row

                #endregion Pages Row

                output.WriteEndElement(); // table - right cell internals

                output.WriteEndElement(); // td - right cell

                #endregion Right Cell - Content


                output.WriteEndElement(); // tr - main row

                output.WriteEndElement(); // table - main content

                #endregion Top Row - Content






                output.WriteElementString("br", "");
                output.WriteElementString("br", "");
                output.WriteElementString("br", "");
                output.WriteElementString("br", "");






                #region Second Row - Banner

                output.WriteStartElement("table");
                output.WriteAttributeString("class", "cell");

                output.WriteStartElement("tr");
                output.WriteAttributeString("class", "cell");


                #region Banner Cell

                output.WriteStartElement("td");

                WebTools.Log("Reading banner from {0}", _banner.FullName);
                using (XmlTextReader input = new XmlTextReader(_banner.FullName)) 
                    output.WriteNode(input, false);

                output.WriteEndElement(); // td

                #endregion Banner Cell


                output.WriteEndElement(); // tr - banner row

                output.WriteEndElement(); // table - banner content

                #endregion Second Row - Banner


                output.WriteElementString("br", "");
                output.WriteElementString("br", "");

                WebTools.WritePageEnd(output);
                





                output.WriteEndElement(); // body

                output.WriteEndElement(); //html
            }
        }
    }
}
