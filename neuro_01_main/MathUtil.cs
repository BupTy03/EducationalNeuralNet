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

        public static double[,] Transform(double[,] mtx)
        {
            int rowsCount = mtx.GetLength(0);
            int colsCount = mtx.GetLength(1);

            double[,] result = new double[colsCount, rowsCount];
            for (int row = 0; row < rowsCount; ++row)
            {
                for (int col = 0; col < colsCount; ++col)
                {
                    result[col, row] = mtx[row, col];
                }
            }

            return result;
        }

        public static double[,] Multiply(double[,] a, double[,] b)
        {
            Debug.Assert(a.GetLength(1) == b.GetLength(0));

            int countRows = a.GetLength(0);
            int countColumns = b.GetLength(1);
            int commonDim = a.GetLength(1);

            double[,] result = new double[countRows, countColumns];
            for (int row = 0; row < countRows; row++)
            {
                for (int col = 0; col < countColumns; col++)
                {
                    double sum = 0.0;
                    for (int k = 0; k < commonDim; ++k)
                    {
                        sum += a[row, k] * b[k, col];
                    }
                    result[row, col] = sum;
                }
            }

            return result;
        }

        public static void Print(double[,] mtx)
        {
            int countRows = mtx.GetLength(0);
            int countColumns = mtx.GetLength(1);
            for (int row = 0; row < countRows; ++row)
            {
                Console.Write('[');
                for (int col = 0; col < countColumns; ++col)
                {
                    Console.Write(" {0}", mtx[row, col]);
                }
                Console.WriteLine(" ]");
            }

            Console.WriteLine();
        }

        public class ActivationFunction
        {
            public static double Sigmoid(double x)
            {
                return 1.0 / (1.0 + Math.Exp(-x));
            }
        }

        public static double[,] Apply(double[,] mtx, Func<double, double> func)
        {
            int rowsCount = mtx.GetLength(0);
            int columnsCount = mtx.GetLength(1);

            double[,] result = new double[rowsCount, columnsCount];
            for (int row = 0; row < rowsCount; ++row)
            {
                for (int col = 0; col < columnsCount; ++col)
                {
                    result[row, col] = func(mtx[row, col]);
                }
            }

            return result;
        }

        public static string Stringify(double[] arr)
        {
            string result = "";
            for (int i = 0; i < arr.Length; i++)
            {
                result += String.Format("{0:f4}  ", arr[i]);
            }

            return result;
        }

        public static double[] VectorToArray(double[,] mtx)
        {
            int rowsCount = mtx.GetLength(0);
            int columnsCount = mtx.GetLength(1);

            Debug.Assert(columnsCount == 1);

            double[] result = new double[rowsCount];
            for (int row = 0; row < rowsCount; ++row)
            {
                result[row] = mtx[row, 0];
            }

            return result;
        }

        public static double[,] Vectorize(double[,] mtx)
        {
            int rowsCount = mtx.GetLength(0);
            int columnsCount = mtx.GetLength(1);

            double[,] vec = new double[rowsCount * columnsCount, 1];
            int index = 0;
            for(int row = 0; row < rowsCount; ++row)
            {
                for(int col = 0; col < columnsCount; ++col)
                {
                    vec[index, 0] = mtx[row, col];
                    index++;
                }
            }

            return vec;
        }

        public static double[][] IdentityMatrix(int rowsCount, int columnsCount)
        {
            double[][] result = new double[rowsCount][];
            for(int row = 0; row < rowsCount; ++row)
            {
                result[row] = new double[columnsCount];
                for(int col = 0; col < columnsCount; ++col)
                {
                    result[row][col] = (row == col) ? 1 : 0;
                }
            }

            return result;
        }
    }
}
