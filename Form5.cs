using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiceGame
{
    public partial class Form5 : Form
    {

        //the array of the leaderboard scores.
        int[] lbscores;
        //the list of the leaderboard names.
        List<string> lbnames;
        //a long list of each label for a name or a score in the form.
        List<Label> label = new List<Label>();
        //a richTextBox that will appear and show more user information.
        RichTextBox richTextBox1;
        //this variable short of 'remembers' the panel that has a richTextBox still spawned in it, so the textBox can be removed when the next panel spawns another.
        Panel temp_panel;

        public Form5(int[] lbscores, List<string> lbnames)
        {
            InitializeComponent();
            this.lbscores = lbscores;
            this.lbnames = lbnames;
            //adds all the labels in the list.
            create_Label_List();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            int j = 0;
            for(int i = 0; i <= 52; i += 3)
            {
                label[i].Text = lbscores[j].ToString();
                j++;
            }
            j = 0;
            for (int i = 2; i <= 53; i += 3)
            {
                label[i].Text = lbnames.ElementAt(j).ToString();
                j++;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label_Click(object sender, EventArgs e)
        {
        }
        private void label_MouseDown(object sender, MouseEventArgs e)
        {
            //removes the previously spawned richTextBox to re-reveal the leaderboard content.
            if(temp_panel != null) temp_panel.Controls.Remove(richTextBox1);
            richTextBox1 = new RichTextBox();

            //grabs the additional player information based on the label clicked.
            try
            {
                StreamReader sr = new StreamReader("res/data/players.txt");
                StringBuilder sb = new StringBuilder();
                string[] prefix = { "Name: ", "Age: ", "Gender: ", "Self Description: " };
                string s = sr.ReadLine();
                int i = 0;
                while (s != null)
                {
                    string[] arr = s.Split('|');
                    if (arr[0] == ((Label)sender).Text)
                    {
                        foreach (string str in arr)
                        {
                            sb.Append(prefix[i]);
                            sb.Append(str);
                            if(i < 3) sb.Append(Environment.NewLine);
                            i++;
                        }
                        break;
                    }
                    else
                    {
                        s = sr.ReadLine();
                    }
                }
                sr.Close();
                richTextBox1.Text = sb.ToString();
            }
            catch (Exception)
            {
                richTextBox1.Text = "-error-";
            }

            //adds the richTextBox in the parent panel to the label clicked.

            richTextBox1.Location = new Point(0, 0);
            richTextBox1.Size = new Size(275, 30);
            richTextBox1.ForeColor = Color.White;
            richTextBox1.Font = new Font("Comic Sans MS", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            richTextBox1.BackColor = ((Label)sender).Parent.BackColor;
            richTextBox1.ReadOnly = true;
            ((Label)sender).Parent.Controls.Add(richTextBox1);
            richTextBox1.BringToFront();
        }

        private void label_MouseUp(object sender, MouseEventArgs e)
        {
            //originally, when the mouse got released the richTextBox would be removed.
            temp_panel = (Panel)((Label)sender).Parent;
        }
            
        //Just a lot of label.Add()s.    
        private void create_Label_List()
        {
            label.Add(label1);
            label.Add(label2);
            label.Add(label3);
            label.Add(label4);
            label.Add(label5);
            label.Add(label6);
            label.Add(label7);
            label.Add(label8);
            label.Add(label9);
            label.Add(label10);
            label.Add(label11);
            label.Add(label12);
            label.Add(label13);
            label.Add(label14);
            label.Add(label15);
            label.Add(label16);
            label.Add(label17);
            label.Add(label18);
            label.Add(label19);
            label.Add(label20);
            label.Add(label21);
            label.Add(label22);
            label.Add(label23);
            label.Add(label24);
            label.Add(label25);
            label.Add(label26);
            label.Add(label27);
            label.Add(label28);
            label.Add(label29);
            label.Add(label30);
            label.Add(label31);
            label.Add(label32);
            label.Add(label33);
            label.Add(label34);
            label.Add(label35);
            label.Add(label36);
            label.Add(label37);
            label.Add(label38);
            label.Add(label39);
            label.Add(label40);
            label.Add(label41);
            label.Add(label42);
            label.Add(label43);
            label.Add(label44);
            label.Add(label45);
            label.Add(label46);
            label.Add(label47);
            label.Add(label48);
            label.Add(label49);
            label.Add(label50);
            label.Add(label51);
            label.Add(label52);
            label.Add(label53);
            label.Add(label54);
        }

    }
}
