using Microsoft.VisualBasic.Devices;
using System.Numerics;
using static System.Formats.Asn1.AsnWriter;

namespace SimplePongGame
{
    public partial class Form1 : Form
    {

        int ballXspeed = 4;
        int ballYspeed = 4;
        //int speed = 2;
        Random rand = new Random();
        bool goDown, goUp;
        int computer_speed_change = 50;
        int computerScore = 0;
        int playerSpeed = 8;
        int[] i = { 5, 6, 8, 9 };
        int[] j = { 10, 9, 8, 11, 12 };

        bool goup; 
        bool godown;
        bool P2up;
        bool P2down;
        int speed = 5; 
        double ballx = 5;
        int bally = 6; 
        int score = 0; 
        int cpuPoint = 0;
        bool players;

        public Form1(bool players)
        {
            InitializeComponent();
            this.players = players;
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                goup = true;
            }
            if (e.KeyCode == Keys.S)
            {
                godown = true;
            }

            if (players)
            {
                if (e.KeyCode == Keys.Up)
                {
                    P2up = true;
                }
                if (e.KeyCode == Keys.Down)
                {
                    P2down = true;
                }
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                goup = false;
            }
            if (e.KeyCode == Keys.S)
            {
                godown = false;
            }

            if (players)
            {
                if (e.KeyCode == Keys.Up)
                {
                    P2up = false;
                }
                if (e.KeyCode == Keys.Down)
                {
                    P2down = false;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            playerScore.Text = "" + score; 
            cpuLabel.Text = "" + cpuPoint;
            ball.Top -= bally;
            ball.Left -= (int)ballx;
            if (!players)
            {
                cpu.Top += speed;
            }

            if (ballx > 0)
            {
                ballx += 0.05;
            }
            else
            {
                ballx -= 0.05;
            }



            if (score < 5)
            {
                if (cpu.Top < 0 || cpu.Top > 455)
                {
                    speed = -speed;
                }
            }
            else
            {
                if (ball.Top - 40 > cpu.Top && speed < 0)
                {
                    speed = -speed;
                } else if (ball.Top - 40 < cpu.Top && speed > 0)
                {
                    speed = -speed;
                }
            }
            if (ball.Left < -15)
            {

                ball.Left = 434;
                ball.Top = ClientSize.Height / 2;
                ballx = -ballx;
                ball.BackColor = Color.Gray;
                if (ballx > 0)
                {
                    ballx = 5;
                }
                else
                {
                    ballx = -5;
                }

                cpuPoint++;
                if (score > 5)
                {
                    cpu.Top = (ClientSize.Height / 2) - 40;
                }
            }
   
            if (ball.Left + ball.Width > ClientSize.Width + 15)
            {
                // then
                ball.Left = 434;
                ball.Top = ClientSize.Height / 2;
                ballx = -ballx;
                ball.BackColor = Color.Gray;
                if (ballx > 0)
                {
                    ballx = 5;
                } else {
                    ballx = -5;
                }

                score++;
                if (score > 5)
                {
                    cpu.Top = (ClientSize.Height / 2) - 40;
                }
            }


            if (ball.Top < 0 || ball.Top + ball.Height > ClientSize.Height)
            {
                bally = -bally;
            }

            if (ball.Bounds.IntersectsWith(player.Bounds))
            {
                ballx = -ballx;
                ball.BackColor = Color.White;
            }
            else if (ball.Bounds.IntersectsWith(cpu.Bounds))
            {
                ballx = -ballx;
                ball.BackColor = Color.Black;
            }

            if (goup == true && player.Top > 0)
            {
                player.Top -= 8;
            }

            if (godown == true && player.Top < 455)
            {
                player.Top += 8;
            }

            if (players)
            {
                if (P2up == true && cpu.Top > 0)
                {
                    cpu.Top -= 8;
                }

                if (P2down == true && cpu.Top < 455)
                {
                    cpu.Top += 8;
                }
            }
            


            if (score >= 10)
            {
                gameTimer.Stop();
                if (players)
                {
                    MessageBox.Show("Player 1 win this game!");
                } else {
                    MessageBox.Show("You win this game :)");
                }
                this.Close();
            }
            if (cpuPoint >= 10)
            {
                gameTimer.Stop();
                if (players)
                {
                    MessageBox.Show("Player 2 win this game!");
                } else {
                    MessageBox.Show("CPU wins, you lose :(");
                }
                this.Close();
            }
        }

        }
    }
