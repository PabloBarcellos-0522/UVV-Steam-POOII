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
        int speed = 5; 
        int ballx = 5; 
        int bally = 5; 
        int score = 0; 
        int cpuPoint = 0; 

        public Form1()
        {
            InitializeComponent();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                goup = true;
            }
            if (e.KeyCode == Keys.Down)
            {
                godown = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                goup = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                godown = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            playerScore.Text = "" + score; 
            cpuLabel.Text = "" + cpuPoint;
            ball.Top -= bally;
            ball.Left -= ballx; 
            cpu.Top += speed; 

            if (score < 5)
            {
                if (cpu.Top < 0 || cpu.Top > 455)
                {
                    speed = -speed;
                }
            }
            else
            {
                cpu.Top = ball.Top + 30;
            }
            if (ball.Left < 0)
            {

                ball.Left = 434; 
                ballx = -ballx; 
                ballx -= 2; 
                cpuPoint++; 
            }
   
            if (ball.Left + ball.Width > ClientSize.Width)
            {
                // then
                ball.Left = 434;  
                ballx = -ballx; 
                ballx += 2; 
                score++; 
            }


            if (ball.Top < 0 || ball.Top + ball.Height > ClientSize.Height)
            {
                bally = -bally;
            }

            if (ball.Bounds.IntersectsWith(player.Bounds) || ball.Bounds.IntersectsWith(cpu.Bounds))
            {
                ballx = -ballx;
            }

            if (goup == true && player.Top > 0)
            {
                player.Top -= 8;
            }

            if (godown == true && player.Top < 455)
            {
                player.Top += 8;
            }
            if (score > 10)
            {
                gameTimer.Stop();
                MessageBox.Show("You win this game");
            }
            if (cpuPoint > 10)
            {
                gameTimer.Stop();
                MessageBox.Show("CPU wins, you lose");
            }
        }

        }
    }
