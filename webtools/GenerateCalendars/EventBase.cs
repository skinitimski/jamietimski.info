using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Atmosphere.WebTools
{
    public abstract class EventBase
    {
        private EventBase() { }

        protected EventBase(MonthMetaRecord month)
        {
            Month = month;
        }

        protected MonthMetaRecord Month { get; private set; }
    }
}
