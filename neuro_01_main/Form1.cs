using System;
using System.Windows.Forms;
using System.Diagnostics;


namespace neuro_01_main
{
    public partial class Form1 : Form
    {   
        private const double Epsilon = 0.001f;
        private const int NeuronsCount = 5;

        double[][,] _samples = new double[][,]
        {
            new double[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            },
            new double[,]
            {
                { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
            }
        };                
        string[] _sampleNames = new string[] { "Chevrolet", "Lexus" };
        private double[,] _newSample;

        private double[][] _targets;
        private double[,] _weightsMtx;


        private int RowsCount => _samples[0].GetLength(0);
        private int ColumnsCount => _samples[0].GetLength(1);
        private int InputsCount => RowsCount * ColumnsCount;
        private int SamplesCount => _samples.Length;
        private double[,] ChoosenLetter => _samples[_chooseLetterComboBox.SelectedIndex];


        public Form1()
        {
            InitializeComponent();
            _weightsMtx = new double[InputsCount, NeuronsCount];
            _newSample = new double[RowsCount, ColumnsCount];
            _targets = MathUtil.IdentityMatrix(SamplesCount, NeuronsCount);
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

            FillGridViewWithMatrix(_letterAGridView, _samples[0]);
            FillGridViewWithMatrix(_letterBGridView, _samples[1]);                                            
        }

        private void OnFormLoad(object sender, EventArgs e)
        {          
            const int columnWidth = 30;
            for (int col = 0; col < ColumnsCount; col++)
            {
                string cHeader = String.Format("c{0}", col);
                string hcHeader = String.Format("Hc{0}", col);

                _letterAGridView.Columns.Add(cHeader, hcHeader);
                _letterBGridView.Columns.Add(cHeader, hcHeader);
                _newLetterGridView.Columns.Add(cHeader, hcHeader);

                _letterAGridView.Columns[col].Width = columnWidth;
                _letterBGridView.Columns[col].Width = columnWidth;
                _newLetterGridView.Columns[col].Width = columnWidth;
            }

            _letterAGridView.Rows.Add(RowsCount - 1);
            _letterBGridView.Rows.Add(RowsCount - 1);
            _newLetterGridView.Rows.Add(RowsCount - 1);

            _chooseLetterComboBox.Items.AddRange(_sampleNames);
            _chooseLetterComboBox.SelectedIndex = 0;

            _countNeuronsTextBox.Text = Convert.ToString(NeuronsCount);
            _countInputsTextBox.Text = Convert.ToString(InputsCount);
            _countSamplesTextBox.Text = Convert.ToString(SamplesCount);
        }
    
        private void OnLearnButtonClicked(object sender, EventArgs e)
        {
            // задание входных сигналов
            double[,] inputsMtx = new double[InputsCount, SamplesCount];
            for (int row = 0, inputIndex = 0; row < RowsCount; row++)
            {
                for (int col = 0; col < ColumnsCount; col++)
                {                         
                    for(int sampleIndex = 0; sampleIndex < _samples.Length; ++sampleIndex)
                    {
                        inputsMtx[inputIndex, sampleIndex] = _samples[sampleIndex][row, col];         
                    }
                    inputIndex++;
                }
            }

            // матрица весов            
            _weightsMtx = MathUtil.InitialWeights(InputsCount, NeuronsCount);

            // поправки к матрице весов
            double[,] offsetsMtx = new double[InputsCount, NeuronsCount];
            
            int iterationsCount = 10;
            int Flag = 0;
            for (int iterationNum = 0; iterationNum < iterationsCount; iterationNum++)
            {
                Flag = 0;

                // calc outputs
                double[,] outputsMtx = MathUtil.Multiply(MathUtil.Transform(_weightsMtx), inputsMtx);
                outputsMtx = MathUtil.Apply(outputsMtx, MathUtil.ActivationFunction.Sigmoid);

                // Image
                // optimization w
                const double eta = 1.0;
                for (int inputIndex = 0; inputIndex < InputsCount; inputIndex++)
                {
                    for (int neuronIndex = 0; neuronIndex < NeuronsCount; neuronIndex++)
                    {
                        double dEw = 0;
                        for (int sampleIndex = 0; sampleIndex < SamplesCount; sampleIndex++)
                        {
                            dEw += (outputsMtx[neuronIndex, sampleIndex] - _targets[sampleIndex][neuronIndex]) * (1 - outputsMtx[neuronIndex, sampleIndex]) * outputsMtx[neuronIndex, sampleIndex] * inputsMtx[inputIndex, sampleIndex];
                        }
                        double tol = Epsilon * Math.Abs(dEw) + Epsilon;
                        double error = Math.Abs(-dEw - offsetsMtx[inputIndex, neuronIndex] / eta);
                        if (error > tol)
                        {
                            Flag = 1;
                        }
                        offsetsMtx[inputIndex, neuronIndex] = -eta * dEw;
                    }
                }

                _weightsMtx = MathUtil.Plus(_weightsMtx, offsetsMtx);

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
                    _newSample[row, col] = (val == "1") ? 1 : 0;
                }
            }

        }

        private void OnRecognitionButtonClicked(object sender, EventArgs e)
        {
            _netOutputsTextBox.Text = "";
            _resultLetterTextBox.Text = "";

            double[,] xf = MathUtil.Vectorize(_newSample);
            double[,] yf = MathUtil.Apply(MathUtil.Multiply(MathUtil.Transform(_weightsMtx), xf), MathUtil.ActivationFunction.Sigmoid);

            double[] outputs = MathUtil.VectorToArray(yf);
            _netOutputsTextBox.Text = MathUtil.Stringify(outputs);

            const double accuracy = 0.5;
            _resultLetterTextBox.Text = "не распознан";
            for (int sampleIndex = 0; sampleIndex < _samples.Length; ++sampleIndex)
            {
                if (MathUtil.IsAbout(outputs, _targets[sampleIndex], accuracy))
                {
                    _resultLetterTextBox.Text = _sampleNames[sampleIndex];
                    break;
                }
            }                
        }
    }
}