using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BooleanExpressions
{
    public partial class Form1 : Form
    {
        List<string> letras = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r" };
        int count = 1;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int distance = 45;
            textBox1.Size = new Size(this.Width - distance, 20);
            btnStart.Size = new Size(this.Width - distance, 23);
            //776; 371
            dataGridView1.Size = new Size(this.Width - distance, this.Height - 125);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (this.dataGridView1.Columns[e.ColumnIndex].Name == "a") // Substitua "ColumnName" pelo nome da coluna que deseja verificar
            //{
            if (Convert.ToInt32(e.Value) > 0) // Se o valor da célula for maior que zero
            {
                e.CellStyle.BackColor = Color.Green; // Definir cor da célula como verde
            }
            else
            {
                e.CellStyle.BackColor = Color.Red; // Definir cor da célula como vermelha
            }
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (count <= letras.Count)
            {
                textBox1.Clear();
                for (int i = 0; i < count; i++)
                {
                    textBox1.Text += $"{letras[i]}";
                }
                count++;
                timer1.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show($"{dataGridView1.Rows.Count} possibilities");
                timer1.Enabled = false;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //dataGridView1.DataSource = null;
            dataGridView1.Invoke((MethodInvoker)delegate
            {
                dataGridView1.DataSource = null;
            });
            char[] preposicoes = textBox1.Text.ToArray();
            double columnCount = preposicoes.Length;
            double lineCount = Math.Pow(2, columnCount);
            double division = lineCount / 2;
            double count_ = 0;
            DataTable dt = new DataTable();
            int[,] _lines = new int[(int)columnCount, (int)lineCount];//colunas - linhas
            List<string> lines = new List<string>();
            for (int i = 0; i < columnCount; i++)//colunas
            {
                dt.Columns.Add($"{preposicoes[i]}");
                for (int j = 0; j < (int)lineCount; j++)//linhas
                {
                    int value;
                    if (count_ < division)
                    {
                        value = 0;
                        count_++;
                    }
                    else
                    {
                        value = 1;
                        count_++;
                        if (count_ == division * 2)
                        {
                            count_ = 0;
                        }
                    }
                    _lines[i, j] = value;
                    //Console.WriteLine($"{value}");
                }
                count_ = 0;
                division /= 2;
            }
            // Percorrer o array e adicionar as linhas ao DataTable
            for (int i = 0; i < _lines.GetLength(1); i++)//linhas
            {
                for (int j = 0; j < _lines.GetLength(0); j++)//colunas
                {
                    lines.Add($"{_lines[j, i]}");
                }
                dt.Rows.Add(lines.ToArray());
                lines.Clear();
            }
            //dataGridView1.DataSource = dt;
            dataGridView1.Invoke((MethodInvoker)delegate
            {
                dataGridView1.DataSource = dt;
            });
            //
            dataGridView1.Rows[0].DefaultCellStyle.SelectionBackColor = Color.Red;
            dataGridView1.Rows[0].DefaultCellStyle.SelectionForeColor = Color.Black;
            //
            // Centralizar o texto das células e cabeçalhos do DataGridView
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //MessageBox.Show($"{preposicoes.Length} propositions | {dataGridView1.Rows.Count} possibilities");
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer1.Enabled = true;
        }
    }
}
