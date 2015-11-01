using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavigField
{
    class GuidanseFieldSpace
    {
        private Field[,] GuidanseFieldArray;

        private int xSize;
        private int ySize;

        public GuidanseFieldSpace(int xSize_tmp, int ySize_tmp)
        {
            GuidanseFieldArray = new Field[xSize_tmp, ySize_tmp];            
           
            xSize = xSize_tmp;
            ySize = ySize_tmp;

            for (int i = 0; i < xSize; i++)
                for (int j = 0; j < ySize; j++)
                    GuidanseFieldArray[i, j] = new Field(false, 0, 0, 0, 0);

        }

        public int SetField(bool isActive, int x, int y, double angle, double cost) {
            if (x < xSize && y < ySize && x > -1 && y> -1 && GuidanseFieldArray.Length > 0 )
            {
                GuidanseFieldArray[x,y].Update(isActive, x, y, angle, cost);
                return 0;
            }
            else
                return -1;
        }

        public Field GetField(int x, int y)
        {
            return GuidanseFieldArray[x,y];
        }

        public Field[,] GetArayOfFields()
        {
            return GuidanseFieldArray;
        }

        public int GetCountOfFields()
        {
            
            return xSize * ySize;
        }

        public int GetCountOfActiveFields()
        {
            int countOfActive = 0;

            foreach(Field e in GuidanseFieldArray)
            {
                
                if (e.IsActive())
                    countOfActive++;
            }
            return countOfActive;
        }
    }
}