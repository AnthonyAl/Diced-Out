using System;
using System.IO;
using System.Windows.Forms;

namespace DiceGame
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text.Length > 0 && textBox2.Text.Length > 0)
            {
                try
                {
                    //checks if the username is already registered.
                    StreamReader sr = new StreamReader("res/data/passwords.txt");
                    string s = sr.ReadLine();
                    bool found = false;
                    while (s != null)
                    {
                        if (s.Split('|')[0].Equals(textBox1.Text))
                        {
                            found = true;
                            break;
                        }
                        else
                        {
                            s = sr.ReadLine();
                        }
                    }
                    sr.Close();

                    if (found) MessageBox.Show("The provided username is taken.");
                    //if the name is not registered, this part will append the passwords, scores and players files with the new player's data.
                    else
                    {
                        StreamWriter password = new StreamWriter("res/data/passwords.txt", Enabled);
                        StreamWriter score = new StreamWriter("res/data/scores.txt", Enabled);
                        StreamWriter player = new StreamWriter("res/data/players.txt", Enabled);

                        password.WriteLine(textBox1.Text + "|" + textBox2.Text);
                        score.WriteLine(textBox1.Text + "|0");

                        string age = "hidden";
                        string gender = "other";
                        string description = "no description provided";
                        if (textBox4.Text.Length > 0) age = int.Parse(textBox4.Text).ToString();
                        if (textBox6.Text.Length > 0) description = textBox6.Text;
                        if (listBox1.Text.Length > 0) gender = listBox1.Text;

                        player.WriteLine(textBox1.Text + "|" + age + "|" + gender + "|" + description);

                        password.Close();
                        score.Close();
                        player.Close();

                        MessageBox.Show("Welcome to Diced Out!");
                        this.Close();
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("Something went wrong, please try again. If the problem persists, please contact our support team. CodeFIO");
                }
            }
            else
            {
                MessageBox.Show("The username and the password are mandatory.");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Code that lets the textbox accept only specific inputs.
            if (textBox1.Text.Length > 0)
            {
                for (int i = 0; i < textBox1.Text.Length; i++)
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text.Substring(i), "^[a-zA-Z ]"))
                    {
                        MessageBox.Show("Usernames accept only alphabetical characters.");
                        textBox1.Text = textBox1.Text.Remove(i, 1);
                    }
                }
            }
            if (textBox1.Text.Length > 20)
            {
                MessageBox.Show("Usernames can only be up to 20 characters.");
                textBox1.Text = textBox1.Text.Remove(20, 1);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //Code that lets the textbox accept only specific inputs.
            if (textBox2.Text.Length > 0)
            {
                for (int i = 0; i < textBox2.Text.Length; i++)
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(textBox2.Text.Substring(i), "^[a-zA-Z0-9 ]"))
                    {
                        MessageBox.Show("Passwords accept only alphabetical characters and/or numbers.");
                        textBox2.Text = textBox2.Text.Remove(i, 1);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Shows - Hides the password characters.
            if (textBox2.PasswordChar == '\0')
            {
                textBox2.PasswordChar = '•';
                button3.Text = "Show";
            }
            else if (textBox2.PasswordChar == '•')
            {
                textBox2.PasswordChar = '\0';
                button3.Text = "Hide";
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //Code that lets the textbox accept only specific inputs.
            if (textBox4.Text.Length > 0)
            {
                for (int i = 0; i < textBox4.Text.Length; i++)
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(textBox4.Text.Substring(i), "^[0-9 ]"))
                    {
                        MessageBox.Show("The age must be a number.");
                        textBox4.Text = textBox4.Text.Remove(i, 1);
                    }
                }
                if (textBox4.Text.Length > 0)
                {
                    int j = int.Parse(textBox4.Text);
                    if (j < 1)
                    {
                        textBox4.Text = "1";
                    }
                    else if (j > 160)
                    {
                        textBox4.Text = "160";
                    }
                }
            }
        }
    }
}
