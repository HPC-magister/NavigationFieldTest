using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavigField
{
    public class NavigationFieldSpace : FieldSpace
    {
        public new NavigationGridCell[,] FieldArray { get; protected set; }

        public NavigationFieldSpace(int xSize_tmp = 100, int ySize_tmp = 100)
        {
            xSize = xSize_tmp;
            ySize = ySize_tmp;

            FieldArray = new NavigationGridCell[xSize, ySize];

            for (int i = 0; i < xSize; i++)
                for (int j = 0; j < ySize; j++)
                    FieldArray[i, j] = new NavigationGridCell();
        }

        public override int UpdateCell(bool isObstacle, int x, int y, double angl, double t) {
            return 0;
        }
        /*
        public int Update()
        {
            
            int iteration;
            int sideLength = 2 * iteration;
            
            int xBegin = 1 + xk, yBegin = -1 + yk;
            int x = xBegin, y = yBegin;

            int[] dxdy = new int[] { 0, -1, 0, 1, 0 };

            for (int side = 1; side < 5; side++)
            {
                int dx = dxdy[side + 1], dy = dxdy[side];       

                for (int dl = 0; dl < sideLength; dl++)
                {
                    FieldArray[x, y];
                    x += dx; y += dy;
                }
            }

            return 0;
        }
        */
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
    }
}
