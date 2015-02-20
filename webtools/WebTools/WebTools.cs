using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;

namespace Atmosphere.WebTools
{
    /// <summary>
    /// Shared code for all WebTools clients. Handles logging.
    /// </summary>
    public static partial class WebTools
    {


        #region Static Constructor

        /// <summary>
        /// Sets up the logger.
        /// </summary>
        static WebTools()
        {
            if (!Directory.Exists("logs")) Directory.CreateDirectory("logs");

            string file = "logs/";
            file += Assembly.GetEntryAssembly().GetName().Name;
            file += "-";
            file += GetTimeString().Replace("/", "").Replace(":", "").Replace(" ", "");
            file += ".log";

            Logger = new StreamWriter(file);
            Logger.AutoFlush = true;

            Web = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), @".www"));
            Data = new DirectoryInfo(Directory.GetCurrentDirectory());

            XsltArgList = new XsltArgumentList();
            XsltArgList.AddExtensionObject("urn:XsltExtensions", new XsltExtensions());


            PagesDir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "pages"));
            CalendarsDir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "calendars"));
            InsertDir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "insert"));
            LookupsDir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "lookups"));
            XsltDir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "xslt"));
            WebPagesDir = new DirectoryInfo(Path.Combine(Web.FullName, "pages"));
            WebGalleryDir = new DirectoryInfo(Path.Combine(Web.FullName, "gallery"));
            WebCalendarsDir = new DirectoryInfo(Path.Combine(Web.FullName, "calendars"));

            if (!CalendarsDir.Exists) throw new DirectoryNotFoundException("Could not find 'calendars' directory in current folder.");
            if (!LookupsDir.Exists) throw new DirectoryNotFoundException("Could not find 'lookup' directory in current folder.");
            if (!PagesDir.Exists) throw new DirectoryNotFoundException("Could not find 'pages' directory in current folder.");
            if (!XsltDir.Exists) throw new DirectoryNotFoundException("Could not find 'xslt' directory in current folder.");


            Log(Assembly.GetCallingAssembly(), "WebTools initiated successfully.");
        }

        #endregion Static Constructor



        #region Utility Methods

        public static void WriteHeadElement(this XmlWriter pageWriter, string pageName)
        {
            pageWriter.WriteStartElement("head");
            pageWriter.WriteElementString("title",
                String.Format("Timski - {0}", pageName));

            // stylesheet
            pageWriter.WriteStartElement("link");
            pageWriter.WriteAttributeString("rel", "stylesheet");
            pageWriter.WriteAttributeString("type", "text/css");
            pageWriter.WriteAttributeString("href", "style.css");
            pageWriter.WriteEndElement(); // link

            // favicon
            pageWriter.WriteStartElement("link");
            pageWriter.WriteAttributeString("rel", "shortcut icon");
            pageWriter.WriteAttributeString("type", "image/x-icon");
            pageWriter.WriteAttributeString("href", "favicon.ico");
            pageWriter.WriteEndElement(); // link

            pageWriter.WriteEndElement(); // head
        }




        #region Link Methods

        public static void WriteLargeLink(this XmlWriter pageWriter, string url, string text)
        {
            WriteLink(pageWriter, url, text, "large", 3, true);
        }
        public static void WriteLargeLink(this XmlWriter pageWriter, string url, string text, int breaks)
        {
            WriteLink(pageWriter, url, text, "large", breaks, true);
        }



        public static void WriteSmallLink(this XmlWriter pageWriter, string url, string text)
        {
            WriteLink(pageWriter, url, text, "small", 3, true);
        }
        public static void WriteSmallLink(this XmlWriter pageWriter, string url, string text, bool strong)
        {
            WriteLink(pageWriter, url, text, "small", 3, strong);
        }
        public static void WriteSmallLink(this XmlWriter pageWriter, string url, string text, int breaks)
        {
            WriteLink(pageWriter, url, text, "small", breaks, true);
        }
        public static void WriteSmallLink(this XmlWriter pageWriter, string url, string text, int breaks, bool strong)
        {
            WriteLink(pageWriter, url, text, "small", breaks, strong);
        }

        private static void WriteLink(this XmlWriter pageWriter, string url, string text, string @class, int breaks, bool strong)
        {
            if (strong) pageWriter.WriteStartElement("strong");

            pageWriter.WriteStartElement("a");
            if (!String.IsNullOrEmpty(@class)) pageWriter.WriteAttributeString("class", @class);
            pageWriter.WriteAttributeString("href", url);
            pageWriter.WriteString(text);
            pageWriter.WriteEndElement(); // a

            if (strong) pageWriter.WriteEndElement(); // strong

            pageWriter.WriteString("    ");

            for (; breaks > 0; breaks--) pageWriter.WriteElementString("br", "");
        }

        #endregion Link Methods




        public static void WriteImage(XmlWriter pageWriter, string src, FileInfo caption)
        {
            // Inner table
            pageWriter.WriteStartElement("table");
            pageWriter.WriteAttributeString("class", "cell");
            pageWriter.WriteAttributeString("style", "width:100%");

            #region Image Row

            pageWriter.WriteStartElement("tr");
            pageWriter.WriteAttributeString("class", "cell");
            pageWriter.WriteAttributeString("style", "width: 100%");

            pageWriter.WriteStartElement("td");
            pageWriter.WriteStartElement("img");
            pageWriter.WriteAttributeString("src", src);
            pageWriter.WriteString("");
            pageWriter.WriteEndElement(); // img
            pageWriter.WriteEndElement(); // td - image cell

            pageWriter.WriteEndElement(); // tr - image row

            #endregion Image Row


            #region Caption Row

            pageWriter.WriteStartElement("tr");
            pageWriter.WriteAttributeString("class", "cell");
            pageWriter.WriteAttributeString("style", "width:100%");

            pageWriter.WriteStartElement("td");
            WebTools.Log("Reading caption from {0}", caption.FullName);
            using (XmlTextReader input = new XmlTextReader(caption.FullName))
                pageWriter.WriteNode(input, false);
            pageWriter.WriteEndElement(); // td - caption cell

            pageWriter.WriteEndElement(); // tr - caption row

            #endregion Caption Row

            pageWriter.WriteEndElement(); // table
        }

        public static void WritePageEnd(this XmlWriter pageWriter)
        {
            WritePageEnd(pageWriter, "./", true);
        }
        public static void WritePageEnd(this XmlWriter pageWriter, bool centerTimestamp)
        {
            WritePageEnd(pageWriter, "./", centerTimestamp);
        }
        public static void WritePageEnd(this XmlWriter pageWriter, string pathToRoot, bool centerTimestamp)
        {
            pageWriter.WriteStartElement("div");
            if (centerTimestamp) pageWriter.WriteAttributeString("style", "text-align: center;");

            pageWriter.WriteStartElement("span");
            pageWriter.WriteAttributeString("class", "normal");

            pageWriter.WriteStartElement("em");
            pageWriter.WriteString(XsltExtensions.Timestamp());
            pageWriter.WriteEndElement(); // em

            pageWriter.WriteEndElement(); // span

            #region Write line breaks

            WriteLineBreaks(pageWriter, 55);

            #endregion

            pageWriter.WriteEndElement(); // div
        }
        public static void WriteLineBreaks(this XmlWriter writer, int count)
        {
            for ( int i = 0; i < count; i++) writer.WriteElementString("br", String.Empty);
        }

        public static void Setup(this XmlTextWriter pageWriter)
        {
            pageWriter.Formatting = Formatting.Indented;
            pageWriter.Indentation = 4;
            pageWriter.IndentChar = ' ';
        }

        public static void Log(string message, params object[] messageParts)
        {
            Log(Assembly.GetCallingAssembly(), message, messageParts);
        }





        public static T CreateObject<T>(Type type, object[] args) where T : class
        {
            return (T)CreateObject(type, args);
        }

        public static object CreateObject(Type type, object[] args)
        {
            object retVal = null;

            if (type == null) throw new ArgumentNullException("type", "Parameter 'type' cannot be null.");
            if (args == null) throw new ArgumentNullException("args", "Parameter 'args' cannot be null.");

            try
            {
                retVal = type.InvokeMember(String.Empty, BindingFlags.CreateInstance, null, null, args);
            }
            catch (Exception ex)
            {
                Exception toThrow = ex;

                if (ex is TargetInvocationException && ex.InnerException != null) toThrow = ex.InnerException;
                
                string msg = String.Format("Caught the following exception trying to instantiate an object of type '{0}' using the {1} constructor.\n{2}: {3}\n{4}",
                    type.Name,
                    GetConstructorArgsString(args),
                    toThrow.GetType().Name,
                    toThrow.Message,
                    toThrow.StackTrace);

                Log(msg, toThrow);

                throw toThrow;
            }

            return retVal;
        }



        /// <summary>
        /// Gets the names of the given enumeration of objects as a parenthetical list.
        /// </summary>
        private static string GetConstructorArgsString(IEnumerable<object> args)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("( ");

            foreach (object arg in args)
            {
                if (arg == null)
                {
                    sb.Append("[null], ");
                }
                else
                {
                    sb.AppendFormat("{0}, ", arg.GetType().Name);
                }
            }

            // remove trailing ", "
            if (sb.Length > 2) sb.Length = sb.Length - 2;

            sb.Append(" )");

            return sb.ToString();
        }














        #endregion Utility






        public static XsltArgumentList XsltArgList { get; private set; }

        /// <summary>
        /// Gets an object representing the root "Web" directory.
        /// </summary>
        public static DirectoryInfo Web { get; private set; }
        /// <summary>
        /// Gets an object representing the root "Data" directory.
        /// </summary>
        public static DirectoryInfo Data { get; private set; }
        /// <summary>
        /// Gets an object representing the raw pages directory.
        /// </summary
        public static DirectoryInfo PagesDir { get; private set; }
        /// <summary>
        /// Gets an object representing the raw calendars directory.
        /// </summary
        public static DirectoryInfo CalendarsDir { get; private set; }
        /// <summary>
        /// Gets an object representing the insert pages directory.
        /// </summary>
        public static DirectoryInfo InsertDir { get; private set; }
        /// <summary>
        /// Gets an object representing the raw lookup directory.
        /// </summary>
        public static DirectoryInfo LookupsDir { get; private set; }
        /// <summary>
        /// Gets an object representing the raw XSLT directory.
        /// </summary>
        public static DirectoryInfo XsltDir { get; private set; }
        /// <summary>
        /// Gets an object representing the web pages directory.
        /// </summary>
        public static DirectoryInfo WebPagesDir { get; private set; }
        /// <summary>
        /// Gets an object representing the web gallery directory.
        /// </summary>
        public static DirectoryInfo WebGalleryDir { get; private set; }
        /// <summary>
        /// Gets an object representing the web calendars directory.
        /// </summary>
        public static DirectoryInfo WebCalendarsDir { get; private set; }
    }
}
