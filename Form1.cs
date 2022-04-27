using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace DiceGame
{

    //Instead of making a bunch of static global variables, I decided to pass this instance in every Form it initiates for simplicity's sake.

    public partial class Menu : Form
    {
        //variable for playing audio.
        public SoundPlayer Player = new SoundPlayer();
        //Stores the name of the currently logged-in player.
        public string player;
        //high_score stores the score of the currently logged-in player, difficulty describes the level of difficulty selected in the main menu.
        public int high_score = 0, difficulty = 0;
        //An array that stores the scores of the leaderboard for in-game calculations.
        private int[] top_40 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //A list that stores the names of the leaderboard for in-game calculations such as updating the board.
        private List<string> top_names = new List<string>();
        //This is a time counter. It is utilized by the two timers used in this form to loop through the 2 audio themes played in the lobby.
        int time = 0;

        public Menu()
        {
            InitializeComponent();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void login_Click(object sender, EventArgs e)
        {
            //LogIn is a Form that prompts the user to sign in with the use of a username and a password. A game cannot start if a login has not been performed.
            //LogIn has access to this instance for changing some of its values.
            new LogIn(this).Show();
            this.Hide();
        }

        private void play_Click(object sender, EventArgs e)
        {
            
            //Sets up the gameplay theme.
            this.Player.Stop();
            try
            {
                time = 0;
                this.Player.SoundLocation = "res/sounds/598762_Boss-X.wav";
                this.Player.PlayLooping();
                timer1.Enabled = false;
                timer2.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error playing sound");
            }
            //Game is the window that runs the actual game. The game has access to this instance.
            new Game(this).Show();
            this.Hide();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            //When the main menu loads, it reads through the leaderboard file to initiate top_40 and top_names.
            try
            {
                StreamReader sr = new StreamReader("res/data/leaderboard.txt");
                string[] arr = sr.ReadLine().Split(',');
                int i = 0;
                foreach (string s in arr)
                {
                    top_40[i] = int.Parse(s);
                    i++;
                }
                arr = sr.ReadLine().Split(',');
                foreach (string s in arr)
                {
                    top_names.Add(s);
                }
                sr.Close();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Something went wrong while trying to read the leaderboard.txt file. The game will now close.");
                this.Close();
            }

            //calls the resetSongLoop method here in order to start the loop.
            resetSongLoop();

        }

        private void leaderboard_Click(object sender, EventArgs e)
        {
            //Form5 is a simple window used to show some of the top leaderboard scores in a fancy manner. It also displays general user information if provided upon signing up.
            new Form5(top_40, top_names).Show();
        }

        private void levels_Click(object sender, EventArgs e)
        {
            //Switches between 3 different difficulty settings upon clicking this button.
            if (difficulty == 0)
            {
                difficulty = 1;
                levels.Text = "DIFFICULTY HARD";
            }
            else if (difficulty == 1)
            {
                difficulty = 2;
                levels.Text = "DIFFICULTY DEMON";
                levels.ForeColor = System.Drawing.Color.DarkRed;
                levels.BackColor = System.Drawing.Color.Black;
            }
            else if (difficulty == 2)
            {
                difficulty = 0;
                levels.Text = "DIFFICULTY EASY";
                levels.ForeColor = System.Drawing.Color.Black;
                levels.BackColor = System.Drawing.Color.WhiteSmoke;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //This label is not visible if a login has been performed.
            MessageBox.Show("You must log in first!!");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //timer1 is enabled when timer2 is done. Once the audio clip started by timer2 is finished, timer1 plays its own assigned audio and resets the time counter for
            //timer2 to do the same set of actions.
            time++;
            //MUSIC//
            if (time >= 97) {
                try
                {
                    this.Player.SoundLocation = "res/sounds/Diced Out.wav";
                    this.Player.Play();
                    time = 0;
                    timer1.Enabled = false;
                    timer2.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error playing sound");
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            time++;
            if (time >= 53)
            {
                try
                {
                    this.Player.SoundLocation = "res/sounds/982338_Stringwave.wav";
                    this.Player.Play();
                    time = 0;
                    timer2.Enabled = false;
                    timer1.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error playing sound");
                }
            }
        }

        //this method takes care of all the file reading-writting needed when a game round ends, such as the player personal high-score and any leaderboard modifications.
        public void set_scores(int score)
        {

            //====SET SCORES====//

            StreamReader scores = new StreamReader("res/data/scores.txt");
            StringBuilder sb = new StringBuilder("");
            List<int> top;
            top = top_40.ToList<int>();

        string s = scores.ReadLine();
            while (s != null)
            {
                if (s.Split('|')[0].Equals(player))
                {
                    if (score > int.Parse(s.Split('|')[1]))
                    {
                        sb.Append(player + "|" + score.ToString());
                        sb.Append(Environment.NewLine);
                        high_score = score;
                    }
                    else
                    {
                        sb.Append(s);
                        sb.Append(Environment.NewLine);
                    }
                }
                else
                {
                    sb.Append(s);
                    sb.Append(Environment.NewLine);
                }
                s = scores.ReadLine();
            }
            scores.Close();
            StreamWriter sw = new StreamWriter("res/data/scores.txt", false);
            sw.Write(sb.ToString());
            sw.Close();

            //====SET LEADERBOARD====//

            int i = top_names.IndexOf(player);
            if (i > -1)
            {
                if (score > top.ElementAt(i))
                {
                    top.RemoveAt(i);
                    top_names.RemoveAt(i);
                    for (int j = 0; j < top.Count(); j++)
                    {
                        if (score > top.ElementAt(j))
                        {
                            top.Insert(j, score);
                            top_names.Insert(j, player);
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < top.Count(); j++)
                {
                    if (score > top.ElementAt(j))
                    {
                        top.RemoveAt(top.Count - 1);
                        top.Insert(j, score);
                        top_names.RemoveAt(top_names.Count - 1);
                        top_names.Insert(j, player);
                        break;
                    }
                }
            }

            sw = new StreamWriter("res/data/leaderboard.txt", false);
            sb = new StringBuilder("");
            for(int j = 0; j < top.Count(); j++)
            {
                sb.Append(top.ElementAt(j).ToString());
                if (j < top.Count() - 1) sb.Append(",");
            }
            sb.Append(Environment.NewLine);
            for (int j = 0; j < top_names.Count(); j++)
            {
                sb.Append(top_names.ElementAt(j).ToString());
                if (j < top_names.Count() - 1) sb.Append(",");
            }
            sw.Write(sb.ToString());
            sw.Close();

            //changes top_40 since all the leaderboard modifications were applied in a list clone.

            for (int j = 0; j < top.Count; j++)
            {
                top_40[j] = top.ElementAt(j);
            }
            
        }
        
        private void about_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Welcome to DICED OUT. This game is all about speed and a little bit of luck! Hit the dice as it teleports around the screen! Each time you catch it" +
                " the number on the face of the dice will be added to your total game score! But beware! Sometimes the dice is modified in one of two possible 'charges'. If the" +
                " dice is positively charged, your score will be doubled but if it is negatively charged, your score will be halved after hitting it!" + Environment.NewLine +
                "DICED OUT comes with three (3) different difficulties - the harder the difficulty, the faster the dice moves accross the screen! However, you shouldn't stick to" +
                " the easiest difficulty, as there is a much better chance to collect a lot of points on harder difficulties thanks to the modifiers! And don't worry about losing" +
                " your score, every completed game will only update your score if it's better than you personal best, that way you can climb up the leaderboards like the wind!!" +
                Environment.NewLine + Environment.NewLine + "Credits for the music used in the game: " + Environment.NewLine + Environment.NewLine + "Lobby themes: " + Environment.NewLine +
                "Name: Diced out / Creator: Vasilis Giata (p19036)" + Environment.NewLine + "Name: Stringwave / Creator: etstringy" + Environment.NewLine + Environment.NewLine +
                "Gameplay theme: " + Environment.NewLine + "Name: Megaman 3 - boss theme / Creator: phoenixdk" + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                "Two of the themes where downloaded from" + " www.newgrounds.com " + Environment.NewLine + "A comunity filled with creators of all kind.");
        }

        //when returning to the main menu from form Game, the audio playing in the lobby needs to be initialized properly. For that, this method is used.
        public void resetSongLoop()
        {
            //MUSIC//
            this.Player.Stop();
            try
            {
                this.Player.SoundLocation = "res/sounds/Diced Out.wav";
                this.Player.Play();
                time = 0;
                timer2.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error playing sound");
            }
        }

    }
}
