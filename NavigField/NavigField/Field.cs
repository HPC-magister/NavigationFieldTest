using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavigField
{
    class Field
    {

        private double angle;
        private double cost;
        private int xPos;
        private int yPos;

        private bool isActive;

        public Field(bool act, int x, int y, double angle_tmp, double cost_tmp)
        {
            isActive = act;
            Update(act, x, y, angle_tmp, cost_tmp);
        }

        public int Update(bool isAct, int x, int y, double angle_tmp, double cost_tmp)
        {
            isActive = isAct;
            xPos = x;
            yPos = y;
            angle = angle_tmp;
            cost = cost_tmp;

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
        

    }
}

