using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml;

namespace Atmosphere.WebTools
{
    class GenerateGallery
    {
        static List<string> _validImages;

        static GenerateGallery()
        {
            _validImages = new List<string>();
            _validImages.Add(".jpg");
            _validImages.Add(".png");
            //_validImages.Add(".gif");
            //_validImages.Add(".bmp");
        }

        static void Main(string[] args)
        {
            if (args.Length != 1) throw new Exception("Must supply gallery root directory on command line.");

            DirectoryInfo images = new DirectoryInfo(args[0]);

            if (!images.Exists) throw new Exception(String.Format("Specified 'images' directory does not exist: {0}", images.FullName));



            ProcessFolder(images, WebTools.WebGalleryDir, "index");
        }        


        static void ProcessFolder(DirectoryInfo data, DirectoryInfo web, string parent)
        {
            WebTools.Log("Processing input directory {0}", data.FullName);

            if (!web.Exists) web.Create();

            using (XmlTextWriter output = new XmlTextWriter(Path.Combine(web.FullName, web.Name + ".html"), Encoding.UTF8))
            {
                string bannerPath = Path.Combine(data.FullName, "banner.xml");
                string propertiesPath = Path.Combine(data.FullName, "properties.txt");


                // Only process those directories with a properties.txt file
                if (File.Exists(propertiesPath))
                {
                    if (!File.Exists(bannerPath)) throw new ArgumentException(String.Format("Banner missing from directory '{0}'. Banner is required.", data.FullName));

                    PropertiesFile properties = new PropertiesFile(propertiesPath);

                    string galleryName = properties.GetProperty("name", data.Name);
                    bool suppressParentLink = properties.GetProperty("suppressParentLink", "false") == "false";


                    output.Setup();

                    output.WriteStartElement("html");

                    // header
                    output.WriteHeadElement(web.Name);


                    output.WriteStartElement("body");

                    output.WriteStartElement("table");
                    output.WriteAttributeString("width", "90%");

                    output.WriteStartElement("tbody");

                    output.WriteStartElement("tr");
                    output.WriteAttributeString("class", "cell");

                    output.WriteStartElement("td");

                    // title
                    output.WriteElementString("h1", galleryName);

                    WebTools.Log("\tReading banner from {0}...", bannerPath);
                    using (XmlTextReader input = new XmlTextReader(bannerPath)) output.WriteNode(input, false);

                    output.WriteElementString("br", "");
                    output.WriteElementString("br", "");


                    // Link Back
                    if (!suppressParentLink) output.WriteSmallLink("../" + parent + ".html", "Back to Parent");


                    // List nested galleries
                    DirectoryInfo[] dirs = data.GetDirectories();

                    if (dirs.Length > 0)
                    {
                        output.WriteElementString("h2", "Nested Galleries");

                        foreach (DirectoryInfo subDir in dirs)
                        {
                            string subDirPropertiesPath = Path.Combine(subDir.FullName, "properties.txt");
                            string nestedGalleryName = subDir.Name;

                            using (StreamReader reader = new StreamReader(subDirPropertiesPath))
                            {
                                string line = null;

                                while ((line = reader.ReadLine()) != null)
                                {
                                    string[] parts = line.Split('\t');

                                    if (parts.Length != 2) throw new ArgumentException(String.Format("Line in properties.txt not valid ({0}): {1}", data.Name, line));

                                    if (parts[0] == "name") nestedGalleryName = parts[1];
                                }
                            }

                            DirectoryInfo webSubDir = new DirectoryInfo(Path.Combine(web.FullName, subDir.Name));

                            output.WriteSmallLink(String.Format("{0}/{0}.html", subDir.Name), nestedGalleryName, 1, false);

                            ProcessFolder(subDir, webSubDir, web.Name);
                        }
                    }


                    //FileInfo[] imgs = Filter(data.GetFiles(), IsValidImage);
                    IEnumerable<FileInfo> imgsTemp = data.GetFiles();
                    imgsTemp = imgsTemp.Where((FileInfo x) => IsValidImage(x));
                    imgsTemp = imgsTemp.Where((FileInfo x) => !Path.GetFileNameWithoutExtension(x.Name).EndsWith("_orig"));

                    FileInfo[] imgs = imgsTemp.ToArray();

                    if (imgs.Length > 0)
                    {
                        output.WriteElementString("h2", "Images");

                        for (int i = 0; i < imgs.Length; i++)
                        {
                            WebTools.Log("\tProcessing image {0}", imgs[i].Name);

                            File.Copy(imgs[i].FullName, Path.Combine(web.FullName, imgs[i].Name));


                            FileInfo prevImage = imgs[(i + imgs.Length - 1) % imgs.Length];
                            FileInfo nextImage = imgs[(i + 1) % imgs.Length];

                            string imageName = Path.GetFileNameWithoutExtension(imgs[i].Name);
                            string prevImageName = Path.GetFileNameWithoutExtension(prevImage.Name);
                            string nextImageName = Path.GetFileNameWithoutExtension(nextImage.Name);


                            #region Image page

                            using (XmlTextWriter imgPage = new XmlTextWriter(Path.Combine(web.FullName, imageName + ".html"), Encoding.UTF8))
                            {
                                imgPage.Setup();

                                imgPage.WriteStartElement("html");

                                // header
                                imgPage.WriteHeadElement(imageName);


                                imgPage.WriteStartElement("body");

                                imgPage.WriteStartElement("table");
                                imgPage.WriteAttributeString("class", "cell");
                                imgPage.WriteAttributeString("width", "90%");

                                imgPage.WriteStartElement("tbody");



                                // First row -- links
                                imgPage.WriteStartElement("tr");
                                imgPage.WriteAttributeString("class", "cell");
                                imgPage.WriteStartElement("td");

                                imgPage.WriteLargeLink(prevImageName + ".html", "Prev", 0);
                                imgPage.WriteLargeLink(web.Name + ".html", "Back", 0);
                                imgPage.WriteLargeLink(nextImageName + ".html", "Next", 3);

                                imgPage.WriteEndElement(); // td
                                imgPage.WriteEndElement(); // tr



                                // Second row -- caption (if available)
                                string captionPath = Path.Combine(data.FullName, imageName + ".xml");

                                if (File.Exists(captionPath))
                                {
                                    imgPage.WriteStartElement("tr");
                                    imgPage.WriteAttributeString("class", "cell");
                                    imgPage.WriteStartElement("td");

                                    using (XmlTextReader input = new XmlTextReader(captionPath)) imgPage.WriteNode(input, false);

                                    imgPage.WriteEndElement(); // td
                                    imgPage.WriteEndElement(); // tr
                                }



                                // Third row -- image
                                imgPage.WriteStartElement("tr");
                                imgPage.WriteAttributeString("class", "cell");
                                imgPage.WriteStartElement("td");

                                imgPage.WriteStartElement("a");
                                imgPage.WriteAttributeString("href", nextImageName + ".html");
                                imgPage.WriteStartElement("img");
                                imgPage.WriteAttributeString("src", imgs[i].Name);
                                imgPage.WriteEndElement(); // img
                                imgPage.WriteEndElement(); // a

                                imgPage.WriteEndElement(); // td
                                imgPage.WriteEndElement(); // tr




                                imgPage.WriteEndElement(); // tbody
                                imgPage.WriteEndElement(); // table
                                imgPage.WriteEndElement(); // body
                                imgPage.WriteEndElement(); // html
                            }

                            #endregion image page


                            WebTools.WriteSmallLink(output, imageName + ".html", imgs[i].Name, 1, false);
                        }

                        output.WriteElementString("br", "");
                        output.WriteElementString("br", "");
                        output.WriteElementString("br", "");
                    }



                    output.WriteEndElement(); // td
                    output.WriteEndElement(); // tr
                    output.WriteEndElement(); // tbody
                    output.WriteEndElement(); // table
                    output.WriteEndElement(); // body
                    output.WriteEndElement(); // html
                }
            }
        }



        static void GenerateGalleryIndex()
        {
            using (XmlTextWriter output = new XmlTextWriter(Path.Combine(WebTools.WebGalleryDir.FullName, "gallery.html"), Encoding.UTF8))
            {
                output.Setup();

                output.WriteStartElement("html");
                
                // Header
                output.WriteHeadElement("Gallery");
                
                output.WriteStartElement("body");

                output.WriteElementString("h1", "Gallery");

                output.WriteElementString("br", "");
                output.WriteElementString("br", "");
                output.WriteElementString("br", "");

                output.WriteStartElement("div");
                output.WriteStartElement("font");
                output.WriteAttributeString("class", "normal");

                DirectoryInfo[] dirs = WebTools.WebGalleryDir.GetDirectories();

                if (dirs.Length > 0)
                {
                    output.WriteElementString("h2", "Galleries");

                    // Link to each directory
                    foreach (DirectoryInfo dir in dirs)
                    {
                        string url = WebTools.WebGalleryDir.FullName + "/" + dir.Name + "/" + dir.Name + ".html";

                        output.WriteSmallLink(url, dir.Name, 1, false);
                    }
                }

                output.WriteElementString("br", "");

                // Link Back
                output.WriteSmallLink("index.html", "Back to Home");

                output.WriteEndElement(); // font
                output.WriteEndElement(); // div
                output.WriteEndElement(); // body
                output.WriteEndElement(); //html
            }
        }

        static bool IsValidImage(FileInfo file)
        {
            return _validImages.Contains(file.Extension);
        }
    }
}
