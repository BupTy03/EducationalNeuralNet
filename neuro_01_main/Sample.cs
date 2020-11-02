using System;
using System.Collections.Generic;
using System.Text;

namespace neuro_01_main
{
    class Sample
    {
        private string _name;
        private double[,] _matrix;

        public string Name => _name;
        public double[,] InputsMtx => _matrix;

        public Sample(string name, double[,] matrix)
        {
            _name = name;
            _matrix = matrix;
        }
    }
}
