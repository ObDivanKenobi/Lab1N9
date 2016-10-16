using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1N9
{
    public partial class Form1 : Form
    {
        const int MaxNum = 9;
        const int MaxDegree = 9;

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            textBoxSource.Clear();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Задано натуральное число M. Требуется найти разложение этого числа в виде i1^k1 + i2^k2 + … + in^kn = M, где все 0<= Ij <= 9, 0<= Kj <= 9.", "О задаче");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string exp;
            int N;
            if (!int.TryParse(textBoxSource.Text, out N))
            {
                MessageBox.Show("Ошибка. Введены некорректные данные - не удаётся преобразовать в число!");
                textBoxSource.Clear();
                textBoxResult.Clear();
                return;
            }

            try
            {
                Expand(N, MaxNum, MaxDegree, out exp);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Невозможно разложить число указанным образом!");
                return;
            }

            textBoxResult.Text = exp;
        }

        void Expand(int number, int maxNum, int maxDegree, out string expansion)
        {
            expansion = "Невозможно разложить число!";
            int length = 1;
            bool found = false;
            while (!found)
            {
                Combination numbers = new Combination(length, maxNum);
                Placement degrees = new Placement(length, maxDegree);
                found = Check(numbers, degrees, number);
                bool needMoreNumbers = false;
                while (!found && !needMoreNumbers)
                {
                    while (!found && degrees.NextLexicographical())
                        found = Check(numbers, degrees, number);
                    if (!found)
                    {
                        degrees = new Placement(length, maxDegree);
                        needMoreNumbers = !numbers.NextLexicographical();
                    }
                }
                if (!found) ++length;
                else expansion = FormResult(numbers, degrees, number);
            }
        }

        bool Check(Combination n, Placement d, int number)
        {
            long sum = 0;
            int i = 0;
            while (sum <= number && i < n.Length)
            {
                sum += (int)Math.Pow(Convert.ToDouble(n[i]), Convert.ToDouble(d[i]));
                ++i;
            }

            if (sum != number)
                return false;
            else
                return true;
        }

        string FormResult(Combination n, Placement d, int number)
        {
            string result = string.Format("{0} = ", number);
            bool first = true;
            for (int i = 0; i < n.Length; ++i)
            {
                if (n[i] == 0)
                    continue;
                else
                {
                    char superscript;
                    if (d[i] == 1)
                        superscript = Convert.ToChar(0x00B9);
                    else if (d[i] == 2 || d[i] == 3)
                        superscript = Convert.ToChar(0x00B0 + d[i]);
                    else
                        superscript = Convert.ToChar(0x2070 + d[i]);
                    string add = string.Format("{0}{1}", n[i], superscript);
                    if (first)
                        first = false;
                    else
                        add = " + " + add;
                    result += add;
                }
            }
            return result;
        }
    }
}