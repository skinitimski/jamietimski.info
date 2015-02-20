using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Atmosphere.WebTools
{
    public class BasicEvent : EventBase, IEvent
    {
        public BasicEvent(MonthMetaRecord month, string description)
            : base(month)
        {
            string[] parts = description.Split(new char[] { '\t' });

            if (parts.Length == 4)
            {
                int index = parts[0].IndexOf('-');

                if (index >= 0)
                {
                    StartDate = Int32.Parse(parts[0].Substring(0, index));
                    EndDate = Int32.Parse(parts[0].Substring(index + 1));
                }
                else
                {
                    StartDate = Int32.Parse(parts[0]);
                    EndDate = StartDate;
                }

                DescriptionShort = parts[1];
                Title = parts[2];
                Comments = parts[3];
            }
            else
            {
                WebTools.Log(description);

                throw new ArgumentException(
                    "BasicEvent string not valid. Must be one of the following forms: \n" +
                    "    startDate[-endDate] <> descriptionShort <> descriptionLong <> comments");
            }
        }




        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public string StartTime { get { return "00:00 AM"; } }

        public string EndTime { get { return StartTime; } }

        public string DescriptionShort { get; private set; }

        public string Title { get; private set; }

        public string Comments { get; private set; }

        public IList<string> DescriptionLong
        {
            get
            {
                List<string> description = new List<string>();

                return description;
            }
        }
    }
}
