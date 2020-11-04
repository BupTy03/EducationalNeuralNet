using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace neuro_01_main
{
    public partial class Form1 : Form
    {   
        private const double Epsilon = 0.001f;
        private const int DefaultNeuronsCount = 5;

        List<Sample> _samples = null;

        private double[,] _newSample = null;

        private double[][] _targets = null;
        private double[,] _weightsMtx = null;

        OpenFileDialog _fileDialog = new OpenFileDialog();

        private int NeuronsCount
        {
            get
            {
                try
                {
                    return Int32.Parse(_countNeuronsTextBox.Text);
                }
                catch(Exception)
                {
                    _countNeuronsTextBox.Text = Convert.ToString(DefaultNeuronsCount);
                    MessageBox.Show(this, "Invalid neurons count", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return DefaultNeuronsCount;
                }
            }
        }

        private int RowsCount => _samples[0].InputsMtx.GetLength(0);
        private int ColumnsCount => _samples[0].InputsMtx.GetLength(1);
        private int InputsCount => RowsCount * ColumnsCount;
        private int SamplesCount => _samples.Count;
        private Sample ChoosenSample => _samples[_chooseLetterComboBox.SelectedIndex];


        public Form1()
        {
            InitializeComponent();
            _fileDialog.Filter = "Text files(*.txt)|*.txt";
        }

        private void FillGridViewWithMatrix(DataGridView gridView, double[,] mtx)
        {
            const int columnWidth = 30;
            int rowsCount = mtx.GetLength(0);
            int columnsCount = mtx.GetLength(1);

            gridView.Rows.Clear();
            gridView.Columns.Clear();
            for (int col = 0; col < ColumnsCount; col++)
            {
                gridView.Columns.Add($"c{col}", $"Hc{col}");
                gridView.Columns[col].Width = columnWidth;
            }
            gridView.Rows.Add(RowsCount - 1);

            Debug.Assert(gridView.Rows.Count == rowsCount);
            Debug.Assert(gridView.Columns.Count == columnsCount);

            for (int row = 0; row < rowsCount; ++row)
            {
                for(int col = 0; col < columnsCount; ++col)
                {
                    gridView.Rows[row].Cells[col].Value = (mtx[row, col] > 0) ? "1" : "";
                }
            }
        }

        private void OnFillButtonClicked(object sender, EventArgs e)
        {
            _fileDialog.InitialDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            _fileDialog.FileName = "text.txt";
            if (_fileDialog.ShowDialog(this) == DialogResult.Cancel)
                return;
            
            try
            {
                _samples = Utils.LoadSamples(_fileDialog.FileName);
            }            
            catch(Exception exc)
            {
                MessageBox.Show(this, exc.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _samplesListTable.Controls.Clear();
            _samplesListTable.RowCount = _samples.Count;
            for (int sampleIndex = 0; sampleIndex < _samples.Count; ++sampleIndex)
            {
                DataGridView grid = new DataGridView();                         
                FillGridViewWithMatrix(grid, _samples[sampleIndex].InputsMtx);                
                grid.Dock = DockStyle.Fill;

                _samplesListTable.Controls.Add(grid, 1, sampleIndex);                  
                if(sampleIndex < _samplesListTable.RowStyles.Count)
                {
                    _samplesListTable.RowStyles[sampleIndex].SizeType = SizeType.Percent;
                    _samplesListTable.RowStyles[sampleIndex].Height = 100f / _samples.Count;
                }                              
                else
                {
                    _samplesListTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / _samples.Count));
                }
            }

            _samplesListTable.AutoScroll = true;           

            _chooseLetterComboBox.Items.Clear();
            foreach (var sample in _samples)
                _chooseLetterComboBox.Items.Add(sample.Name);

            _chooseLetterComboBox.SelectedIndex = 0;

            _countNeuronsTextBox.Text = Convert.ToString(NeuronsCount);
            _countSamplesTextBox.Text = Convert.ToString(_samples.Count);
            _countInputsTextBox.Text = Convert.ToString(InputsCount);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {          
            _countNeuronsTextBox.Text = Convert.ToString(DefaultNeuronsCount);
            _countInputsTextBox.Text = "0";
            _countSamplesTextBox.Text = "0";
        }
    
        private void OnLearnButtonClicked(object sender, EventArgs e)
        {
            _weightsMtx = new double[InputsCount, NeuronsCount];
            _newSample = new double[RowsCount, ColumnsCount];
            _targets = MathUtil.IdentityMatrix(SamplesCount, NeuronsCount);

            // задание входных сигналов
            double[,] inputsMtx = new double[InputsCount, SamplesCount];
            for (int row = 0, inputIndex = 0; row < RowsCount; row++)
            {
                for (int col = 0; col < ColumnsCount; col++)
                {                         
                    for(int sampleIndex = 0; sampleIndex < _samples.Count; ++sampleIndex)
                    {
                        inputsMtx[inputIndex, sampleIndex] = _samples[sampleIndex].InputsMtx[row, col];         
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
            _resultSampleTextBox.Text = "";

            FillGridViewWithMatrix(_newLetterGridView, ChoosenSample.InputsMtx);
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            _netOutputsTextBox.Text = "";
            _resultSampleTextBox.Text = "";
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
            _resultSampleTextBox.Text = "";

            double[,] xf = MathUtil.Vectorize(_newSample);
            double[,] yf = MathUtil.Apply(MathUtil.Multiply(MathUtil.Transform(_weightsMtx), xf), MathUtil.ActivationFunction.Sigmoid);

            double[] outputs = MathUtil.VectorToArray(yf);
            _netOutputsTextBox.Text = MathUtil.Stringify(outputs);

            const double accuracy = 0.5;
            _resultSampleTextBox.Text = "не распознан";
            for (int sampleIndex = 0; sampleIndex < _samples.Count; ++sampleIndex)
            {
                if (MathUtil.IsAbout(outputs, _targets[sampleIndex], accuracy))
                {
                    _resultSampleTextBox.Text = _samples[sampleIndex].Name;
                    break;
                }
            }                
        }
    }
}