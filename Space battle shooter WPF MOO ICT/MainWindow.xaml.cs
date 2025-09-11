using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Space_battle_shooter_WPF_MOO_ICT
{
    public partial class MainWindow : Window
    {
        private TimeSpan lastRenderTime = TimeSpan.Zero;
        bool moveLeft, moveRight;
        List<Rectangle> itemRemover = new List<Rectangle>();

        Random rand = new Random();

        Rect bossHitBox;
        private List<string> shipImage;
        int enemySpriteCounter = 0;
        double enemySpawnCoolDown = 3.3;
        int playerSpeed = 350;
        double limit = 50;
        int score = 0;
        int damage = 0;
        double enemySpeed = 150;
        Rectangle? boss = null;
        Boolean bossActive = false;
        int bossTime = 0;
        int bossLife = 300;
        int cooldownBossAttacks = 0;
        bool avanco = false;
        double playerPositionY;
        double playerPositionX;

        //Duração dos ataques do boss
        int bossAttackDuration;

        Rect playerHitBox;

        ImageBrush bg = new ImageBrush();
        TranslateTransform bgTransform = new TranslateTransform();

        public MainWindow(string shipImagePath)
        {
            InitializeComponent();
            InitializeGame("pack://application:,,," + shipImagePath);
            shipImage = new List<string>
            {
                "/images/player.png",
                "/images/1.png",
                "/images/2.png",
                "/images/3.png",
                "/images/4.png",
                "/images/5.png"
            };


            if (shipImagePath == "/images/1.png")
            {
                playerSpeed = 400;
            }

            if (shipImagePath == "/images/2.png")
            {
                playerSpeed = 340;
            }
            if (shipImagePath == "/images/3.png")
            {
                playerSpeed = 400;
            }

            if (shipImagePath == "/images/5.png")
            {
                playerSpeed = 340;
            }

        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame("pack://application:,,,/images/player.png");
        }

        private void InitializeGame(string playerImageUri)
        {
            CompositionTarget.Rendering += GameLoop;

            MyCanvas.Focus();

            BitmapImage bgImg = new BitmapImage();
            bgImg.BeginInit();
            bgImg.UriSource = new Uri("pack://application:,,,/images/starsBG.png");
            bgImg.CacheOption = BitmapCacheOption.OnLoad;
            bgImg.EndInit();
            bg.ImageSource = bgImg;
            bg.TileMode = TileMode.Tile;
            bg.Transform = bgTransform;
            MyCanvas.Background = bg;


            ImageBrush playerImage = new ImageBrush();
            if (playerImageUri != "pack://application:,,,/images/player.png")
            {
                RotateTransform rotate = new RotateTransform();
                rotate.Angle = 180;
                rotate.CenterX = 0.5;
                rotate.CenterY = 0.5;
                playerImage.RelativeTransform = rotate;
            }

            playerImage.ImageSource = new BitmapImage(new Uri(playerImageUri));
            player.Fill = playerImage;
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (!(e is RenderingEventArgs args)) return;
            if (lastRenderTime == TimeSpan.Zero)
            {
                lastRenderTime = args.RenderingTime;
                return;
            }

            if (lastRenderTime == args.RenderingTime) return;

            double delta = (args.RenderingTime - lastRenderTime).TotalSeconds;
            lastRenderTime = args.RenderingTime;

            bgTransform.Y += 150 * delta;

            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

            enemySpawnCoolDown -= delta;

            if (limit > 20)
            {
                limit -= 1.5 * delta;
            }

            enemySpeed += 4.5 * delta;

            if (!bossActive)
            {
                scoreText.Content = "Score: " + score;
            }
            damageText.Content = "Damage: " + damage;

            if (enemySpawnCoolDown < 0 && !bossActive)
            {
                MakeEnemies();
                enemySpawnCoolDown = limit / 30.0;
            }

            if (moveLeft == true && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed * delta);
            }
            if (moveRight == true && Canvas.GetLeft(player) + player.Width < MyCanvas.ActualWidth)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed * delta);
            }


            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - (600 * delta));

                    Rect bulletHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (Canvas.GetTop(x) < 10)
                    {
                        itemRemover.Add(x);
                    }

                    foreach (var y in MyCanvas.Children.OfType<Rectangle>())
                    {
                        if (y is Rectangle && (string)y.Tag == "enemy")
                        {
                            Rect enemyHit = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            if (bulletHitBox.IntersectsWith(enemyHit))
                            {
                                string playerUriString = "";
                                if (player.Fill is ImageBrush playerBrush && playerBrush.ImageSource is BitmapImage playerBitmap)
                                {
                                    playerUriString = playerBitmap.UriSource.ToString();
                                }

                                string enemyUriString = "";
                                if (y.Fill is ImageBrush enemyBrush && enemyBrush.ImageSource is BitmapImage enemyBitmap)
                                {
                                    enemyUriString = enemyBitmap.UriSource.ToString();
                                }


                                itemRemover.Add(x);
                                itemRemover.Add(y);
                                if (playerUriString == enemyUriString)
                                {
                                    score -= 3;
                                }
                                else
                                {
                                    score += 1;
                                }


                            }


                        }


                    }
                    if (boss != null)
                    {
                        bossHitBox = new Rect(Canvas.GetLeft(boss), Canvas.GetTop(boss), boss.Width, boss.Height);
                        string playerUriString = "";
                        if (player.Fill is ImageBrush playerBrush && playerBrush.ImageSource is BitmapImage playerBitmap)
                        {
                            playerUriString = playerBitmap.UriSource.ToString();

                        }
                    }

                    
                    if (bulletHitBox.IntersectsWith(bossHitBox))
                    {
                        itemRemover.Add(x);
                        if ((string)scoreText.Content == "Life: 0")
                        {
                            itemRemover.Add(boss);
                        }
                        else
                        {
                            bossLife -= 1;
                            scoreText.Content = "Life: " + bossLife;
                        }
                    }

                }
                //Boss laser projectile
                if (x is Rectangle && (string)x.Tag == "laser")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + enemySpeed * delta);
                    
                    string playerUriString = "";

                    if (Canvas.GetTop(x) > 750)
                    {
                        itemRemover.Add(x);
                        
                    }
                    Rect laserHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (playerHitBox.IntersectsWith(laserHitBox))
                    {

                        
                        damage += 1;
                    }


                }

                    if (x is Rectangle && (string)x.Tag == "enemy")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + enemySpeed * delta);

                    string playerUriString = "";
                    if (player.Fill is ImageBrush playerBrush && playerBrush.ImageSource is BitmapImage playerBitmap)
                    {
                        playerUriString = playerBitmap.UriSource.ToString();
                    }

                    string enemyUriString = "";
                    if (x.Fill is ImageBrush enemyBrush && enemyBrush.ImageSource is BitmapImage enemyBitmap)
                    {
                        enemyUriString = enemyBitmap.UriSource.ToString();
                    }

                    if (Canvas.GetTop(x) > 750)
                    {
                        itemRemover.Add(x);
                        if (playerUriString == enemyUriString)
                        {
                            score += 3;
                        }
                        else
                        {
                            damage += 10;
                        }
                    }

                    Rect enemyHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (playerHitBox.IntersectsWith(enemyHitBox))
                    {
                        itemRemover.Add(x);
                        if (playerUriString == enemyUriString)
                        {
                            score -= 1;
                        }
                        damage += 5;
                    }

                }

                if (x is Rectangle && (string)x.Tag == "bossBullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + (300 * delta));

                    if (Canvas.GetTop(x) > 750)
                    {
                        itemRemover.Add(x);
                    }

                    Rect bossBulletHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (playerHitBox.IntersectsWith(bossBulletHitBox))
                    {
                        itemRemover.Add(x);
                        damage += 10;
                    }
                }
            }

            foreach (Rectangle i in itemRemover)
            {
                MyCanvas.Children.Remove(i);
            }


            if (damage > 99)
            {
                CompositionTarget.Rendering -= GameLoop;
                damageText.Content = "Damage: 100";
                damageText.Foreground = Brushes.Red;
                MessageBox.Show("Captain You have destroyed " + score + " Alien Ships" + Environment.NewLine + "Press Ok to Play Again", "MOO Says: ");

                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                this.Close();

            }

            if (score >= 1 && !bossActive)
            {
                bossActive = true;
                damage = 0;
                bossTime = 0;
            }

            if (bossActive)
            {
                bossTime += 1;
                if (boss == null)
                {
                    if (bossTime >= 150)
                    {
                        scoreText.Content = "Life: " + bossLife;
                        ImageBrush bossImg = new ImageBrush();
                        bossImg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Abrantes.png"));

                        boss = new Rectangle
                        {
                            Tag = "boss",
                            Height = 250,
                            Width = 300,
                            Fill = bossImg
                        };


                        Canvas.SetTop(boss, -300);
                        Canvas.SetLeft(boss, (MyCanvas.ActualWidth - boss.Width) / 2);
                        MyCanvas.Children.Add(boss);
                        bossHitBox = new Rect(Canvas.GetLeft(boss), Canvas.GetTop(boss), boss.Width, boss.Height);
                    }

                }
                else if (Canvas.GetTop(boss) < 15)
                {
                    Canvas.SetTop(boss, Canvas.GetTop(boss) + (30 * delta));
                }
                else if (avanco)
                {
                    double bossX = Canvas.GetLeft(boss);
                    double bossY = Canvas.GetTop(boss);

                    double directionX = Math.Sign(playerPositionX - bossX);
                    double directionY = Math.Sign(playerPositionY - bossY);

                    double moveSpeed = 150 * delta; // velocidade do avanço

                    // Movimento em X
                    if (Math.Abs(playerPositionX - bossX) > 5)
                    {
                        Canvas.SetLeft(boss, bossX + directionX * moveSpeed);
                    }

                    // Movimento em Y
                    if (Math.Abs(playerPositionY - bossY) > 5)
                    {
                        Canvas.SetTop(boss, bossY + directionY * moveSpeed);
                    }

                    // Se chegou perto o suficiente da posição do player
                    if (Math.Abs(playerPositionX - bossX) <= 5 && Math.Abs(playerPositionY - bossY) <= 5)
                    {
                        avanco = false; // Parar avanço
                    }
                }

            }

            cooldownBossAttacks += 1;
            if (cooldownBossAttacks % 1000 == 0 && bossActive && boss != null && Canvas.GetTop(boss) > 14)
            {
                cooldownBossAttacks = 0;
                int attack = rand.Next(0, 3);
                AtaquesBoss(attack);
            }

        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.A)
            {
                moveLeft = true;
            }
            if (e.Key == Key.Right || e.Key == Key.D)
            {
                moveRight = true;
            }

            if (e.Key == Key.Space)
            {
                Rectangle newBullet = new Rectangle
                {
                    Tag = "bullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Red

                };

                Canvas.SetLeft(newBullet, Canvas.GetLeft(player) + player.Width / 2);
                Canvas.SetTop(newBullet, Canvas.GetTop(player) - newBullet.Height);

                MyCanvas.Children.Add(newBullet);

            }
        }


        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.A)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right || e.Key == Key.D)
            {
                moveRight = false;
            }


        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CompositionTarget.Rendering -= GameLoop;
        }

        private void MakeEnemies()
        {
            ImageBrush enemySprite = new ImageBrush();

            enemySpriteCounter = rand.Next(1, 7);

            switch (enemySpriteCounter)
            {
                case 1:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/1.png"));
                    break;
                case 2:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/2.png"));
                    break;
                case 3:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/3.png"));
                    break;
                case 4:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/4.png"));
                    break;
                case 5:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/5.png"));
                    break;
                case 6:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/player.png"));
                    RotateTransform rotate = new RotateTransform(180, 0.5, 0.5);
                    enemySprite.RelativeTransform = rotate;

                    break;
            }

            Rectangle newEnemy = new Rectangle
            {
                Tag = "enemy",
                Height = 50,
                Width = 56,
                Fill = enemySprite
            };

            string playerUriString = "";
            if (player.Fill is ImageBrush playerBrush && playerBrush.ImageSource is BitmapImage playerBitmap)
            {
                playerUriString = playerBitmap.UriSource.ToString();
            }

            string enemyUriString = "";
            if (newEnemy.Fill is ImageBrush enemyBrush && enemyBrush.ImageSource is BitmapImage enemyBitmap)
            {
                enemyUriString = enemyBitmap.UriSource.ToString();
            }

            var shadowEffect = new System.Windows.Media.Effects.DropShadowEffect
            {
                ShadowDepth = 0,
                BlurRadius = 30
            };

            if (playerUriString == enemyUriString)
            {
                shadowEffect.Color = Colors.LawnGreen;
            }
            else
            {
                shadowEffect.Color = Colors.Red;
            }

            newEnemy.Effect = shadowEffect;

            Canvas.SetTop(newEnemy, -100);
            Canvas.SetLeft(newEnemy, rand.Next(30, 430));
            MyCanvas.Children.Add(newEnemy);

        }

        public void AtaquesBoss(int ataque)
        {

            switch (ataque)
            {
                case 0:
                    //Ataque de ballas pela boca
                    Ataque0();
                    break;
                case 1:
                    //Ataque de Lasers pela boca

                    Rectangle newLaser = new Rectangle
                    {
                        Tag = "laser", 
                        Height = 25,
                        Width = 50,
                        Fill = Brushes.White,
                        Stroke = Brushes.Red
                    };

                    Canvas.SetLeft(newLaser, Canvas.GetLeft(boss) + boss.Width / 2 - newLaser.Width / 2);
                    Canvas.SetTop(newLaser, Canvas.GetTop(boss) + boss.Height - 10); // aparece na "boca" do boss

                    MyCanvas.Children.Add(newLaser);


                    break;
                case 2:
                    //Ataque Avanço letal
                    avanco = true;
                    playerPositionY = Canvas.GetTop(player);
                    playerPositionX = Canvas.GetLeft(player);
                    break;
            }

        }

        private async void Ataque0()
        {
            int duracaoTotal = 5000; // 5 segundos
            int intervalo = 500;     // 0.5 segundo
            int disparos = duracaoTotal / intervalo;

            for (int i = 0; i < disparos; i++)
            {
                Rectangle bossBullet = new Rectangle
                {
                    Tag = "bossBullet",
                    Height = 20,
                    Width = 20,
                    Fill = Brushes.Yellow,
                    Stroke = Brushes.Orange,
                    RadiusX = 10,
                    RadiusY = 10
                };

                if (boss != null)
                {
                    Canvas.SetLeft(bossBullet, Canvas.GetLeft(boss) + (boss.Width / 2) - (bossBullet.Width / 2));
                    Canvas.SetTop(bossBullet, Canvas.GetTop(boss) + boss.Height - 40);

                    MyCanvas.Children.Add(bossBullet);
                }
                await Task.Delay(intervalo);
            }
        }

    }
}
