using System;
using System.IO;
using System.Windows.Forms;

namespace DiceGame
{
    public partial class LogIn : Form
    {

        Menu menu;

        public LogIn(Menu menu)
        {
            InitializeComponent();
            this.menu = menu;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            menu.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new SignUp().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //searches in the passwords file for a matching combination of a username and password with the ones provided. If everything goes well,
            //the player name and high-score are stored in variables for the game's needs.
            try
            {
                StreamReader sr = new StreamReader("res/data/passwords.txt");
                StreamReader score = new StreamReader("res/data/scores.txt");
                string s = sr.ReadLine();
                string sc = score.ReadLine();
                bool found = false;
                while (s != null)
                {
                    if (s.Split('|')[0].Equals(textBox1.Text) && s.Split('|')[1].Equals(textBox2.Text))
                    {
                        menu.play.Enabled = true;
                        menu.label1.Hide();
                        menu.player = s.Split('|')[0];
                        menu.high_score = int.Parse(sc.Split('|')[1]);
                        found = true;
                        break;
                    }
                    else
                    {
                        s = sr.ReadLine();
                        sc = score.ReadLine();
                    }
                }
                sr.Close();
                score.Close();
                if (!found) MessageBox.Show("The provided username and/or password were incorrect.");
                else
                {
                    menu.Show();
                    this.Close();
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Something went wrong, please try again. If the problem persists, please contact our support team. CodeFIO");
            }
        }

        //Shows - Hides the password characters.
        private void showHide_Click(object sender, EventArgs e)
        {
            if (textBox2.PasswordChar == '\0')
            {
                textBox2.PasswordChar = '•';
                showHide.Text = "Show";
            }
            else if (textBox2.PasswordChar == '•')
            {
                textBox2.PasswordChar = '\0';
                showHide.Text = "Hide";
            }
        }
    }
}
