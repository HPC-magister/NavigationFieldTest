using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavigField
{
    public class NavigationFieldSpace : FieldSpace
    {
        public new NavigationGridCell[,] FieldArray { get; set; }

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
        
        public int CalculateField(int xAim = 0, int yAim = 0)
        {

            int dist = 50;

            for (int iteration = 1; iteration <= 3; iteration++)
            {
                int pointsToDraw = 2 * iteration;

                int xBegin = iteration * dist + xAim, yBegin = iteration * dist + yAim;
                int x = xBegin, y = yBegin;

                int[] kxky = new int[] { 0, -1, 0, 1, 0 };


                for (int side = 0; side < 4; side++)
                {
                    int kx = kxky[side + 1], ky = kxky[side];

                    for (int dl = 0; dl < pointsToDraw; dl++)
                    {
                        x = x + kx * dist; y = y + ky * dist;

                        
                    }
                }
            }

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
    }
}
