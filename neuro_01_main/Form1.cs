﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace neuro_01_main
{
    public partial class Form1 : Form
    {
        public class ActivationFunction
        {
            public static double Sigmoid(double x)
            {
                return 1.0 / (1.0 + Math.Exp(-x));
            }
        }

        // однослойная нейронная сеть
        const int RowsCount = 10;
        const int ColumnsCount = 7;
        const double Epsilon = 0.001f; // точность для w1

        const int InputsCount = RowsCount * ColumnsCount; // количество входных сигналов по факту
        const int NeuronsCount = 3; // количество нейронов по факту
        const int SamplesCount = 2; // количество обучающих примеров по факту

        double[,] _inputsMtx = new double[InputsCount, SamplesCount]; // входящие сигналы
        double[,] _outputsMtx = new double[NeuronsCount, SamplesCount]; // выходящие сигналы
        double[,] _weightsMtx = new double[InputsCount, NeuronsCount]; // матрица весов
        double[,] _offsetsMtx = new double[InputsCount, NeuronsCount]; // поправки к матрице весов

        // буква А
        double[,] _letterA = new double[,]
        {
            { 0, 0, 0, 0, 0, 1, 1 },
            { 0, 0, 0, 0, 1, 1, 1 },
            { 0, 0, 0, 0, 1, 1, 1 },
            { 0, 0, 0, 1, 1, 1, 1 },
            { 0, 0, 0, 1, 1, 1, 1 },
            { 0, 0, 1, 1, 0, 1, 1 },
            { 0, 0, 1, 1, 0, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 0, 0, 0, 1, 1 }
        };

        // буква Б
        double[,] _letterB = new double[,]
        {
            { 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 0, 0, 0, 0, 0 },
            { 1, 1, 0, 0, 0, 0, 0 },
            { 1, 1, 1, 1, 1, 1, 0 },
            { 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 0, 0, 0, 1, 1 },
            { 1, 1, 0, 0, 0, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 0 }
        };

        double[,] ChoosenLetter => (_chooseLetterComboBox.SelectedIndex == 0) ? _letterA : _letterB;

        double[,] _targetsMtx = new double[,]
        {
            { 1, 0, 0 },
            { 0, 1, 0 }
        };

        // буква дефектная
        double[,] _newLetter = new double[RowsCount, ColumnsCount];

        public Form1()
        {
            InitializeComponent();
        }

        private void FillGridViewWithMatrix(DataGridView gridView, double[,] mtx)
        {
            int rowsCount = mtx.GetLength(0);
            int columnsCount = mtx.GetLength(1);

            Debug.Assert(gridView.Rows.Count == rowsCount);
            Debug.Assert(gridView.Columns.Count == columnsCount);

            for(int row = 0; row < rowsCount; ++row)
            {
                for(int col = 0; col < columnsCount; ++col)
                {
                    gridView.Rows[row].Cells[col].Value = (mtx[row, col] > 0) ? "1" : "";
                }
            }
        }

        private void OnFillButtonClicked(object sender, EventArgs e)
        {
            _countInputsTextBox.Text = Convert.ToString(InputsCount);
            _countNeuronsTextBox.Text = Convert.ToString(NeuronsCount);
            _countSamplesTextBox.Text = Convert.ToString(SamplesCount);

            FillGridViewWithMatrix(_letterAGridView, _letterA);
            FillGridViewWithMatrix(_letterBGridView, _letterB);                                               
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            //  параметры таблицы для обучающих примеров
            _chooseLetterComboBox.SelectedIndex = 0;

            for (int i = 0; i < ColumnsCount; i++)
            {
                string cHeader = String.Format("c{0}", i);
                string hcHeader = String.Format("Hc{0}", i);

                _letterAGridView.Columns.Add(cHeader, hcHeader);
                _letterBGridView.Columns.Add(cHeader, hcHeader);
                _newLetterGridView.Columns.Add(cHeader, hcHeader);

                _letterAGridView.Columns[i].Width = 30;
                _letterBGridView.Columns[i].Width = 30;
                _newLetterGridView.Columns[i].Width = 30;
            }

            _letterAGridView.Rows.Add(RowsCount - 1);
            _letterBGridView.Rows.Add(RowsCount - 1);
            _newLetterGridView.Rows.Add(RowsCount - 1);
        }

        private void OnLearnButtonClicked(object sender, EventArgs e)
        {
            // обучение 
            // задание входных сигналов
            for (int row = 0, inputIndex = 0; row < RowsCount; row++)
            {
                for (int col = 0; col < ColumnsCount; col++)
                {                                       
                    _inputsMtx[inputIndex, 0] = _letterA[row, col];
                    _inputsMtx[inputIndex, 1] = _letterB[row, col];
                    inputIndex++;
                }
            }

            // матрица весов
            double eta = 1.0;
            for (int inputIndex = 0; inputIndex < InputsCount; inputIndex++)
            {
                for (int neuronIndex = 0; neuronIndex < NeuronsCount; neuronIndex++)
                {
                    _weightsMtx[inputIndex, neuronIndex] = Math.Pow(-1.0, (inputIndex + neuronIndex));
                    _offsetsMtx[inputIndex, neuronIndex] = 0;
                }
            }

            int iterationsCount = 10;
            int Flag = 0;
            for (int iterationNum = 0; iterationNum < iterationsCount; iterationNum++)
            {
                Flag = 0;
                // calc y
                for (int sampleIndex = 0; sampleIndex < SamplesCount; sampleIndex++)
                {
                    for (int neuronIndex = 0; neuronIndex < NeuronsCount; neuronIndex++)
                    {
                        double sum = 0.0;
                        for (int inputIndex = 0; inputIndex < InputsCount; inputIndex++)
                        {
                            sum += _inputsMtx[inputIndex, sampleIndex] * _weightsMtx[inputIndex, neuronIndex];
                        }

                        _outputsMtx[neuronIndex, sampleIndex] = ActivationFunction.Sigmoid(sum);
                    }
                }

                // Image
                // optimization w
                for (int inputIndex = 0; inputIndex < InputsCount; inputIndex++)
                {
                    for (int neuronIndex = 0; neuronIndex < NeuronsCount; neuronIndex++)
                    {
                        double dEw = 0;
                        for (int sampleIndex = 0; sampleIndex < SamplesCount; sampleIndex++)
                        { // Image
                            dEw += (_outputsMtx[neuronIndex, sampleIndex] - _targetsMtx[sampleIndex, neuronIndex]) * (1 - _outputsMtx[neuronIndex, sampleIndex]) * _outputsMtx[neuronIndex, sampleIndex] * _inputsMtx[inputIndex, sampleIndex];
                        } // Image
                        double tol = Epsilon * Math.Abs(dEw) + Epsilon;
                        double error = Math.Abs(-dEw - _offsetsMtx[inputIndex, neuronIndex] / eta);
                        if (error > tol)
                        {
                            Flag = 1;
                        }
                        _offsetsMtx[inputIndex, neuronIndex] = -eta * dEw;                       
                    }
                }
                
                for (int inputIndex = 0; inputIndex < InputsCount; inputIndex++)
                {
                    for (int neuronIndex = 0; neuronIndex < NeuronsCount; neuronIndex++)
                    {
                        _weightsMtx[inputIndex, neuronIndex] = _weightsMtx[inputIndex, neuronIndex] + _offsetsMtx[inputIndex, neuronIndex];
                    }
                }

                if (Flag == 0)
                {
                    iterationsCount = iterationNum;
                    break;
                }
            }

            MessageBox.Show(String.Format(" Education OK; Flag={0}", Flag));

        }

        private void OnGenerateButtonClicked(object sender, EventArgs e)
        {
            _netOutputsTextBox.Text = "";
            _resultLetterTextBox.Text = "";

            for (int row = 0; row < RowsCount; row++)
            {
                for (int col = 0; col < ColumnsCount; col++)
                {
                    _newLetterGridView.Rows[row].Cells[col].Value = ChoosenLetter[row, col] == 1 ? "1" : "";                    
                }
            }
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            _netOutputsTextBox.Text = "";
            _resultLetterTextBox.Text = "";
            for (int row = 0; row < RowsCount; row++)
            {
                for (int col = 0; col < ColumnsCount; col++)
                {
                    string val = Convert.ToString(_newLetterGridView.Rows[row].Cells[col].Value);
                    _newLetter[row, col] = (val == "1") ? 1 : 0;
                }
            }

        }

        private void OnRecognitionButtonClicked(object sender, EventArgs e)
        {
            _netOutputsTextBox.Text = "";
            _resultLetterTextBox.Text = "";

            // входящий сигнал
            double[] xf = new double[InputsCount];
            for (int row = 0, inputIndex = 0; row < RowsCount; row++)
            {
                for (int col = 0; col < ColumnsCount; col++)
                {                    
                    xf[inputIndex] = _newLetter[row, col];
                    inputIndex++;
                }
            }

            string str = "";

            // y - выходящий сигнал
            double[] yf = new double[NeuronsCount];
            for (int neuronIndex = 0; neuronIndex < NeuronsCount; neuronIndex++)
            {
                double sum = 0;
                for (int inputIndex = 0; inputIndex < InputsCount; inputIndex++)
                {
                    sum += xf[inputIndex] * _weightsMtx[inputIndex, neuronIndex];
                }
                yf[neuronIndex] = ActivationFunction.Sigmoid(sum);
                str += String.Format("{0:f4}  ", yf[neuronIndex]);
            }

            _netOutputsTextBox.Text = str;
            string resultLetter = "неизвестная буква";
            if ((yf[0] >= 0.5) && (yf[1] < 0.5) && (yf[2] < 0.5))
            {
                resultLetter = "А";
            }
            else if ((yf[0] < 0.5) && (yf[1] >= 0.5) && (yf[2] < 0.5))
            {
                resultLetter = "Б";
            }

            _resultLetterTextBox.Text = resultLetter;
        }

    }
}