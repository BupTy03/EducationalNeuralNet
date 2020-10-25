using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace neuro_01_main
{
    class MathUtil
    {
        public static double[,] InitialWeights(int countRows, int countColumns)
        {
            double[,] result = new double[countRows, countColumns];
            for (int i = 0; i < countRows; i++)
            {
                for (int j = 0; j < countColumns; j++)
                {
                    result[i, j] = Math.Pow(-1.0, (i + j));
                }
            }

            return result;
        }

        public static double[,] Plus(double[,] a, double[,] b)
        {
            int countRows = a.GetLength(0);
            int countColumns = a.GetLength(1);

            Debug.Assert(countRows == b.GetLength(0));
            Debug.Assert(countColumns == b.GetLength(1));

            double[,] result = new double[countRows, countColumns];
            for (int row = 0; row < countRows; ++row)
            {
                for (int col = 0; col < countColumns; ++col)
                {
                    result[row, col] = a[row, col] + b[row, col];
                }
            }

            return result;
        }

        public static bool IsAbout(double[] a, double[] b, double accuracy)
        {
            Debug.Assert(a.Length == b.Length);
            for(int i = 0; i < a.Length; ++i)
            {
                if (Math.Abs(a[i] - b[i]) >= accuracy)
                    return false;
            }

            return true;
        }
    }
}
