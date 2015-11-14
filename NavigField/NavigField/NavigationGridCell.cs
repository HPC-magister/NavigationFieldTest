using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavigField
{
    public class NavigationGridCell : GridCell
    {
        public bool wasCalculated;
        public int calcIterationsPassed { get; set; }
        private double pathCst;
        public double pathCost
        {
            get
            {
                return pathCst;
            }
            set
            {
                pathCst = value;
                if (pathCst == Double.PositiveInfinity)
                    isObstcl = true;
                else
                    isObstcl = false;
            }   
        }

        public int xPredecessor { get; set; }
        public int yPredecessor { get; set; }

        private bool isObstcl;
        public new bool isObstacle
        {
            get { return isObstcl; }
            set
            {
                isObstcl = value;
                if (isObstcl)
                    pathCst = Double.PositiveInfinity;
                else
                    pathCst = 0;
            }
        }


        public NavigationGridCell()
        {
            calcIterationsPassed = 0;            
            pathCost = 0;
        }

        public int Update()
        {
            return 0;
        }
    }
}
