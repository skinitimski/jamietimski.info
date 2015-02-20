using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Atmosphere.WebTools
{
    public static partial class WebTools
    {
        private const string LOG_FORMAT = "\t{0}\t{1} : {2}";



        private static string GetTimeString()
        {
            DateTime now = DateTime.Now;

            string dt = String.Format("{0}/{1}/{2} {3}:{4}:{5}.{6}",
                now.Year.ToString("D4"),
                now.Month.ToString("D2"),
                now.Day.ToString("D2"),
                now.Hour.ToString("D2"),
                now.Minute.ToString("D2"),
                now.Second.ToString("D2"),
                now.Millisecond.ToString("D3"));

            return dt;
        }

        private static void Log(Assembly caller, string message, params object[] messageParts)
        {
            string logMessage = String.Format(LOG_FORMAT,
                GetTimeString(),
                caller.GetName().Name,
                String.Format(message, messageParts));

            Console.WriteLine(logMessage);
            Logger.WriteLine(logMessage);
            Logger.Flush();
        }

        private static StreamWriter Logger { get; set; }
    }
}
