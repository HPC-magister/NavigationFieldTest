﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavigField
{
    public class NavigationFieldSpace : FieldSpace
    {
        public new NavigationGridCell[,] FieldArray { get; private set; }

        private delegate int Del(int xCurrent, int yCurrent, int xIsCalculated, int yIsCalculated);


        public NavigationFieldSpace(int xSize_tmp = 100, int ySize_tmp = 100)
        {
            xSize = xSize_tmp;
            ySize = ySize_tmp;

            Clear();        
        }

        public int Clear()
        {            
            FieldArray = new NavigationGridCell[xSize, ySize];

            for (int i = 0; i < xSize; i++)
                for (int j = 0; j < ySize; j++)
                    FieldArray[i, j] = new NavigationGridCell();

            return 0;
        }

        public int CalculateFieldForAim(int xAimIndex, int yAimIndex, int calcIterations = 1, double initCost = 0)
        {
            Del handler = CostMinFinder;

            this.Clear();

            this.FieldArray[xAimIndex, yAimIndex].pathCost = initCost;

            this.FieldArray[xAimIndex, yAimIndex].calcIterationsPassed = 1;

            int maxDistToBorder = Math.Max(Math.Max((this.xSize - 1) - xAimIndex, xAimIndex),
                                      Math.Max((this.ySize - 1) - yAimIndex, yAimIndex));


            Indexer(handler, xAimIndex, yAimIndex, maxDistToBorder, calcIterations);

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

        private int CostMinFinder(int xIsCalculated, int yIsCalculated, int param3, int param4)
        {
            Del costCreatorDel = CostCreator;


            Indexer(costCreatorDel, xIsCalculated, yIsCalculated);
            this.FieldArray[xIsCalculated, yIsCalculated].calcIterationsPassed++;

            return 0;
        }

        private int CostCreator(int xCurrent, int yCurrent, int xIsCalculated, int yIsCalculated)
        {
            if (this.FieldArray[xCurrent, yCurrent].calcIterationsPassed > 0 
                &&
                this.FieldArray[xIsCalculated, yIsCalculated].calcIterationsPassed > 0 
                &&
                this.FieldArray[xCurrent, yCurrent].pathCost
                < this.FieldArray[xIsCalculated, yIsCalculated].pathCost 
                ||
                this.FieldArray[xCurrent, yCurrent].calcIterationsPassed > 0 
                &&
                !(this.FieldArray[xIsCalculated, yIsCalculated].calcIterationsPassed > 0))
                    {
                        this.FieldArray[xIsCalculated, yIsCalculated].pathCost
                            = this.FieldArray[xCurrent, yCurrent].pathCost + 3;
                        
                        this.FieldArray[xIsCalculated, yIsCalculated].xPredecessor = xCurrent;
                        this.FieldArray[xIsCalculated, yIsCalculated].yPredecessor = yCurrent;

                        System.Threading.Thread.Sleep(10);
                    }
                





            return 0;
        }

        private int Indexer(Del handler, int xAimIndex, int yAimIndex, int calculatingRadius = 1, int calcIterations = 1)
        {
                        
            for (int iteration = 1; iteration <= calcIterations; iteration++)
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

                            if ((0 <= x) && (x < this.FieldArray.GetLength(0)) &&
                                (0 <= y) && (y < this.FieldArray.GetLength(1)))
                            {

                                if (!this.FieldArray[x, y].isObstacle)
                                {
                                    handler(x, y, xAimIndex, yAimIndex);
                                }
                            }
                        }
                    }
                }




            return 0;
        }

        

    }
}
