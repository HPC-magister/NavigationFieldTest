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
        int[] kxky = new int[] { 0, -1, 0, 1, 0, -1 };
        public int calcIterationsPassed { get; set; }
        public int sleepTime;

        private delegate int Del(int xCurrent, int yCurrent, int xIsCalculated, int yIsCalculated);


        public NavigationFieldSpace(int xSize_tmp = 100, int ySize_tmp =100)
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

                    this.NavigFieldArray[i, j].optimalXPredecessor = i;
                    this.NavigFieldArray[i, j].optimalYPredecessor = j;
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
                      
            this.Indexer(PathFinder, xAimIndex, yAimIndex, maxDistToBorder, 15);
            this.Indexer(ExtendedPredecessorFinder, xAimIndex, yAimIndex, maxDistToBorder, calcIterations);


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


        private int Indexer(Del handler, int xAimIndex, int yAimIndex, int calculatingRadius = 1, int calcIterations = 1)
        {

            for (int iteration = 1; iteration <= calcIterations; iteration++)
            {
                if (handler == PathFinder || handler == ExtendedPredecessorFinder)
                    this.calcIterationsPassed++;

                for (int radius = 1; radius <= calculatingRadius; radius++)
                {
                    int onSideCellsToDraw = 2 * radius;

                    int xBeginIndex = xAimIndex + radius;
                    int yBeginIndex = yAimIndex + radius;

                    int x = xBeginIndex, y = yBeginIndex;


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
       
        private int PathFinder(int xIsCalculated, int yIsCalculated, int param3, int param4)
        {
            

            if (!this.NavigFieldArray[xIsCalculated, yIsCalculated].extended_alg)
            {
                this.Indexer(PredecessorFinder, xIsCalculated, yIsCalculated);
                               
                this.NavigFieldArray[xIsCalculated, yIsCalculated].pathCost
                    = this.EvaluatingCell(
                        xIsCalculated, 
                        yIsCalculated,
                    this.NavigFieldArray[xIsCalculated, yIsCalculated].optimalXPredecessor,
                    this.NavigFieldArray[xIsCalculated, yIsCalculated].optimalYPredecessor);
            }
            else
            {

            }
                      
            this.NavigFieldArray[xIsCalculated, yIsCalculated].calcIterationsPassed++;
            //System.Threading.Thread.Sleep(this.sleepTime);


            return 0;
        }

        private int PredecessorFinder(int xCurrent, int yCurrent, int xIsCalculated, int yIsCalculated)
        {
            int xPredecessor = this.NavigFieldArray[xIsCalculated, yIsCalculated].optimalXPredecessor;
            int yPredecessor = this.NavigFieldArray[xIsCalculated, yIsCalculated].optimalYPredecessor;

            if (xIsCalculated == xCurrent || yIsCalculated == yCurrent)
            if (this.NavigFieldArray[xCurrent, yCurrent].wasCalculated
                &&
                this.NavigFieldArray[xIsCalculated, yIsCalculated].wasCalculated
                &&
                this.EvaluatingCell(xIsCalculated, yIsCalculated, xPredecessor, yPredecessor)
                > this.EvaluatingCell(xIsCalculated, yIsCalculated, xCurrent, yCurrent)
                ||
                this.NavigFieldArray[xCurrent, yCurrent].wasCalculated
                &&
                !(this.NavigFieldArray[xIsCalculated, yIsCalculated].wasCalculated))
                    {
                        this.NavigFieldArray[xIsCalculated, yIsCalculated].optimalXPredecessor = xCurrent;
                        this.NavigFieldArray[xIsCalculated, yIsCalculated].optimalYPredecessor = yCurrent;
                        this.NavigFieldArray[xIsCalculated, yIsCalculated].wasCalculated = true;                        
                    }
            
                
            return 0;
        }

   
        private double EvaluatingCell(int xIsCalculated, int yIsCalculated, 
                                      int currentXPredecessor, int currentYPredecessor)
        {
            
            double aimCellDistance = Math.Sqrt(
                    Math.Pow(Math.Abs(xIsCalculated - currentXPredecessor), 2) +
                    Math.Pow(Math.Abs(yIsCalculated - currentYPredecessor), 2));

            double guidVector;
            double a = 0;

            if (currentXPredecessor - xIsCalculated != 0)
            {
                a = Math.Atan(
                (currentYPredecessor - yIsCalculated) /
                (currentXPredecessor - xIsCalculated));

                if (currentXPredecessor - xIsCalculated < 0)
                    a += Math.PI;
            }
            else
                a = Math.Asin((currentYPredecessor - yIsCalculated) / aimCellDistance);

            if (currentYPredecessor - yIsCalculated == 0)
                a = Math.Acos((currentXPredecessor - xIsCalculated) / aimCellDistance);

                
            double navDirect = a;
            a -= this.FieldArray[currentXPredecessor, currentYPredecessor].angle;

            guidVector = this.FieldArray[currentXPredecessor, currentYPredecessor].amplitude;

            double predCellTraversingVelocity = (( guidVector * Math.Cos(a) ) +
                Math.Sqrt(
                    Math.Pow(guidVector * Math.Cos(a), 2) -
                    Math.Pow(guidVector, 2) +
                    this.preferredVelocity));

            predCellTraversingVelocity = (predCellTraversingVelocity > 1) ? 1 : predCellTraversingVelocity;
            double predCellTraversingCost = (aimCellDistance / 2) / predCellTraversingVelocity;

            a = navDirect;
            a -= this.FieldArray[xIsCalculated, yIsCalculated].angle;
            guidVector = this.FieldArray[xIsCalculated, yIsCalculated].amplitude;

            double cellTraversingVelocity = ((guidVector * Math.Cos(a)) +
                Math.Sqrt(
                    Math.Pow(guidVector * Math.Cos(a), 2) -
                    Math.Pow(guidVector, 2) +
                    this.preferredVelocity));

            cellTraversingVelocity = (cellTraversingVelocity > 1) ? 1 : cellTraversingVelocity;

            double cellTraversingCost = (aimCellDistance / 2) / cellTraversingVelocity;


            

            double resultCost = Math.Round((this.NavigFieldArray[currentXPredecessor, currentYPredecessor]
                    .pathCost + cellTraversingCost + predCellTraversingCost), 2);

            this.NavigFieldArray[xIsCalculated, yIsCalculated].xPredecessor = currentXPredecessor;
            this.NavigFieldArray[xIsCalculated, yIsCalculated].yPredecessor = currentYPredecessor;
            this.NavigFieldArray[xIsCalculated, yIsCalculated].angle = navDirect;
            this.NavigFieldArray[xIsCalculated, yIsCalculated].amplitude = cellTraversingVelocity;            
            this.NavigFieldArray[xIsCalculated, yIsCalculated].pathCost = resultCost;


            return resultCost;
        }

        private int ExtendedPredecessorFinder(int xIsCalculating, int yIsCalculating,
                                         int param3, int param4)
        {
            System.Threading.Thread.Sleep(this.sleepTime);

            double ak = this.NavigFieldArray[xIsCalculating, yIsCalculating].ak;

            double minCost = this.NavigFieldArray[xIsCalculating, yIsCalculating].pathCost;

            for (int rectSide = 0; rectSide < 4; rectSide++)
            {
                int ax = this.kxky[rectSide + 1];
                int ay = this.kxky[rectSide];
                int bx = this.kxky[rectSide + 2];
                int by = this.kxky[rectSide + 1];


                int Ax = xIsCalculating + ax;
                int Ay = yIsCalculating + ay;

                int Bx = xIsCalculating + bx;
                int By = yIsCalculating + by;

                if ((0 <= Ax) && (Ax < this.NavigFieldArray.GetLength(0)) &&
                    (0 <= Ay) && (Ay < this.NavigFieldArray.GetLength(1)) &&
                    (0 <= Bx) && (Bx < this.NavigFieldArray.GetLength(0)) &&
                    (0 <= By) && (By < this.NavigFieldArray.GetLength(1)))
                {
                    if (!this.NavigFieldArray[Ax, Ay].isObstacle &&
                        !this.NavigFieldArray[Bx, By].isObstacle)
                    {
                        double costA = this
                            .NavigFieldArray[Ax, Ay]
                            .pathCost;

                        double costB = this
                            .NavigFieldArray[Bx, By]
                            .pathCost;

                        int kx = 0;
                        int vx = 0;

                        int ky = 0;
                        int vy = 0;

                        switch (rectSide)
                        {
                            case 0:
                                {
                                    kx = 1;
                                    vx = -1;

                                    ky = -1;
                                    vy = 0;
                                }
                                break;
                            case 1:
                                {
                                    kx = 1;
                                    vx = 0;

                                    ky = 1;
                                    vy = -1;
                                }
                                break;
                            case 2:
                                {
                                    kx = -1;
                                    vx = 1;

                                    ky = 1;
                                    vy = 0;
                                }
                                break;
                            case 3:
                                {
                                    kx = -1;
                                    vx = 0;

                                    ky = -1;
                                    vy = 1;
                                }
                                break;
                        }

                        for (double a = 0; a < 1; a += 1 / ak)
                        {
                            double currentXPoint = xIsCalculating + a * kx + vx;
                            double currentYPoint = yIsCalculating + a * ky + vy;

                            double currentCost = 1;
                                
                            currentCost = ExtendedEvaluatingCell(
                                xIsCalculating, yIsCalculating,
                                currentXPoint,
                                currentYPoint,
                                Ax, Ay,
                                Bx, By,
                                a);
                            /**/
                            if (currentCost <= minCost)
                            {
                                this.NavigFieldArray[xIsCalculating, yIsCalculating]
                                    .extendedCalculated = true;

                                this.NavigFieldArray[xIsCalculating, yIsCalculating]
                                    .optimalExtendedXPredecessor = currentXPoint;
                                this.NavigFieldArray[xIsCalculating, yIsCalculating]
                                    .optimalExtendedYPredecessor = currentYPoint;

                                this.NavigFieldArray[xIsCalculating, yIsCalculating]
                                    .CostCalculatingData.Ax = Ax;
                                this.NavigFieldArray[xIsCalculating, yIsCalculating]
                                    .CostCalculatingData.Ay = Ay;

                                this.NavigFieldArray[xIsCalculating, yIsCalculating]
                                    .CostCalculatingData.Bx = Bx;
                                this.NavigFieldArray[xIsCalculating, yIsCalculating]
                                    .CostCalculatingData.By = By;

                                this.NavigFieldArray[xIsCalculating, yIsCalculating]
                                    .CostCalculatingData.a = a;

                                minCost = currentCost;
                            }

                        }

                    }
                }
            }

            if (this.NavigFieldArray[xIsCalculating, yIsCalculating]
                                    .extendedCalculated)
            {
                double xPoint = this.NavigFieldArray[xIsCalculating, yIsCalculating]
                                        .optimalExtendedXPredecessor;
                double yPoint = this.NavigFieldArray[xIsCalculating, yIsCalculating]
                                        .optimalExtendedYPredecessor;

                this.NavigFieldArray[xIsCalculating, yIsCalculating]
                    .optimalXPredecessor = (int)Math.Round(
                        xPoint);

                this.NavigFieldArray[xIsCalculating, yIsCalculating]
                    .optimalYPredecessor = (int)Math.Round(
                        yPoint);

                NavigationGridCell.CostCalculatingData_st CostCalculatingData
                    = this.NavigFieldArray[xIsCalculating, yIsCalculating]
                                        .CostCalculatingData;

                ExtendedEvaluatingCell(
                        xIsCalculating, yIsCalculating,
                        xPoint,
                        yPoint,
                        CostCalculatingData.Ax, CostCalculatingData.Ay,
                        CostCalculatingData.Bx, CostCalculatingData.By,
                        CostCalculatingData.a);
            }


            return 0;
        }


        private double ExtendedEvaluatingCell(int xIsCalculating, int yIsCalculating, 
                                               double currentXPoint, double currentYPoint,
                                               int Ax, int Ay,
                                               int Bx, int By,
                                               double coordParam)
        {
            double resultCost;
            double cellDistance = 1;
            double toPointDistance = Math.Sqrt(
                    Math.Pow(Math.Abs(xIsCalculating - currentXPoint), 2) +
                    Math.Pow(Math.Abs(yIsCalculating - currentYPoint), 2));

            double guidVector;
            double a = 0;

            if (currentXPoint - xIsCalculating != 0)
            {
                a = Math.Atan(
                (currentYPoint - yIsCalculating) /
                (currentXPoint - xIsCalculating));

                if (currentXPoint - xIsCalculating < 0)
                    a += Math.PI;
            }
            else
                a = Math.Asin((currentYPoint - yIsCalculating) / toPointDistance);

            if (currentYPoint - yIsCalculating == 0)
                a = Math.Acos((currentXPoint - xIsCalculating) / toPointDistance);
           
            double navDirect = a;

            if (0 <= navDirect && navDirect < Math.PI / 4
                 ||
                 Math.PI / 2 <= navDirect && navDirect < Math.PI * 3 / 4
                 ||
                 Math.PI <= navDirect && navDirect < Math.PI * 5 / 4
                 ||
                 -Math.PI / 2 <= navDirect && navDirect < -Math.PI / 4)
            {
                a -= this.FieldArray[Ax, Ay].angle;
                guidVector = this.FieldArray[Ax, Ay].amplitude;
            }
            else
            {
                a -= this.FieldArray[Bx, By].angle;
                guidVector = this.FieldArray[Bx, By].amplitude;
            }

            double predCellTraversingVelocity = ((guidVector * Math.Cos(a)) +
                Math.Sqrt(
                    Math.Pow(guidVector * Math.Cos(a), 2) -
                    Math.Pow(guidVector, 2) +
                    this.preferredVelocity));

            predCellTraversingVelocity = (predCellTraversingVelocity > 1) ? 1 : predCellTraversingVelocity;

            double isCalculatingCellDistance = 0;
            double predCellDistance = 0;

            if (-Math.PI / 2 <= navDirect && navDirect < 0)
            {
                isCalculatingCellDistance = (cellDistance / 2) /
                    Math.Cos(
                        Math.Abs(Math.Abs(-Math.PI / 4 + navDirect + Math.PI / 2) - Math.PI / 4));
                predCellDistance = toPointDistance - isCalculatingCellDistance;
            }
            if (0 <= navDirect && navDirect < Math.PI / 2)
            {
                isCalculatingCellDistance = (cellDistance / 2) /
                    Math.Cos(
                        Math.Abs(Math.Abs(-Math.PI / 4 + navDirect) - Math.PI / 4));
                predCellDistance = toPointDistance - isCalculatingCellDistance;
            }
            if (Math.PI / 2 <= navDirect && navDirect < Math.PI)
            {
                isCalculatingCellDistance = (cellDistance / 2) /
                    Math.Cos(
                        Math.Abs(Math.Abs(-Math.PI / 4 + navDirect - Math.PI / 2) - Math.PI / 4));
                predCellDistance = toPointDistance - isCalculatingCellDistance;
            }
            if (Math.PI <= navDirect && navDirect < 1.5 * Math.PI)
            {
                isCalculatingCellDistance = (cellDistance / 2) /
                    Math.Cos(
                        Math.Abs(Math.Abs(-Math.PI / 4 + navDirect - Math.PI) - Math.PI / 4));
                predCellDistance = toPointDistance - isCalculatingCellDistance;
            }

            double predCellTraversingCost = predCellDistance / predCellTraversingVelocity;

            a = navDirect;
            a -= this.FieldArray[xIsCalculating, yIsCalculating].angle;
            guidVector = this.FieldArray[xIsCalculating, yIsCalculating].amplitude;

            double cellTraversingVelocity = ((guidVector * Math.Cos(a)) +
                Math.Sqrt(
                    Math.Pow(guidVector * Math.Cos(a), 2) -
                    Math.Pow(guidVector, 2) +
                    this.preferredVelocity));

            cellTraversingVelocity = (cellTraversingVelocity > 1) ? 1 : cellTraversingVelocity;

            double cellTraversingCost = isCalculatingCellDistance / cellTraversingVelocity;

            resultCost = (1 - coordParam) * this.NavigFieldArray[Ax, Ay].pathCost
                + coordParam * this.NavigFieldArray[Bx, By].pathCost
                + (cellTraversingCost + predCellTraversingCost);

            this.NavigFieldArray[xIsCalculating, yIsCalculating].angle = navDirect;
            this.NavigFieldArray[xIsCalculating, yIsCalculating].amplitude = cellTraversingVelocity;
            this.NavigFieldArray[xIsCalculating, yIsCalculating]
                .extendedXPredecessor = currentXPoint;
            this.NavigFieldArray[xIsCalculating, yIsCalculating]
                .extendedYPredecessor = currentYPoint;

            this.NavigFieldArray[xIsCalculating, yIsCalculating].pathCost = resultCost;

            double t = Math.Round(resultCost, 2);


            return t;
        }

        
    }
}
