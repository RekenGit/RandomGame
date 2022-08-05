using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LosujGierke
{
    public partial class Glowny : Form
    {
        Random rand = new Random();
        List<string> options = new List<string>();
        string[] optionsArr;
        string[] optionChangedInt;
        bool openOption = false;
        public Glowny()
        {
            InitializeComponent();
            select(0);
            panel1.Visible = false;
            textBox2.Text = "";
        }
        void select(int x)
        {
            if (x == 0)//0-Dodawanie
            {
                if (openOption)
                {
                    panel1.Visible = false;
                    openOption = false;
                }
                button8.Visible = true;
                textBox1.Visible = true;
                button1.Visible = true;
                button2.Visible = true;
                listView1.Visible = true;
                button4.Visible = true;
                button3.Visible = false;
                flowLayoutPanel1.Visible = false;
            }
            else if (x == 1)//1-Losowanie
            {
                if (openOption)
                {
                    panel1.Visible = false;
                    openOption = false;
                }
                button8.Visible = false;
                textBox1.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                listView1.Visible = false;
                button4.Visible = false;
                button3.Visible = true;
                flowLayoutPanel1.Visible = true;
            }
        }
        void addRandOptions()
        {
            int count = listView1.Items.Count;
            optionsArr = options.ToArray();

            for (int i = 0; i < count; i++)
            {
                Button newButton = new Button();
                newButton.Name = i+"";
                newButton.Text = "?";
                if(optionsArr.Length < 13) newButton.Size = new Size(130, 130);
                else if(optionsArr.Length < 31) newButton.Size = new Size(84, 84);
                else newButton.Size = new Size(84, 40);
                newButton.Click += revealOption;
                flowLayoutPanel1.Controls.Add(newButton);
            }
        }
        void sortOptions()
        {
            int count = listView1.Items.Count;
            optionChangedInt = new string[count];
            string[] optRnd = optionChangedInt.ToArray();
            int[] optRnd_temp = new int[count];
            for (int i = 0; i < count; i++)
            {
                optRnd[i] = rand.Next(0, 300) + "";
                optRnd_temp[i] = Int32.Parse(optRnd[i]);
            }
            Array.Sort(optRnd_temp);
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < optRnd.Length; j++)
                {
                    if (optRnd_temp[i].ToString() == optRnd[j])
                    {
                        optRnd_temp[i] = 400;
                        optRnd[j] = "x";
                        optionChangedInt[j] = i+"";
                        //MessageBox.Show(optRnd_temp[i].ToString() + "    " + optRnd[j] + "    " + optionChangedInt[j]);
                        break;
                    }
                }
            }
        }
        private void revealOption(object sender, EventArgs e)
        {
            for (int i = 0; i < optionChangedInt.Length; i++)
            {
                if (((Button)sender).Name == optionChangedInt[i])
                {
                    ((Button)sender).Text = optionsArr[i];
                    ((Button)sender).Enabled = false;
                    ((Button)sender).BackColor = Color.Gray;
                    ((Button)sender).FlatStyle = FlatStyle.Flat;
                }
            }
        }
        private void Glowny_Load(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            select(1);
            flowLayoutPanel1.Controls.Clear();
            sortOptions();
            addRandOptions();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            select(0);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            dodaj();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            usun();
        }

        List<string> lines;

        private void button5_Click(object sender, EventArgs e)//Import
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Plik listy Gier|*.lgfile";
            if(fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = fd.FileName;
            }
            else
            {
                textBox2.Text = "";
            }
            
        }
        private void button9_Click(object sender, EventArgs e)//Export
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Plik listy Gier|*.lgfile";
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (Stream s = File.Open(fd.FileName, FileMode.CreateNew))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    foreach(string x in options)
                    {
                        sw.WriteLine(x);
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != null && textBox2.Text != "")
            {
                lines = File.ReadAllLines(textBox2.Text).ToList();
                foreach (string s in lines)
                {
                    string x = Regex.Replace(s, @"\s+", " ");
                    if (x != "" && x != null && x != " ")
                    {
                        listView1.Items.Add(x);
                        options.Add(x);
                    }
                }
            }
            else
            {
                MessageBox.Show("Ścieżka do pliku nie została wybrana.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            openOption = false;
            panel1.Visible = false;
            button8.Visible = true;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            openOption = true;
            panel1.Visible = true;
            button8.Visible = false;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dodaj();
                e.SuppressKeyPress = true;
            }
        }
        void dodaj()
        {
            string addName = textBox1.Text;
            if (String.IsNullOrWhiteSpace(addName))
            {
                MessageBox.Show("Pole na nazwę, nie może być puste.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                listView1.Items.Add(addName);
                options.Add(addName);
                textBox1.Text = "";
            }
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                usun();
                e.SuppressKeyPress = true;
            }
        }
        void usun()
        {
            int count = listView1.SelectedItems.Count;
            if (listView1.SelectedItems.Count == 1)
            {
                string text = listView1.SelectedItems[0].Text;
                listView1.Items.Remove(listView1.SelectedItems[0]);
                options.Remove(text);
            }
            else if (listView1.SelectedItems.Count > 1)
            {
                string textElement;
                if (count > 1 && count < 5) textElement = " elementy.";
                else textElement = " elementów.";
                if (MessageBox.Show("Czy chcesz usunąć " + count + textElement, "Uwaga!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < count; i++)
                    {
                        string text = listView1.SelectedItems[0].Text;
                        listView1.Items.Remove(listView1.SelectedItems[0]);
                        options.Remove(text);
                    }
                }
            }
        }
    }
}
