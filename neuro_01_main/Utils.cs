using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace neuro_01_main
{
    class Utils
    {
        public static char[] SpacesChars = new char[] { ' ', '\t', '\f', '\n', '\r', '\v' };

        public static double[] ReadMatrixRowDouble(string line)
        {
            var valuesStr = line.Split(SpacesChars, StringSplitOptions.RemoveEmptyEntries);
            var row = new double[valuesStr.Length];
            for (int i = 0; i < row.Length; ++i)
                row[i] = Double.Parse(valuesStr[i]);

            return row;
        }

        public static int[] ReadMatrixRowInt(string line)
        {
            var valuesStr = line.Split(SpacesChars, StringSplitOptions.RemoveEmptyEntries);
            var row = new int[valuesStr.Length];
            for (int i = 0; i < row.Length; ++i)
                row[i] = Int32.Parse(valuesStr[i]);

            return row;
        }

        public static MatrixSize ReadMatrixSize(string line)
        {
            var arr = ReadMatrixRowInt(line);
            if (arr.Length != 2)
                throw new FormatException($"Expected two integers for matrix size, {arr.Length} given");

            return new MatrixSize(arr[0], arr[1]);
        }

        public static string ReadSampleTitle(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                line = line.Trim(SpacesChars);
                if (line.Length == 0)
                    continue;

                return line;
            }

            throw new FormatException("Sample title expected");
        }

        public static double[,] ReadSampleMatrix(StreamReader reader, MatrixSize mtxSize)
        {
            var mtx = new double[mtxSize.Rows, mtxSize.Colums];
            int rowIndex = 0;
            while (rowIndex < mtxSize.Rows && !reader.EndOfStream)
            {
                string line = reader.ReadLine();
                line = line.Trim(SpacesChars);
                if (line.Length == 0)
                    continue;

                double[] currRow = ReadMatrixRowDouble(line);
                if (currRow.Length != mtxSize.Colums)
                    throw new FormatException($"Expected matrix columns count: {mtxSize.Colums}, given: {currRow.Length}");

                for (int col = 0; col < currRow.Length; ++col)
                    mtx[rowIndex, col] = currRow[col];

                rowIndex++;
            }

            if (rowIndex != mtxSize.Rows)
                throw new FormatException($"Expected matrix rows count: {mtxSize.Rows}, given: {rowIndex}");

            return mtx;
        }

        public static List<Sample> LoadSamples(string filename)
        {
            var samples = new List<Sample>();
            using (var reader = new StreamReader(filename))
            {
                MatrixSize matrixSize = ReadMatrixSize(reader.ReadLine());
                while (!reader.EndOfStream)
                {
                    samples.Add(new Sample(ReadSampleTitle(reader), ReadSampleMatrix(reader, matrixSize)));
                }
            }

            if (samples.Count == 0)
                throw new FormatException("At least one sample needed");

            return samples;
        }
    }
}
