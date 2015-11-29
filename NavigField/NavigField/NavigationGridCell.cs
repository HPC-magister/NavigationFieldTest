using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavigField
{
    public class NavigationGridCell : GridCell
    {
        public bool extended_alg;
        public double ak { get; set; }
        public bool isTraversable;
        public bool isAim;
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

        public int optimalXPredecessor { get; set; }
        public int optimalYPredecessor { get; set; }
        public double optimalExtendedXPredecessor { get; set; }
        public double optimalExtendedYPredecessor { get; set; }

        public int xPredecessor { get; set; }
        public int yPredecessor { get; set; }
        public double extendedXPredecessor { get; set; }
        public double extendedYPredecessor { get; set; }


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

        public struct CostCalculatingData_st {
            public int Ax;
            public int Ay;
            public int Bx;
            public int By;
            public double a;
        }
        public CostCalculatingData_st CostCalculatingData;

        public bool extendedCalculated;

        public NavigationGridCell()
        {
            calcIterationsPassed = 0;            
            pathCost = 0;
            isTraversable = true;
            ak = 10;
        }

        public int Update()
        {
            return 0;
        }
    }
}
