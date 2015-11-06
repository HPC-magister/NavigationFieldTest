using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavigField
{
    public class GridCell
    {
        public int xPos { get; protected set; }
        public int yPos { get; protected set; }

        public double angle { get; protected set; }
        public double amplitude { get; protected set; }

        public bool isObstacle { get; set; }
               

        public GridCell(bool isObstcl = true, int x = 0, int y = 0, double angle_tmp = 0, double amplitude_tmp = 0)
        {
            isObstacle = isObstcl;
            xPos = x;
            yPos = y;
            angle = angle_tmp;
            amplitude = amplitude_tmp;
        }
    }
}

