using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OverworldObjects
{
    /// <summary>
    /// Interface to define all dwellings. A dwelling is a building of which
    /// units are made. Here you can purchase a set number of units per week.
    /// </summary>
    interface Dwelling
    {
        Unit Unit { get; set; }
        int UnitsPresent { get; set; }
        int UnitsPerWeek { get; set; }
    }
}
