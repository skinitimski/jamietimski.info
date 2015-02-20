using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Atmosphere.WebTools
{
    public static class CalendarMonth
    {
        /// <summary>
        /// Gets the full name of given month.
        /// </summary>
        /// <param name="month">1-based index of the month you want the name of.</param>
        /// <returns>A string with the full/long name for specified month.</summary>
        public static string GetLongName(int month)
        {
            if (month < 1 || month > 12) throw new ArgumentOutOfRangeException("month", month, "Month must be between 1 and 12 (inclusive).");

            string longName = null;

            switch (month)
            {
                case 1: longName = "January"; break;
                case 2: longName = "February"; break;
                case 3: longName = "March"; break;
                case 4: longName = "April"; break;
                case 5: longName = "May"; break;
                case 6: longName = "June"; break;
                case 7: longName = "July"; break;
                case 8: longName = "August"; break;
                case 9: longName = "September"; break;
                case 10: longName = "October"; break;
                case 11: longName = "November"; break;
                case 12: longName = "December"; break;
            }

            return longName;
        }
        
        /// <summary>
        /// Gets the short name of given month.
        /// </summary>
        /// <param name="month">1-based index of the month you want the abbreviation of.</param>
        /// <returns>A 3-letter abbreviation for specified month.</summary>
        public static string GetShortName(int month)
        {
            if (month < 1 || month > 12) throw new ArgumentOutOfRangeException("month", month, "Month must be between 1 and 12 (inclusive).");

            string shortName = null;

            switch (month)
            {
                case 1: shortName = "Jan"; break;
                case 2: shortName = "Feb"; break;
                case 3: shortName = "Mar"; break;
                case 4: shortName = "Apr"; break;
                case 5: shortName = "May"; break;
                case 6: shortName = "Jun"; break;
                case 7: shortName = "Jul"; break;
                case 8: shortName = "Aug"; break;
                case 9: shortName = "Sep"; break;
                case 10: shortName = "Oct"; break;
                case 11: shortName = "Nov"; break;
                case 12: shortName = "Dec"; break;
            }

            return shortName;
        }


    }
}
