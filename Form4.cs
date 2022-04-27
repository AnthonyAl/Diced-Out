using System;
using System.Drawing;
using System.Windows.Forms;

namespace DiceGame
{
    public partial class Game : Form
    {

        Menu menu;
        //The two spritesheets will be loaded in these bitmaps.
        Bitmap bitmap, modifiers;
        //The end screen textures.
        Bitmap gameOver1, gameOver2;
        //The end screen labels.
        Label l = new Label();
        Label l1 = new Label();
        Random r;
        //t is a time limit depending on the difficulty, luckyChance and unluckyChance are the numbers that will affect how often a modifier appears based on difficulty.
        //endScreen is a small counter activated on the first tick of timer3 that spawns the end screen, so some of the code won't be called repeatedly.
        int t, face = 1, luckyChance, unluckyChance, endScreen = 0;
        //close turns true when the end screen is clicked, clicked turns true when the dice is successfully hit, doubled and halved turn true if a green or red dice are hit.
        bool close, clicked, doubled, halved;
        //The end screen.
        PictureBox p = new PictureBox();

        public Game(Menu menu)
        {
            InitializeComponent();
            this.menu = menu;
            pictureBox1.Controls.Add(dice);
        }

        private void Game_Load(object sender, EventArgs e)
        {
            bitmap = (Bitmap)Bitmap.FromFile("res/textures/dice.png");
            modifiers = (Bitmap)Bitmap.FromFile("res/textures/diceMod.png");
            gameOver1 = (Bitmap)Bitmap.FromFile("res/textures/gameOver.png");
            gameOver2 = (Bitmap)Bitmap.FromFile("res/textures/gameOver2.png");
            r = new Random();

            hight_score.Text = menu.high_score.ToString();

            //sets the chosen difficulty's rules.
            if (menu.difficulty == 0)
            {
                level.Text = "EASY";
                t = 50;
                time.Text = "50";
                timer1.Interval = 1250;
                luckyChance = 15;
                unluckyChance = 10;
            }
            else if (menu.difficulty == 1)
            {
                level.Text = "HARD";
                t = 60;
                time.Text = "60";
                timer1.Interval = 900;
                luckyChance = 15;
                unluckyChance = 10;
            }
            else if (menu.difficulty == 2)
            {
                level.Text = "DEMON";
                pictureBox7.BackColor = System.Drawing.Color.DarkRed;
                level.ForeColor = System.Drawing.Color.Black;
                level.BackColor = System.Drawing.Color.DarkRed;
                t = 70;
                time.Text = "70";
                timer1.Interval = 750;
                luckyChance = 7;
                unluckyChance = 7;
            }
        }

        private void endScreen_Click(object sender, EventArgs e)
        {
            close = true;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            //this timer ticks every second, counting down on the time limit t.
            time.Text = t.ToString();
            if (t > 0) t -= 1;
            else
            {
                timer1.Enabled = false;

                if (endScreen == 0)
                {
                    //Spawns the end screen.
                    p.Location = new Point(0, 0);
                    p.Dock = DockStyle.Fill;
                    p.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox17.Dock = DockStyle.None;
                    pictureBox18.Dock = DockStyle.None;
                    groupBox1.Dock = DockStyle.None;
                    p.Image = gameOver1;
                    this.Controls.Add(p);
                    p.BringToFront();
                    menu.Player.Stop();


                    //spawns a set of labels that show the player's score etc.
                    String s = "  Score: " + score.Text + "  Successful hits: " + successful.Text + "  Failed hits: " + failed.Text + "  Hight Score: " + menu.high_score;
                    l.AutoSize = true;
                    l.BackColor = System.Drawing.Color.Black;
                    l.Font = new System.Drawing.Font("Comic Sans MS", 15.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    l.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                    l.Location = new System.Drawing.Point(p.Width / 2 - s.Length * 6 + s.Length / 5, p.Height * 55 / 100);
                    l.Name = "endScreenInfo";
                    l.TabIndex = 149;
                    l.Text = s;
                    this.Controls.Add(l);
                    l.BringToFront();

                    l1.AutoSize = true;
                    l1.BackColor = System.Drawing.Color.Black;
                    l1.Font = new System.Drawing.Font("Comic Sans MS", 15.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    l1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                    l1.Dock = DockStyle.Top;
                    l1.Name = "endScreenInfo";
                    l1.TabIndex = 149;
                    l1.Text = menu.player;
                    this.Controls.Add(l1);
                    l1.BringToFront();


                    this.MaximumSize = this.Size;
                    this.MinimumSize = this.Size;
                }
                if (endScreen == 1)
                {
                    p.Click += new System.EventHandler(endScreen_Click);
                    l.Click += new System.EventHandler(endScreen_Click);
                    l1.Click += new System.EventHandler(endScreen_Click);
                }
                endScreen++;

                //blinking animation.
                if (p.Image != null)
                {
                    if (p.Image.Equals(gameOver1)) p.Image = gameOver2;
                    else p.Image = gameOver1;
                }
                //closes the game and opens the main menu after the end screen is clicked.
                if (close)
                {
                    timer3.Enabled = false;
                    menu.Show();
                    menu.resetSongLoop();
                    menu.set_scores(int.Parse(score.Text));
                    this.Close();
                }

            }
            if (t < 25 && t >= 10)
            {
                time.ForeColor = System.Drawing.Color.Yellow;
                timer2.Enabled = false;
                time.Visible = true;
            }
            if (t < 10)
            {
                time.ForeColor = System.Drawing.Color.Red;
                timer2.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //this timer teleports the dice around the screen at a different rate depending on difficulty.
            //it also checks if the dice has been hit, and adjusts the score accordingly.
            if (clicked)
            {
                successful.Text = (int.Parse(successful.Text) + 1).ToString();
                if (face == 6)
                {
                    t += 3;
                    face = 1;
                }
                if (!doubled && !halved) score.Text = (int.Parse(score.Text) + face + 1).ToString();
                else score.Text = face.ToString();
            }
            else failed.Text = (int.Parse(failed.Text) + 1).ToString();
            halved = false;
            doubled = false;
            clicked = false;

            face = r.Next(7);

            if (face == 1)
            {
                if (r.Next(luckyChance) == 1)
                {
                    dice.Image = getImage(modifiers, 0);
                    face = int.Parse(score.Text) * 2;
                    doubled = true;
                }
                else if (r.Next(unluckyChance) == 1)
                {
                    dice.Image = getImage(modifiers, 1);
                    face = int.Parse(score.Text) / 2;
                    halved = true;
                }
                else
                {
                    face = 1;
                    dice.Image = getImage(bitmap, face);
                }
            }
            else if (face == 6)
            {
                dice.Image = getImage(modifiers, 2);
            }
            else dice.Image = getImage(bitmap, face);

            int x = r.Next(250, pictureBox1.Width - 110);
            int y = r.Next(10, pictureBox1.Height - 110);
            dice.Location = new Point(x, y);
            failed.Visible = true;

        }

        private void terminate_Click(object sender, EventArgs e)
        {
            menu.Close();
        }

        private void home_Click(object sender, EventArgs e)
        {
            //Useful to be able to quit without losing your current score.
            menu.Show();
            menu.resetSongLoop();
            menu.set_scores(int.Parse(score.Text));
            this.Close();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //this timer makes the time blink when it's running out.
            if (time.Visible == true) time.Hide();
            else time.Show();
        }

        //this method returns a subimage of the provided image. It is used to chose between dice faces in the texture sheets.
        private Bitmap getImage(Bitmap bitmap, int i)
        {
            return bitmap.Clone(new Rectangle(i * 161, 0, 161, 161), bitmap.PixelFormat);
        }

        private void dice_Click(object sender, EventArgs e)
        {
            if (!clicked)
            {

                clicked = true;
            }
        }
    }
}
