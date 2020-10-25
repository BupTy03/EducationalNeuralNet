using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        const int MaxInputsCount = 100; // максимальное количество входных сигналов
        const int MaxNeuronsCount = 10; // максимальное количество нейронов
        const int MaxSamplesCount = 10; // максимальное количество обучающих примеров
        const int MaxRowsCount = 15; // число строк матрицы образа буквы
        const int MaxColumnsCount = 10; // число столбцов матрицы образа буквы
        const int RowsCount = 10;
        const int ColumnsCount = 7;
        const double Epsilon = 0.001f; // точность для w1

        const int InputsCount = RowsCount * ColumnsCount; // количество входных сигналов по факту
        const int NeuronsCount = 3; // количество нейронов по факту
        const int SamplesCount = 2; // количество обучающих примеров по факту

        double[,] _inputsMtx = new double[MaxInputsCount, MaxSamplesCount]; // входящие сигналы
        double[,] _outputsMtx = new double[MaxNeuronsCount, MaxSamplesCount]; // выходящие сигналы
        double[,] _targetsMtx = new double[MaxNeuronsCount, MaxSamplesCount]; // обучающие примеры
        double[,] _weightsMtx = new double[MaxInputsCount, MaxNeuronsCount]; // матрица весов
        double[,] _offsetsMtx = new double[MaxInputsCount, MaxNeuronsCount]; // поправки к матрице весов
        double[,] _letterA = new double[MaxRowsCount, MaxColumnsCount]; // буква А
        double[,] _letterB = new double[MaxRowsCount, MaxColumnsCount]; // буква Б
        double[,] _newLetter = new double[MaxRowsCount, MaxColumnsCount]; // буква дефектная

        double[] xf = new double[MaxInputsCount]; // входящий сигнал
        double[] yf = new double[MaxNeuronsCount]; // выходящий сигнал

        public Form1()
        {
            InitializeComponent();
        }

        private void OnFillButtonClicked(object sender, EventArgs e)
        {
            _countInputsTextBox.Text = Convert.ToString(InputsCount);
            _countNeuronsTextBox.Text = Convert.ToString(NeuronsCount);
            _countSamplesTextBox.Text = Convert.ToString(SamplesCount);

            //      вектор обучения: А соотв.вектору (1;0;0); Б соотв.вектору (0;1;0)
            for (int sampleIndex = 1; sampleIndex <= SamplesCount; sampleIndex++)
            {
                for (int neuronIndex = 1; neuronIndex <= NeuronsCount; neuronIndex++)
                {
                    _targetsMtx[neuronIndex, sampleIndex] = 0;
                    if (neuronIndex == sampleIndex)
                    {
                        _targetsMtx[neuronIndex, sampleIndex] = 1;
                    }
                }
            }
            //  здесь задаем обучающие примеры: буквы А и Б            
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    this._letterAGridView.Rows[i].Cells[j].Value = Convert.ToString("");
                    this._letterBGridView.Rows[i].Cells[j].Value = Convert.ToString("");
                }
            }
            //  буква А    
            for (int i = 0; i <= 9; i++)
            {
                this._letterAGridView.Rows[i].Cells[5].Value = Convert.ToString(1);
                this._letterAGridView.Rows[i].Cells[6].Value = Convert.ToString(1);
            }
            for (int j = 1; j <= 6; j++)
            {
                this._letterAGridView.Rows[7].Cells[j].Value = Convert.ToString(1);
                this._letterAGridView.Rows[8].Cells[j].Value = Convert.ToString(1);
            }
            this._letterAGridView.Rows[1].Cells[4].Value = Convert.ToString(1);
            this._letterAGridView.Rows[2].Cells[4].Value = Convert.ToString(1);
            this._letterAGridView.Rows[3].Cells[3].Value = Convert.ToString(1);
            this._letterAGridView.Rows[3].Cells[4].Value = Convert.ToString(1);
            this._letterAGridView.Rows[4].Cells[3].Value = Convert.ToString(1);
            this._letterAGridView.Rows[4].Cells[4].Value = Convert.ToString(1);
            this._letterAGridView.Rows[5].Cells[2].Value = Convert.ToString(1);
            this._letterAGridView.Rows[5].Cells[3].Value = Convert.ToString(1);
            this._letterAGridView.Rows[6].Cells[2].Value = Convert.ToString(1);
            this._letterAGridView.Rows[6].Cells[3].Value = Convert.ToString(1);
            this._letterAGridView.Rows[9].Cells[0].Value = Convert.ToString(1);
            this._letterAGridView.Rows[9].Cells[1].Value = Convert.ToString(1);
            //  буква Б    
            for (int i = 0; i <= 9; i++)
            {
                this._letterBGridView.Rows[i].Cells[0].Value = Convert.ToString(1);
                this._letterBGridView.Rows[i].Cells[1].Value = Convert.ToString(1);
            }
            for (int j = 0; j <= 6; j++)
            {
                this._letterBGridView.Rows[0].Cells[j].Value = Convert.ToString(1);
                this._letterBGridView.Rows[1].Cells[j].Value = Convert.ToString(1);
                this._letterBGridView.Rows[5].Cells[j].Value = Convert.ToString(1);
                this._letterBGridView.Rows[8].Cells[j].Value = Convert.ToString(1);
            }
            for (int j = 0; j <= 5; j++)
            {
                this._letterBGridView.Rows[4].Cells[j].Value = Convert.ToString(1);
                this._letterBGridView.Rows[9].Cells[j].Value = Convert.ToString(1);
            }
            this._letterBGridView.Rows[6].Cells[5].Value = Convert.ToString(1);
            this._letterBGridView.Rows[6].Cells[6].Value = Convert.ToString(1);
            this._letterBGridView.Rows[7].Cells[5].Value = Convert.ToString(1);
            this._letterBGridView.Rows[7].Cells[6].Value = Convert.ToString(1);
            
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    string tt1 = Convert.ToString(this._letterAGridView.Rows[i].Cells[j].Value);
                    string tt2 = Convert.ToString(this._letterBGridView.Rows[i].Cells[j].Value);
                    _letterA[i + 1, j + 1] = 0;
                    if (tt1 == "1")
                    {
                        _letterA[i + 1, j + 1] = 1;
                    };
                    _letterB[i + 1, j + 1] = 0;
                    if (tt2 == "1")
                    {
                        _letterB[i + 1, j + 1] = 1;
                    };
                }
            }
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
            for (int row = 1, inputIndex = 0; row <= RowsCount; row++)
            {
                for (int col = 1; col <= ColumnsCount; col++)
                {
                    inputIndex++;
                    _inputsMtx[inputIndex, 1] = _letterA[row, col];
                    _inputsMtx[inputIndex, 2] = _letterB[row, col];
                }
            }

            // матрица весов
            double eta = 1.0;
            for (int inputIndex = 1; inputIndex <= InputsCount; inputIndex++)
            {
                for (int neuronIndex = 1; neuronIndex <= NeuronsCount; neuronIndex++)
                {
                    _weightsMtx[inputIndex, neuronIndex] = Math.Pow(-1.0, (inputIndex + neuronIndex));
                    _offsetsMtx[inputIndex, neuronIndex] = 0;
                }
            }

            int iterationsCount = 10;
            int Flag = 0;
            for (int iterationNum = 1; iterationNum <= iterationsCount; iterationNum++)
            {
                Flag = 0;
                // calc y
                for (int sampleIndex = 1; sampleIndex <= SamplesCount; sampleIndex++)
                {
                    for (int neuronIndex = 1; neuronIndex <= NeuronsCount; neuronIndex++)
                    {
                        double sum = 0.0;
                        for (int inputIndex = 1; inputIndex <= InputsCount; inputIndex++)
                        {
                            sum += _inputsMtx[inputIndex, sampleIndex] * _weightsMtx[inputIndex, neuronIndex];
                        }

                        _outputsMtx[neuronIndex, sampleIndex] = ActivationFunction.Sigmoid(sum);
                    }
                }

                // Image
                // optimization w
                for (int inputIndex = 1; inputIndex <= InputsCount; inputIndex++)
                {
                    for (int neuronIndex = 1; neuronIndex <= NeuronsCount; neuronIndex++)
                    {
                        double dEw = 0;
                        for (int sampleIndex = 1; sampleIndex <= SamplesCount; sampleIndex++)
                        { // Image
                            dEw += (_outputsMtx[neuronIndex, sampleIndex] - _targetsMtx[neuronIndex, sampleIndex]) * (1 - _outputsMtx[neuronIndex, sampleIndex]) * _outputsMtx[neuronIndex, sampleIndex] * _inputsMtx[inputIndex, sampleIndex];
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
                
                for (int inputIndex = 1; inputIndex <= InputsCount; inputIndex++)
                {
                    for (int neuronIndex = 1; neuronIndex <= NeuronsCount; neuronIndex++)
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

            // Распознование нечетко написанных букв
            double rr = 0.0;
            for (int row = 0; row < RowsCount; row++)
            {
                for (int col = 0; col < ColumnsCount; col++)
                {
                    _newLetter[row + 1, col + 1] = 0;
                    if (_chooseLetterComboBox.SelectedIndex == 0)
                    {
                        rr = _letterA[row + 1, col + 1];
                    }
                    if (_chooseLetterComboBox.SelectedIndex == 1)
                    {
                        rr = _letterB[row + 1, col + 1];
                    }
                    if (rr == 1)
                    {
                        _newLetterGridView.Rows[row].Cells[col].Value = Convert.ToString(rr);
                    }
                    else
                    {
                        _newLetterGridView.Rows[row].Cells[col].Value = Convert.ToString(" ");
                    }
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
                    _newLetter[row + 1, col + 1] = 0;
                    if (val == "1")
                    {
                        _newLetter[row + 1, col + 1] = 1;
                    }
                }
            }

        }

        private void OnRecognitionButtonClicked(object sender, EventArgs e)
        {
            _netOutputsTextBox.Text = "";
            _resultLetterTextBox.Text = "";
            string resultLetter = "неизвестная буква";

            for (int row = 1, inputIndex = 0; row <= RowsCount; row++)
            {
                for (int col = 1; col <= ColumnsCount; col++)
                {
                    inputIndex++;
                    xf[inputIndex] = _newLetter[row, col];
                }
            }

            string str = "";
            // calc y
            for (int neuronIndex = 1; neuronIndex <= NeuronsCount; neuronIndex++)
            {
                double sum = 0;
                for (int inputIndex = 0; inputIndex <= InputsCount; inputIndex++)
                {
                    sum = sum + xf[inputIndex] * _weightsMtx[inputIndex, neuronIndex];
                }
                yf[neuronIndex] = ActivationFunction.Sigmoid(sum);
                str += String.Format("{0:f4}  ", yf[neuronIndex]);
            }

            _netOutputsTextBox.Text = str;
            if ((yf[1] >= 0.8) && (yf[2] <= 0.2) && (yf[3] <= 0.2))
            {
                resultLetter = "А";
            }
            if ((yf[1] <= 0.2) && (yf[2] >= 0.8) && (yf[3] <= 0.2))
            {
                resultLetter = "Б";
            }

            _resultLetterTextBox.Text = resultLetter;
        }

    }
}