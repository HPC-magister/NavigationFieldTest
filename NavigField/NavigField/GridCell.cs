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

        public bool isActive { get; set; }

        public GridCell()
        {
            isActive = false;
            xPos = 0;
            yPos = 0;
            angle = 0;
            amplitude = 0;
        }

        public GridCell(bool act, int x, int y, double angle_tmp, double amplitude_tmp)
        {
            isActive = act;
            xPos = x;
            yPos = y;
            angle = angle_tmp;
            amplitude = amplitude_tmp;
        }
    }
}

