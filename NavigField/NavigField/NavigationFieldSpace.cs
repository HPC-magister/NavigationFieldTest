using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavigField
{
    public class NavigationFieldSpace : FieldSpace
    {
        public new NavigationGridCell[,] FieldArray { get; private set; }

        private delegate int Del(int xCurrent, int yCurrent, int xIsCalculated, int yIsCalculated, int countOfSubIterations);


        public NavigationFieldSpace(int xSize_tmp = 100, int ySize_tmp = 100)
        {
            xSize = xSize_tmp;
            ySize = ySize_tmp;

            FieldArray = new NavigationGridCell[xSize, ySize];

            for (int i = 0; i < xSize; i++)
                for (int j = 0; j < ySize; j++)
                    FieldArray[i, j] = new NavigationGridCell();
        }

        public int CalculateFieldForAim(int xAim = 0, int yAim = 0, double initCost = 0)
        {
            Del handler = CostMinFinder;

            this.FieldArray[xAim, yAim].pathCost = initCost;

            int xAimIndex = xAim, yAimIndex = yAim;
            this.FieldArray[xAimIndex, yAimIndex].iterationsPassed = 1;

            int maxDistToBorder = Math.Max(Math.Max((this.xSize - 1) - xAimIndex, xAimIndex),
                                      Math.Max((this.ySize - 1) - yAimIndex, yAimIndex));


            Indexer(handler, xAimIndex, yAimIndex, maxDistToBorder, 1);

            return 0;
        }

        public override int GetCountOfActiveCells()
        {
            int countOfActive = 0;

            foreach (NavigationGridCell e in FieldArray)
            {

                if (!e.isObstacle)
                    countOfActive++;
            }
            return countOfActive;
        }

        private int CostMinFinder(int xIsCalculated, int yIsCalculated, int param3, int param4, int countOfSubIterations)
        {
            Del costCreatorDel = CostCreator;


            Indexer(costCreatorDel, xIsCalculated, yIsCalculated, countOfSubIterations, 0);

            return 0;
        }

        private int CostCreator(int xCurrent, int yCurrent, int xIsCalculated, int yIsCalculated, int param5)
        {
            if (this.FieldArray[xIsCalculated, yIsCalculated].iterationsPassed > 0
                && this.FieldArray[xCurrent, yCurrent].iterationsPassed > 0)
            {
                if (this.FieldArray[xCurrent, yCurrent].pathCost
                < this.FieldArray[xIsCalculated, yIsCalculated].pathCost)
                {
                    this.FieldArray[xIsCalculated, yIsCalculated].pathCost
                        = this.FieldArray[xCurrent, yCurrent].pathCost + 1;

                    this.FieldArray[xIsCalculated, yIsCalculated].iterationsPassed++;
                    this.FieldArray[xIsCalculated, yIsCalculated].xPredecessor = xCurrent;
                    this.FieldArray[xIsCalculated, yIsCalculated].yPredecessor = yCurrent;
                }
            }
            else
                if (this.FieldArray[xCurrent, yCurrent].iterationsPassed > 0)
            {
                this.FieldArray[xIsCalculated, yIsCalculated].pathCost
                = this.FieldArray[xCurrent, yCurrent].pathCost + 1;

                this.FieldArray[xIsCalculated, yIsCalculated].iterationsPassed++;
                this.FieldArray[xIsCalculated, yIsCalculated].xPredecessor = xCurrent;
                this.FieldArray[xIsCalculated, yIsCalculated].yPredecessor = yCurrent;
            }





            return 0;
        }

        private int Indexer(Del handler, int xAimIndex, int yAimIndex, int countOfIterations, int countOfSubIterations)
        {

            for (int i = 1; i <= countOfIterations; i++)
            {
                int onSideCellsToDraw = 2 * i;

                int xBeginIndex = xAimIndex + i;
                int yBeginIndex = yAimIndex + i;

                int x = xBeginIndex, y = yBeginIndex;

                int[] kxky = new int[] { 0, -1, 0, 1, 0 };


                for (int rectSide = 0; rectSide < 4; rectSide++)
                {
                    int kx = kxky[rectSide + 1], ky = kxky[rectSide];

                    for (int dl = 0; dl < onSideCellsToDraw; dl++)
                    {
                        x += kx; y += ky;

                        if ((0 <= x) && (x < this.FieldArray.GetLength(0)) &&
                            (0 <= y) && (y < this.FieldArray.GetLength(1)))
                        {

                            if (!this.FieldArray[x, y].isObstacle)
                            {
                                handler(x, y, xAimIndex, yAimIndex, countOfSubIterations);
                            }
                        }
                    }
                }
            }




            return 0;
        }

        

    }
}
