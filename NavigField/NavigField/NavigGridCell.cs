using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavigField
{
    class NavigGridCell : GridCell
    {
        public int iterationsPassed { get; private set; }
        public double pathCost { get; private set; }

        public NavigGridCell()
        {
            iterationsPassed = 0;
            pathCost = 0;
            amplitude = 0;
        }
    }
}
