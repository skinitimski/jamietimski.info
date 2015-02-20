using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Atmosphere.WebTools
{
    class GenerateCalendars
    {
        private const int YEAR_MIN = 1800;
        private const int YEAR_MAX = 2020;

        private const int BREAKS = 6;

        private static FileInfo _monthColorLookup = new FileInfo(Path.Combine(WebTools.LookupsDir.FullName, "monthColor.lookup"));

        private static Dictionary<string, string> _monthColor;

        static void Main(string[] args)
        {
            if (!_monthColorLookup.Exists) throw new Exception(@"Could not find 'monthColor.lookup' in '.\lookups' folder.");

            // Do that awesome thang you do
            GenerateMonthColorLookup();
            GenerateCalendarPages();
        }


        static void GenerateMonthColorLookup()
        {
            _monthColor = new Dictionary<string,string>();

            using (StreamReader reader = new StreamReader(_monthColorLookup.FullName))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line == "") continue;
                    if (line[0] == '#') continue;
                    if (line[0] == ';') continue;

                    string[] parts = line.Split('\t');

                    _monthColor.Add(parts[0], parts[1]);
                }
            }
        }


        static void GenerateCalendarPages()
        {
            WebTools.Log("Generating calendar pages...");

            DirectoryInfo[] calendarDirectories = WebTools.CalendarsDir.GetDirectories();

            foreach (DirectoryInfo calendarDirectory in calendarDirectories)
            {
                string calendarName = calendarDirectory.Name;
                string calendarPageName = String.Format("{0}.html", calendarName);

                WebTools.Log("  Processing calendar: {0}", calendarName);

                DirectoryInfo webCalendarDir = new DirectoryInfo(Path.Combine(WebTools.WebCalendarsDir.FullName, calendarName));
                DirectoryInfo webEventsDir = new DirectoryInfo(Path.Combine(webCalendarDir.FullName, "events"));

                FileInfo propertiesFile = new FileInfo(Path.Combine(calendarDirectory.FullName, "properties.txt"));

                if (propertiesFile.Exists)
                {
                    PropertiesFile properties = new PropertiesFile(propertiesFile);

                    if (!webCalendarDir.Exists) webCalendarDir.Create();
                    if (!webEventsDir.Exists) webEventsDir.Create();

                    string title = properties["title"];
                    string eventTypeString = properties["eventType"];

                    Type eventType = Type.GetType(eventTypeString);

                    if (eventType == null)
                    {
                        string msg = String.Format("Specified type is unknown to this assembly: {0}", eventTypeString);
                        WebTools.Log(msg);
                        throw new Exception(msg);
                    }




                    #region Write Calendars

                    using (XmlTextWriter calIndex = new XmlTextWriter(Path.Combine(WebTools.WebCalendarsDir.FullName, calendarPageName), Encoding.UTF8))
                    {
                        int yearHOLD = 0;

                        calIndex.Setup();

                        calIndex.WriteStartDocument();

                        calIndex.WriteDocType("html", "-//W3C//DTD XHTML 1.0 Strict//EN", null, null);

                        calIndex.WriteStartElement("html");

                        calIndex.WriteAttributeString("xmlns", "http://www.w3.org/1999/xhtml");

                        #region Header

                        calIndex.WriteHeadElement("Calendars");

                        #endregion Header

                        calIndex.WriteStartElement("body");


                        calIndex.WriteStartElement("div");
                        calIndex.WriteAttributeString("style", "text-align: center;");

                        calIndex.WriteElementString("h1", title);

                        calIndex.WriteStartElement("font");
                        calIndex.WriteAttributeString("class", "normal");





                        #region Month-By-Month

                        for (int year = YEAR_MIN; year < YEAR_MAX; year++)
                        {
                            for (int month = 1; month < 13; month++)
                            {
                                MonthMetaRecord monthMeta = new MonthMetaRecord(year, month);

                                string color = _monthColor[monthMeta.MonthIndex.ToString()];

                                string monthName = CalendarMonth.GetLongName(monthMeta.MonthIndex);
                                string monthAbbr = CalendarMonth.GetShortName(monthMeta.MonthIndex);

                                string eventFile = Path.Combine(calendarDirectory.FullName, monthAbbr.ToLower() + year + ".txt");

                                if (File.Exists(eventFile))
                                {
                                    if (year != yearHOLD)
                                    {
                                        yearHOLD = year;

                                        calIndex.WriteLineBreaks(2);
                                        calIndex.WriteElementString("h2", year.ToString());
                                    }


                                    calIndex.WriteLineBreaks(1);

                                    string monthPageFilename = String.Format("{0}{1}.html", monthAbbr.ToLower(), year.ToString());

                                    calIndex.WriteStartElement("a");
                                    calIndex.WriteAttributeString("href", String.Format("{0}/{1}", calendarName, monthPageFilename));
                                    calIndex.WriteString(monthName + " " + year.ToString());
                                    calIndex.WriteEndElement(); // a


                                    int curDay = 0;
                                    int eventIndex = 0;



                                    IEnumerable<string> eventDescriptions = File.ReadAllLines(eventFile);

                                    // Remove empty lines, comments
                                    eventDescriptions = eventDescriptions.Where(x => !String.IsNullOrEmpty(x));
                                    eventDescriptions = eventDescriptions.Where(x => x[0] != '#');
                                    eventDescriptions = eventDescriptions.Where(x => x[0] != ';');

                                    List<IEvent> events = eventDescriptions.Select(x => WebTools.CreateObject<IEvent>(eventType, new object[] { monthMeta, x })).ToList();

                                    events.Sort(CompareEvents);




                                    using (XmlTextWriter calWriter = new XmlTextWriter(Path.Combine(webCalendarDir.FullName, monthPageFilename), Encoding.UTF8))
                                    {
                                        WebTools.Log("{0,9} {1} -> Events: {2}", monthName, year, events.Count);

                                        calWriter.Setup();

                                        calWriter.WriteStartDocument();

                                        calWriter.WriteDocType("html", "-//W3C//DTD XHTML 1.0 Strict//EN", null, null);

                                        calWriter.WriteStartElement("html");
                                        calWriter.WriteAttributeString("xmlns", "http://www.w3.org/1999/xhtml");

                                        #region Header

                                        calWriter.WriteHeadElement(monthName);

                                        #endregion Header

                                        #region Body

                                        calWriter.WriteStartElement("body");

                                        calWriter.WriteStartElement("div");
                                        calWriter.WriteAttributeString("style", "text-align: center;");

                                        calWriter.WriteStartElement("span");
                                        calWriter.WriteAttributeString("style", String.Format("font-family: Verdana; color: #{0}; font-size: 18pt;", color));

                                        calWriter.WriteStartElement("strong");
                                        calWriter.WriteString(monthName);
                                        calWriter.WriteEndElement();

                                        calWriter.WriteEndElement(); // span

                                        calWriter.WriteElementString("br", String.Empty);
                                        calWriter.WriteElementString("br", String.Empty);

                                        calWriter.WriteStartElement("span");
                                        calWriter.WriteAttributeString("style", String.Format("font-family: Verdana; color: #{0}; font-size: 10pt;", color));

                                        calWriter.WriteStartElement("em");
                                        calWriter.WriteStartElement("strong");
                                        calWriter.WriteString("Click on an event to get the details!");
                                        calWriter.WriteEndElement();
                                        calWriter.WriteEndElement();

                                        calWriter.WriteEndElement(); // span

                                        calWriter.WriteElementString("br", String.Empty);
                                        calWriter.WriteElementString("br", String.Empty);

                                        calWriter.WriteEndElement(); // div

                                        calWriter.WriteStartElement("table");
                                        calWriter.WriteAttributeString("class", "calendar");
                                        calWriter.WriteAttributeString("width", "75%");
                                        calWriter.WriteAttributeString("border", "1");
                                        calWriter.WriteAttributeString("bordercolordark", "#000000");
                                        calWriter.WriteAttributeString("bordercolorlight", "#ffffff");
                                        calWriter.WriteAttributeString("cellpadding", "5");

                                        calWriter.WriteStartElement("tbody");
                                        calWriter.WriteStartElement("tr");

                                        WriteHeader(calWriter, "Sunday");
                                        WriteHeader(calWriter, "Monday");
                                        WriteHeader(calWriter, "Tuesday");
                                        WriteHeader(calWriter, "Wednesday");
                                        WriteHeader(calWriter, "Thursday");
                                        WriteHeader(calWriter, "Friday");
                                        WriteHeader(calWriter, "Saturday");

                                        calWriter.WriteEndElement(); // tr


                                        #region Cells

                                        for (int i = 0; i < 6; i++)
                                        {
                                            if (curDay > monthMeta.Days) break;

                                            calWriter.WriteStartElement("tr");

                                            for (int curDayOfWeek = 0; curDayOfWeek < 7; curDayOfWeek++)
                                            {
                                                calWriter.WriteStartElement("td");

                                                WriteCellAttributes(calWriter);

                                                if (curDay == 0 && curDayOfWeek == monthMeta.StartDay) curDay = 1;

                                                if (curDay > 0 && curDay <= monthMeta.Days)
                                                {
                                                    calWriter.WriteStartElement("span");
                                                    calWriter.WriteAttributeString("style", "color: #ff0055");
                                                    calWriter.WriteString(curDay.ToString());
                                                    calWriter.WriteEndElement(); // span
                                                    calWriter.WriteElementString("br", String.Empty);

                                                    int eventCount = 0;

                                                    while (eventIndex < events.Count && events[eventIndex].StartDate < curDay) eventIndex++;
                                                    while (eventIndex < events.Count && events[eventIndex].StartDate == curDay)
                                                    {
                                                        IEvent @event = events[eventIndex];

                                                        string eventPageFilename = String.Format("{0}{1}-{2}.html", monthAbbr.ToLower(), year, eventIndex);

                                                        calWriter.WriteStartElement("span");
                                                        calWriter.WriteAttributeString("class", "gig");
                                                        calWriter.WriteStartElement("a");
                                                        calWriter.WriteAttributeString("href", String.Format("events/{1}", calendarName, eventPageFilename));
                                                        calWriter.WriteString(@event.DescriptionShort);
                                                        calWriter.WriteEndElement(); // a
                                                        calWriter.WriteEndElement(); // span
                                                        calWriter.WriteElementString("br", String.Empty);

                                                        using (XmlTextWriter gigWriter = new XmlTextWriter(Path.Combine(webEventsDir.FullName, eventPageFilename), Encoding.UTF8))
                                                        {
                                                            gigWriter.Setup();

                                                            gigWriter.WriteStartDocument();

                                                            gigWriter.WriteDocType("html", "-//W3C//DTD XHTML 1.0 Strict//EN", null, null);

                                                            gigWriter.WriteStartElement("html");
                                                            gigWriter.WriteAttributeString("xmlns", "http://www.w3.org/1999/xhtml");

                                                            gigWriter.WriteHeadElement(monthName);


                                                            gigWriter.WriteStartElement("body");


                                                            //string title = @event.EventName == null ? @event.Band : @event.EventName;

                                                            gigWriter.WriteElementString("h2", @event.Title);

                                                            gigWriter.WriteStartElement("ul");
                                                            gigWriter.WriteAttributeString("style", "list-style-type: none; margin-left: -2.5em");


                                                            foreach (string line in @event.DescriptionLong)
                                                            {
                                                                gigWriter.WriteStartElement("li");
                                                                gigWriter.WriteElementString("span", line);
                                                                gigWriter.WriteEndElement(); // li
                                                            }


                                                            gigWriter.WriteEndElement(); // ul

                                                            gigWriter.WriteLineBreaks(2);

                                                            if (!String.IsNullOrEmpty(@event.Comments))
                                                            {
                                                                gigWriter.WriteElementString("span", @event.Comments);

                                                                gigWriter.WriteLineBreaks(2);
                                                            }

                                                            #region Footer

                                                            gigWriter.WriteStartElement("span");
                                                            gigWriter.WriteSmallLink(String.Format("../{0}", monthPageFilename), String.Format("Back to {0} {1}", monthName, year), 1);
                                                            gigWriter.WriteEndElement(); // span

                                                            gigWriter.WriteStartElement("span");
                                                            gigWriter.WriteSmallLink(String.Format("../../{0}", calendarPageName), "Back to Calendar", 1);
                                                            gigWriter.WriteEndElement(); // span

                                                            gigWriter.WriteStartElement("span");
                                                            gigWriter.WriteSmallLink("../../../", "Back to Home");
                                                            gigWriter.WriteEndElement(); // span

                                                            gigWriter.WritePageEnd(false);

                                                            #endregion Footer


                                                            gigWriter.WriteEndElement(); // body

                                                            gigWriter.WriteEndDocument();
                                                        }

                                                        eventIndex++;
                                                        eventCount++;

                                                    } // end of gig processing

                                                    // up to 5 <br/>'s because the first always comes after the cal day
                                                    for (int k = eventCount; k < BREAKS; k++)
                                                    {
                                                        calWriter.WriteElementString("br", String.Empty);
                                                    }

                                                    eventCount = 0;
                                                    curDay++;

                                                } // end if (curDay > 0)
                                                else
                                                {
                                                    calWriter.WriteLineBreaks(BREAKS);
                                                }

                                                calWriter.WriteEndElement(); // td

                                            } // end for (int curDayOfWeek = 0; curDayOfWeek < 7; curDayOfWeek++)


                                            calWriter.WriteEndElement(); // tr
                                        }

                                        #endregion Cells


                                        calWriter.WriteEndElement(); // tbody

                                        calWriter.WriteEndElement(); // table


                                        #region Footer

                                        calWriter.WriteLineBreaks(2);

                                        calWriter.WriteStartElement("div");
                                        calWriter.WriteAttributeString("style", "text-align: center;");

                                        calWriter.WriteLargeLink(String.Format("../{0}", calendarPageName), "Back to Calendar", 2);
                                        calWriter.WriteLargeLink("../../", "Back to Home");

                                        calWriter.WritePageEnd();

                                        calWriter.WriteEndElement(); // div

                                        #endregion Footer


                                        calWriter.WriteEndElement(); // body

                                        #endregion Body

                                        calWriter.WriteEndElement(); // html

                                        calWriter.WriteEndDocument();

                                        calWriter.Close();
                                    }
                                }

                            } // end while ((monthMeta = calendarData.ReadLine()) != null)
                        }

                        #endregion Month-By-Month

                        calIndex.WriteEndElement(); // font

                        #region Footer

                        calIndex.WriteLineBreaks(4);

                        WebTools.WriteLargeLink(calIndex, "../", "Back to Home");

                        WebTools.WritePageEnd(calIndex);

                        calIndex.WriteEndElement(); // div

                        #endregion Footer

                        calIndex.WriteEndElement(); // body

                        calIndex.WriteEndElement(); // html

                        calIndex.WriteEndDocument();

                        calIndex.Close();

                    } // end using calIndex

                    #endregion Write Calendars
                }
            }
        }
        


        private static void WriteCellAttributes(XmlTextWriter w)
        {
            w.WriteAttributeString("width", "14%");
            w.WriteAttributeString("border", "1");
            w.WriteAttributeString("bordercolordark", "#000000");
            w.WriteAttributeString("bordercolorlight", "#ffffff");
            w.WriteAttributeString("cellpadding", "5");
        }
        private static void WriteHeader(XmlTextWriter w, string header)
        {
            w.WriteStartElement("th");
            w.WriteAttributeString("vAlign", "center");
            w.WriteAttributeString("align", "middle");
            w.WriteAttributeString("width", "14%");
            w.WriteAttributeString("bgcolor", "#000000");

            w.WriteStartElement("font");
            w.WriteAttributeString("face", "Verdana");
            w.WriteAttributeString("color", "#ffffff");
            w.WriteAttributeString("style", "font-size: 10pt;");
            w.WriteString(header);
            w.WriteEndElement(); // font

            w.WriteEndElement(); // th
        }


        private static int CompareEvents(IEvent left, IEvent right)
        {
            int comp = 0;

            if (left == null && right == null)
            {
                comp = 0;
            }
            else if (left == null)
            {
                comp = -1;
            }
            else if (right == null)
            {
                comp = 1;
            }
            else
            {
                if (left.StartDate < right.StartDate)
                {
                    comp = -1;
                }
                else if (left.StartDate > right.StartDate)
                {
                    comp = 1;
                }
                else
                {
                    bool xpm = left.StartTime.ToUpper().EndsWith("PM");
                    bool ypm = right.StartTime.ToUpper().EndsWith("PM");

                    if ((xpm && ypm) || !(xpm || ypm))
                    {
                        comp = String.Compare(left.StartTime, right.StartTime);
                    }
                    else if (xpm)
                    {
                        comp = 1;
                    }
                    else
                    {
                        comp = -1;
                    }
                }
            }

            return comp;
        }


    }
}
