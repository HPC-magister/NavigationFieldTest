using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavigField
{
    class Field
    {

        double angle;
        double cost;
        int xPos;
        int yPos;
        int xPredecessor; 
        int yPredecessor;
        bool isActive;

        public Field(bool act, int x, int y, double angle_tmp, double cost_tmp, int xPredecessor_tmp, int yPredecessor_tmp)
        {
            isActive = act;
            Update(act, x, y, angle_tmp, cost_tmp, xPredecessor_tmp, yPredecessor_tmp);
        }

        public int Update(bool isAct, int x, int y, double angle_tmp, double cost_tmp, int xPredecessor_tmp, int yPredecessor_tmp)
        {
            isActive = isAct;
            xPos = x;
            yPos = y;
            angle = angle_tmp;
            cost = cost_tmp;
            xPredecessor = xPredecessor_tmp;
            yPredecessor = yPredecessor_tmp;

            return 0;
        }

        public bool IsActive()
        {
            return isActive;
        }

        public double GetAngle()
        {
            return angle;
        }

        public double GetCost()
        {
            return cost;
        }

        public int GetXPos()
        {
            return xPos;
        }

        public int GetYPos()
        {
            return yPos;
        }

        public int GetXPredecessor()
        {
            return xPredecessor;
        }

        public int GetYPredecessor()
        {
            return yPredecessor;
        }

    }
}

