using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavigField
{
    public class NavigationFieldSpace : FieldSpace
    {
        public new NavigationGridCell[,] NavigFieldArray { get; private set; }
        public double preferredVelocity { get; set; }
        public int calcIterationsPassed { get; set; }
        public int sleepTime;

        private delegate int Del(int xCurrent, int yCurrent, int xIsCalculated, int yIsCalculated);


        public NavigationFieldSpace(int xSize_tmp = 20, int ySize_tmp = 20)
        {
            xSize = xSize_tmp;
            ySize = ySize_tmp;

            Clear();        
        }

        public int Clear()
        {            
            NavigFieldArray = new NavigationGridCell[xSize, ySize];

            for (int i = 0; i < xSize; i++)
                for (int j = 0; j < ySize; j++)
                {
                    NavigFieldArray[i, j] = new NavigationGridCell();

                    this.NavigFieldArray[i, j].xPredecessor = i;
                    this.NavigFieldArray[i, j].yPredecessor = j;
                }

            this.calcIterationsPassed = 0;
            this.preferredVelocity = 1;
            this.sleepTime = 0;

            return 0;
        }

        public int ClearCalculations()
        {
            for (int i = 0; i < this.xSize; i++)
                for (int j = 0; j < this.ySize; j++)
                    this.NavigFieldArray[i, j].wasCalculated = false;
            this.calcIterationsPassed = 0;

            return 0;
        }

        public int CalculateFieldForAim(int xAimIndex, int yAimIndex, double preferredVelocity = 1, int calcIterations = 1, double initCost = 0)
        {
            this.ClearCalculations();
            this.NavigFieldArray[xAimIndex, yAimIndex].isAim = true;
            this.NavigFieldArray[xAimIndex, yAimIndex].pathCost = initCost;
            this.preferredVelocity = preferredVelocity;
            this.NavigFieldArray[xAimIndex, yAimIndex].calcIterationsPassed = 1;
            this.NavigFieldArray[xAimIndex, yAimIndex].wasCalculated = true;

            int maxDistToBorder = Math.Max(Math.Max((this.xSize - 1) - xAimIndex, xAimIndex),
                                           Math.Max((this.ySize - 1) - yAimIndex, yAimIndex));
                      
            this.Indexer(PathFinder, xAimIndex, yAimIndex, maxDistToBorder, calcIterations);


            return 0;
        }

        public override int GetCountOfActiveCells()
        {
            int countOfActive = 0;

            foreach (NavigationGridCell e in NavigFieldArray)
            {

                if (!e.isObstacle)
                    countOfActive++;
            }


            return countOfActive;
        }

        private int PathFinder(int xIsCalculated, int yIsCalculated, int param3, int param4)
        {
            this.Indexer(PredecessorFinder, xIsCalculated, yIsCalculated);
                
            int xPredecessor = this.NavigFieldArray[xIsCalculated, yIsCalculated].xPredecessor;

            int yPredecessor = this.NavigFieldArray[xIsCalculated, yIsCalculated].yPredecessor;

            this.NavigFieldArray[xIsCalculated, yIsCalculated].pathCost
                = this.CostFromThrough(xIsCalculated, yIsCalculated, xPredecessor, yPredecessor);           
                        
            this.NavigFieldArray[xIsCalculated, yIsCalculated].calcIterationsPassed++;
            System.Threading.Thread.Sleep(this.sleepTime);


            return 0;
        }

        private int PredecessorFinder(int xCurrent, int yCurrent, int xIsCalculated, int yIsCalculated)
        {
            int xPredecessor = this.NavigFieldArray[xIsCalculated, yIsCalculated].xPredecessor;
            int yPredecessor = this.NavigFieldArray[xIsCalculated, yIsCalculated].yPredecessor;

            if (this.NavigFieldArray[xCurrent, yCurrent].wasCalculated
                &&
                this.NavigFieldArray[xIsCalculated, yIsCalculated].wasCalculated
                &&
                this.CostFromThrough(xIsCalculated, yIsCalculated, xPredecessor, yPredecessor)
                > this.CostFromThrough(xIsCalculated, yIsCalculated, xCurrent, yCurrent)
                ||
                this.NavigFieldArray[xCurrent, yCurrent].wasCalculated
                &&
                !(this.NavigFieldArray[xIsCalculated, yIsCalculated].wasCalculated))
                    {
                        this.NavigFieldArray[xIsCalculated, yIsCalculated].xPredecessor = xCurrent;
                        this.NavigFieldArray[xIsCalculated, yIsCalculated].yPredecessor = yCurrent;
                        this.NavigFieldArray[xIsCalculated, yIsCalculated].wasCalculated = true;                        
                    }
            
                
            return 0;
        }

        private double CostFromThrough(int xIsCalculated, int yIsCalculated, int currentXPredecessor, int currentYPredecessor)
        {

            double cellDistance = Math.Sqrt(
                    Math.Pow(Math.Abs(xIsCalculated - currentXPredecessor), 2) +
                    Math.Pow(Math.Abs(yIsCalculated - currentYPredecessor), 2));

            double guidVector;
            double a = 0;
            double PI = Math.PI;

            if (currentXPredecessor - xIsCalculated != 0)
            {
                a = Math.Atan(
                (currentYPredecessor - yIsCalculated) /
                (currentXPredecessor - xIsCalculated));

                if (currentXPredecessor - xIsCalculated < 0)
                    a += Math.PI;
            }
            else
                a = Math.Asin((currentYPredecessor - yIsCalculated) / cellDistance);

            if (currentYPredecessor - yIsCalculated == 0)
                a = Math.Acos((currentXPredecessor - xIsCalculated) / cellDistance);

                this.NavigFieldArray[xIsCalculated, yIsCalculated].angle = a;
            a -= this.FieldArray[currentXPredecessor, currentYPredecessor].angle;

            guidVector = this.FieldArray[currentXPredecessor, currentYPredecessor].amplitude;

            double predCellTraversingVelocity = (( guidVector * Math.Cos(a) ) +
                Math.Sqrt(
                    Math.Pow(guidVector * Math.Cos(a), 2) -
                    Math.Pow(guidVector, 2) +
                    this.preferredVelocity));

            predCellTraversingVelocity = (predCellTraversingVelocity > 1) ? 1 : predCellTraversingVelocity;
            double predCellTraversingCost = (cellDistance / 2) / predCellTraversingVelocity;

            a = this.NavigFieldArray[xIsCalculated, yIsCalculated].angle;
            a -= this.FieldArray[xIsCalculated, yIsCalculated].angle;
            guidVector = this.FieldArray[xIsCalculated, yIsCalculated].amplitude;

            double cellTraversingVelocity = ((guidVector * Math.Cos(a)) +
                Math.Sqrt(
                    Math.Pow(guidVector * Math.Cos(a), 2) -
                    Math.Pow(guidVector, 2) +
                    this.preferredVelocity));

            cellTraversingVelocity = (cellTraversingVelocity > 1) ? 1 : cellTraversingVelocity;

            double cellTraversingCost = (cellDistance / 2) / cellTraversingVelocity;


            this.NavigFieldArray[xIsCalculated, yIsCalculated].amplitude = cellTraversingVelocity;


            return Math.Round((this.NavigFieldArray[currentXPredecessor, currentYPredecessor]
                    .pathCost + cellTraversingCost + predCellTraversingCost), 2);
        }

        private int Indexer(Del handler, int xAimIndex, int yAimIndex, int calculatingRadius = 1, int calcIterations = 1)
        {

            for (int iteration = 1; iteration <= calcIterations; iteration++)
            {
                if (handler == PathFinder)
                    this.calcIterationsPassed++;

                for (int radius = 1; radius <= calculatingRadius; radius++)
                {
                    int onSideCellsToDraw = 2 * radius;

                    int xBeginIndex = xAimIndex + radius;
                    int yBeginIndex = yAimIndex + radius;

                    int x = xBeginIndex, y = yBeginIndex;

                    int[] kxky = new int[] { 0, -1, 0, 1, 0 };


                    for (int rectSide = 0; rectSide < 4; rectSide++)
                    {
                        int kx = kxky[rectSide + 1], ky = kxky[rectSide];

                        for (int dl = 0; dl < onSideCellsToDraw; dl++)
                        {
                            x += kx; y += ky;

                            if ((0 <= x) && (x < this.NavigFieldArray.GetLength(0)) &&
                                (0 <= y) && (y < this.NavigFieldArray.GetLength(1)))
                            {

                                if (!this.NavigFieldArray[x, y].isObstacle)
                                {
                                    handler(x, y, xAimIndex, yAimIndex);
                                }
                            }
                        }
                    }
                }
            }


            return 0;
        }
        
    }
}
