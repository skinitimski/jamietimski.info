using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Atmosphere.WebTools
{
    public class Gig : EventBase, IEvent
    {
        public Gig(MonthMetaRecord month, string description)
            : base(month)
        {
            string[] parts = description.Split(new char[] { '\t' });

            if (parts.Length == 6)
            {
                StartDate = Int32.Parse(parts[0]);
                StartTime = parts[1];
                VenueShort = parts[2];
                Venue = parts[3];
                Location = parts[4];
                Band = parts[5];
            }
            else if (parts.Length == 7)
            {
                StartDate = Int32.Parse(parts[0]);
                StartTime = parts[1];
                VenueShort = parts[2];
                Venue = parts[3];
                Location = parts[4];
                Band = parts[5];
                EventName = parts[6];
            }
            else
            {
                WebTools.Log(description);

                throw new ArgumentException(
                    "Gig string not valid. Must be one of the following forms: \n" +
                    "    date<>time<>venueShort<>venue<>location<>band\n" +
                    "    date<>time<>venueShort<>venue<>location<>band<>name", "gig");
            }
        }


        public string VenueShort { get; private set; }
        public string Venue { get; private set; }
        public string Location { get; private set; }
        public string Band { get; private set; }
        public string EventName { get; private set; }
        


        public int StartDate { get; private set; }

        public int EndDate { get { return StartDate; } }

        public string StartTime { get; private set; }

        public string EndTime { get { return StartTime; } }

        public string DescriptionShort { get { return "@ " + VenueShort; } }

        public string Title { get { return EventName ?? Band; } }

        public string Comments { get { return String.Empty; } }

        public IList<string> DescriptionLong
        {
            get
            {
                List<string> description = new List<string>();

                if (EventName != null) description.Add(Band);

                description.Add(String.Format("{0} {1}, {2} @ {3}", Month.NameLong, StartDate, Month.Year, StartTime));

                description.Add(Venue);

                description.Add(Location);

                return description;
            }
        }
    }
}
