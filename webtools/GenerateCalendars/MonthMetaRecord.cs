using System;
using System.Collections.Generic;
using System.Text;

namespace Atmosphere.WebTools
{
    public class MonthMetaRecord
    {
        private MonthMetaRecord() { }

        public MonthMetaRecord(int year, int month)
            : this()
        {
            MonthIndex = month;
            Year = year;


            StartDay = (int)(new DateTime(year, month, 1).DayOfWeek);
            Days = DateTime.DaysInMonth(year, month);
        }
        

        public int MonthIndex { get; private set; }
        public int Year { get; private set; }
        public int StartDay { get; private set; }
        public int Days { get; private set; }

        public string NameLong { get { return CalendarMonth.GetLongName(MonthIndex); } }

        public string NameShort { get { return CalendarMonth.GetShortName(MonthIndex); } }
    }
}
