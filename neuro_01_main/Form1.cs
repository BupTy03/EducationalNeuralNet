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
        public class helpfun
        {
            public static float press(float s)
            {
                //   сжимающая функция
                double ss = s;
                double fss = 1 / (1 + Math.Exp(-ss));
                float fs = Convert.ToSingle(fss);
                return fs;
            }
        }
        //      однослойная нейронная сеть
        const int mmax = 100; // максимальное количество входных сигналов
        const int nmax = 10; // максимальное количество нейронов
        const int tmax = 10; // максимальное количество обучающих примеров
        const int mdr = 15; // число строк матрицы образа буквы
        const int mdc = 10; // число столбцов матрицы образа буквы
        const int mr = 10;
        const int mc = 7;
        const float eps = 0.001f; // точность для w1
                                  //   
        int mx; // количество входных сигналов по факту
        int nn; // количество нейронов по факту
        int tn; // количество обучающих примеров по факту
                //   
        float[,] x = new float[mmax, tmax]; // входящие сигналы
        float[,] y = new float[nmax, tmax]; // выходящие сигналы
        float[,] dt = new float[nmax, tmax]; // обучающие примеры
        float[,] w1 = new float[mmax, nmax]; // матрица весов
        float[,] dw1 = new float[mmax, nmax]; // поправки к матрице весов
        float[,] Ad = new float[mdr, mdc]; // буква А
        float[,] Bd = new float[mdr, mdc]; // буква Б
        float[,] Bdf = new float[mdr, mdc]; // буква дефектная
        float[] xf = new float[mmax]; // входящий сигнал
        float[] yf = new float[nmax]; // выходящий сигнал
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tn = 2;
            mx = mr * mc;
            nn = 3;
            textBox1.Text = Convert.ToString(mx);
            textBox2.Text = Convert.ToString(nn);
            textBox3.Text = Convert.ToString(tn);

            //      вектор обучения: А соотв.вектору (1;0;0); Б соотв.вектору (0;1;0)
            for (int it = 1; it <= tn; it++)
            {
                for (int j = 1; j <= nn; j++)
                {
                    dt[j, it] = 0;
                    if (j == it)
                    {
                        dt[j, it] = 1;
                    }
                }
            }
            //  здесь задаем обучающие примеры: буквы А и Б            
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    this.dataGridView1.Rows[i].Cells[j].Value = Convert.ToString("");
                    this.dataGridView2.Rows[i].Cells[j].Value = Convert.ToString("");
                }
            }
            //  буква А    
            for (int i = 0; i <= 9; i++)
            {
                this.dataGridView1.Rows[i].Cells[5].Value = Convert.ToString(1);
                this.dataGridView1.Rows[i].Cells[6].Value = Convert.ToString(1);
            }
            for (int j = 1; j <= 6; j++)
            {
                this.dataGridView1.Rows[7].Cells[j].Value = Convert.ToString(1);
                this.dataGridView1.Rows[8].Cells[j].Value = Convert.ToString(1);
            }
            this.dataGridView1.Rows[1].Cells[4].Value = Convert.ToString(1);
            this.dataGridView1.Rows[2].Cells[4].Value = Convert.ToString(1);
            this.dataGridView1.Rows[3].Cells[3].Value = Convert.ToString(1);
            this.dataGridView1.Rows[3].Cells[4].Value = Convert.ToString(1);
            this.dataGridView1.Rows[4].Cells[3].Value = Convert.ToString(1);
            this.dataGridView1.Rows[4].Cells[4].Value = Convert.ToString(1);
            this.dataGridView1.Rows[5].Cells[2].Value = Convert.ToString(1);
            this.dataGridView1.Rows[5].Cells[3].Value = Convert.ToString(1);
            this.dataGridView1.Rows[6].Cells[2].Value = Convert.ToString(1);
            this.dataGridView1.Rows[6].Cells[3].Value = Convert.ToString(1);
            this.dataGridView1.Rows[9].Cells[0].Value = Convert.ToString(1);
            this.dataGridView1.Rows[9].Cells[1].Value = Convert.ToString(1);
            //  буква Б    
            for (int i = 0; i <= 9; i++)
            {
                this.dataGridView2.Rows[i].Cells[0].Value = Convert.ToString(1);
                this.dataGridView2.Rows[i].Cells[1].Value = Convert.ToString(1);
            }
            for (int j = 0; j <= 6; j++)
            {
                this.dataGridView2.Rows[0].Cells[j].Value = Convert.ToString(1);
                this.dataGridView2.Rows[1].Cells[j].Value = Convert.ToString(1);
                this.dataGridView2.Rows[5].Cells[j].Value = Convert.ToString(1);
                this.dataGridView2.Rows[8].Cells[j].Value = Convert.ToString(1);
            }
            for (int j = 0; j <= 5; j++)
            {
                this.dataGridView2.Rows[4].Cells[j].Value = Convert.ToString(1);
                this.dataGridView2.Rows[9].Cells[j].Value = Convert.ToString(1);
            }
            this.dataGridView2.Rows[6].Cells[5].Value = Convert.ToString(1);
            this.dataGridView2.Rows[6].Cells[6].Value = Convert.ToString(1);
            this.dataGridView2.Rows[7].Cells[5].Value = Convert.ToString(1);
            this.dataGridView2.Rows[7].Cells[6].Value = Convert.ToString(1);
            
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    string tt1 = Convert.ToString(this.dataGridView1.Rows[i].Cells[j].Value);
                    string tt2 = Convert.ToString(this.dataGridView2.Rows[i].Cells[j].Value);
                    Ad[i + 1, j + 1] = 0;
                    if (tt1 == "1")
                    {
                        Ad[i + 1, j + 1] = 1;
                    };
                    Bd[i + 1, j + 1] = 0;
                    if (tt2 == "1")
                    {
                        Bd[i + 1, j + 1] = 1;
                    };
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //  параметры таблицы для обучающих примеров
            comboBox1.SelectedIndex = 0;

            for (int i = 0; i <= 6; i++)
            {
                this.dataGridView1.Columns.Add("c" + Convert.ToString(i), "Hc" + Convert.ToString(i));
                this.dataGridView2.Columns.Add("c" + Convert.ToString(i), "Hc" + Convert.ToString(i));
                this.dataGridView3.Columns.Add("c" + Convert.ToString(i), "Hc" + Convert.ToString(i));
                this.dataGridView1.Columns[i].Width = 30;
                this.dataGridView2.Columns[i].Width = 30;
                this.dataGridView3.Columns[i].Width = 30;
            }

            for (int i = 0; i <= 9; i++) // Цикл добавления строк
            {
                this.dataGridView1.Rows.Add(); // добавление строки 
                this.dataGridView2.Rows.Add(); // добавление строки
                this.dataGridView3.Rows.Add(); // добавление строки
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // обучение 
            // задание входных сигналов
            int ix = 0;
            for (int i = 1; i <= mr; i++)
            {
                for (int j = 1; j <= mc; j++)
                {
                    ix++;
                    x[ix, 1] = Ad[i, j];
                    x[ix, 2] = Bd[i, j];
                }
            }

            // матрица весов
            float eta = 1;
            for (int i = 1; i <= mx; i++)
            {
                for (int j = 1; j <= nn; j++)
                {
                    w1[i, j] = Convert.ToSingle(Math.Pow(-1, (i + j)));
                    dw1[i, j] = 0;
                }
            }


            int iterm = 10;
            int Flag = 0;
            for (int iter = 1; iter <= iterm; iter++)
            {
                Flag = 0;
                // calc y
                for (int it = 1; it <= tn; it++)
                {
                    for (int j = 1; j <= nn; j++)
                    {
                        float s0 = 0;
                        for (int i = 1; i <= mx; i++)
                        {
                            s0 = s0 + x[i, it] * w1[i, j];
                        }

                        y[j, it] = helpfun.press(s0);
                    }
                }

                // Image
                // optimization w
                for (int i = 1; i <= mx; i++)
                {
                    for (int j = 1; j <= nn; j++)
                    {
                        float dEw = 0;
                        for (int it = 1; it <= tn; it++)
                        { // Image
                            dEw = dEw + (y[j, it] - dt[j, it]) * (1 - y[j, it]) * y[j, it] * x[i, it];
                        } // Image
                        float tol = eps * Math.Abs(dEw) + eps;
                        float error = Math.Abs(-dEw - dw1[i, j] / eta);
                        if (error > tol)
                        {
                            Flag = 1;
                        }
                        dw1[i, j] = -eta * dEw;                       
                    }
                }
                
                for (int i = 1; i <= mx; i++)
                {
                    for (int j = 1; j <= nn; j++)
                    {
                        w1[i, j] = w1[i, j] + dw1[i, j];
                    }
                }
                if (Flag == 0)
                {
                    iterm = iter;
                    break;
                }
            }

            MessageBox.Show(" Education OK; Flag=" + Convert.ToString(Flag));

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            textBox4.Text = "";

            // Распознование нечетко написанных букв
            float rr = 0;
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    Bdf[i + 1, j + 1] = 0;
                    if (comboBox1.SelectedIndex == 0)
                    {
                        rr = Ad[i + 1, j + 1];
                    }
                    if (comboBox1.SelectedIndex == 1)
                    {
                        rr = Bd[i + 1, j + 1];
                    }
                    if (rr == 1)
                    {
                        this.dataGridView3.Rows[i].Cells[j].Value = Convert.ToString(rr);
                    }
                    else
                    {
                        this.dataGridView3.Rows[i].Cells[j].Value = Convert.ToString(" ");
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            textBox4.Text = "";
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    string tt1 = Convert.ToString(this.dataGridView3.Rows[i].Cells[j].Value);
                    Bdf[i + 1, j + 1] = 0;
                    if (tt1 == "1")
                    {
                        Bdf[i + 1, j + 1] = 1;
                    };
                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            textBox4.Text = "";
            string goal = "неизвестная буква";
            int ix = 0;
            for (int i = 1; i <= mr; i++)
            {
                for (int j = 1; j <= mc; j++)
                {
                    ix++;
                    xf[ix] = Bdf[i, j];
                }
            }
            string str = "";
            // calc y
            for (int j = 1; j <= nn; j++)
            {
                float s0 = 0;
                for (int i = 0; i <= mx; i++)
                {
                    s0 = s0 + xf[i] * w1[i, j];
                }
                yf[j] = helpfun.press(s0);
                decimal yfj = Convert.ToDecimal(yf[j]);
                string st1 = Convert.ToString(yfj);
                st1 = st1.Substring(0, 4);
                str = str + st1 + "  ";
            }
            textBox5.Text = str;
            if ((yf[1] >= 0.8) & (yf[2] <= 0.2) & (yf[3] <= 0.2))
            {
                goal = "А";
            }
            if ((yf[1] <= 0.2) & (yf[2] >= 0.8) & (yf[3] <= 0.2))
            {
                goal = "Б";
            }
            textBox4.Text = goal;
        }

    }
}