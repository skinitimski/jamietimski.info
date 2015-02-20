using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Atmosphere.WebTools
{
    public interface IEvent
    {
        int StartDate { get; }
        int EndDate { get; }

        string StartTime { get; }
        string EndTime { get; }

        string DescriptionShort { get; }

        string Title { get; }

        IList<string> DescriptionLong { get; }

        string Comments { get; }


    }
}
