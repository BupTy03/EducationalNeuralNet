using System;
using System.Collections.Generic;
using System.Text;

namespace neuro_01_main
{
    struct MatrixSize
    {
        public MatrixSize(int rows, int columns)
        {
            Rows = rows;
            Colums = columns;
        }

        public int Rows { get; set; }
        public int Colums { get; set; }
    }
}
