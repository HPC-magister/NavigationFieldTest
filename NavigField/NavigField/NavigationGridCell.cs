using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavigField
{
    public class NavigationGridCell : GridCell
    {
        public int iterationsPassed { get; private set; }
        public double pathCost { get; private set; }

        public NavigationGridCell()
        {
            iterationsPassed = 0;
            pathCost = 0;
        }
    }
}
