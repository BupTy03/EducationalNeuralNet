namespace neuro_01_main
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._countInputsTextBox = new System.Windows.Forms.TextBox();
            this._countNeuronsTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this._letterAGridView = new System.Windows.Forms.DataGridView();
            this._letterBGridView = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this._countSamplesTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this._newLetterGridView = new System.Windows.Forms.DataGridView();
            this.button3 = new System.Windows.Forms.Button();
            this._resultLetterTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this._chooseLetterComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this._netOutputsTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this._letterAGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._letterBGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._newLetterGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Число вход.сигналов";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Число НЕЙРОНОВ";
            // 
            // _countInputsTextBox
            // 
            this._countInputsTextBox.Location = new System.Drawing.Point(164, 20);
            this._countInputsTextBox.Name = "_countInputsTextBox";
            this._countInputsTextBox.Size = new System.Drawing.Size(41, 20);
            this._countInputsTextBox.TabIndex = 4;
            this._countInputsTextBox.Text = "10";
            // 
            // _countNeuronsTextBox
            // 
            this._countNeuronsTextBox.Location = new System.Drawing.Point(164, 49);
            this._countNeuronsTextBox.Name = "_countNeuronsTextBox";
            this._countNeuronsTextBox.Size = new System.Drawing.Size(39, 20);
            this._countNeuronsTextBox.TabIndex = 5;
            this._countNeuronsTextBox.Text = "3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Обучающие примеры";
            // 
            // _letterAGridView
            // 
            this._letterAGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._letterAGridView.Location = new System.Drawing.Point(12, 151);
            this._letterAGridView.Name = "_letterAGridView";
            this._letterAGridView.RowHeadersWidth = 20;
            this._letterAGridView.Size = new System.Drawing.Size(304, 260);
            this._letterAGridView.TabIndex = 7;
            // 
            // _letterBGridView
            // 
            this._letterBGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._letterBGridView.Location = new System.Drawing.Point(322, 151);
            this._letterBGridView.Name = "_letterBGridView";
            this._letterBGridView.RowHeadersWidth = 20;
            this._letterBGridView.Size = new System.Drawing.Size(300, 260);
            this._letterBGridView.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(164, 115);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "1. Заполнить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnFillButtonClicked);
            // 
            // _countSamplesTextBox
            // 
            this._countSamplesTextBox.Location = new System.Drawing.Point(166, 79);
            this._countSamplesTextBox.Name = "_countSamplesTextBox";
            this._countSamplesTextBox.Size = new System.Drawing.Size(39, 20);
            this._countSamplesTextBox.TabIndex = 10;
            this._countSamplesTextBox.Text = "2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Число обуч.примеров";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(285, 71);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 74);
            this.button2.TabIndex = 12;
            this.button2.Text = "2. ОБУЧЕНИЕ НЕЙРОСЕТИ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnLearnButtonClicked);
            // 
            // _newLetterGridView
            // 
            this._newLetterGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._newLetterGridView.Location = new System.Drawing.Point(628, 12);
            this._newLetterGridView.Name = "_newLetterGridView";
            this._newLetterGridView.Size = new System.Drawing.Size(477, 255);
            this._newLetterGridView.TabIndex = 13;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(418, 19);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(121, 43);
            this.button3.TabIndex = 14;
            this.button3.Text = "3. Сформировать сигнал";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OnGenerateButtonClicked);
            // 
            // _resultLetterTextBox
            // 
            this._resultLetterTextBox.Location = new System.Drawing.Point(798, 312);
            this._resultLetterTextBox.Name = "_resultLetterTextBox";
            this._resultLetterTextBox.Size = new System.Drawing.Size(179, 20);
            this._resultLetterTextBox.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(629, 312);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(140, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Результат распознования";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(418, 98);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(121, 47);
            this.button4.TabIndex = 18;
            this.button4.Text = "4. Сохранить изменения";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.OnSaveButtonClicked);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(632, 273);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(149, 23);
            this.button5.TabIndex = 19;
            this.button5.Text = "5. Распознование";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.OnRecognitionButtonClicked);
            // 
            // _chooseLetterComboBox
            // 
            this._chooseLetterComboBox.FormattingEnabled = true;
            this._chooseLetterComboBox.Location = new System.Drawing.Point(418, 71);
            this._chooseLetterComboBox.Name = "_chooseLetterComboBox";
            this._chooseLetterComboBox.Size = new System.Drawing.Size(121, 21);
            this._chooseLetterComboBox.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(671, 346);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Выходы нейронов";
            // 
            // _netOutputsTextBox
            // 
            this._netOutputsTextBox.Location = new System.Drawing.Point(798, 343);
            this._netOutputsTextBox.Name = "_netOutputsTextBox";
            this._netOutputsTextBox.Size = new System.Drawing.Size(179, 20);
            this._netOutputsTextBox.TabIndex = 22;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 426);
            this.Controls.Add(this._netOutputsTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this._chooseLetterComboBox);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this._resultLetterTextBox);
            this.Controls.Add(this.button3);
            this.Controls.Add(this._newLetterGridView);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._countSamplesTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this._letterBGridView);
            this.Controls.Add(this._letterAGridView);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._countNeuronsTextBox);
            this.Controls.Add(this._countInputsTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "Form1";
            this.Text = "Однослойная нейронная сеть - учебный пример";
            this.Load += new System.EventHandler(this.OnFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this._letterAGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._letterBGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._newLetterGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _countInputsTextBox;
        private System.Windows.Forms.TextBox _countNeuronsTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView _letterAGridView;
        private System.Windows.Forms.DataGridView _letterBGridView;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox _countSamplesTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView _newLetterGridView;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox _resultLetterTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ComboBox _chooseLetterComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox _netOutputsTextBox;

    }
}

