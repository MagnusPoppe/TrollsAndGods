using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OverworldObjects
{
    interface Dwelling
    {
        Unit Unit { get; set; }
        int UnitsPresent { get; set; }
        int UnitsPerWeek { get; set; }
    }
}
