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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TimeSpan lastRenderTime = TimeSpan.Zero;
        bool moveLeft, moveRight;
        List<Rectangle> itemRemover = new List<Rectangle>();
        Random rand = new Random();

        private List<string> shipImage;

        

        int enemySpriteCounter = 0;
        int enemyCounter = 100;
        int playerSpeed = 10;
        double limit = 50;
        int score = 0;
        int damage = 0;
        double enemySpeed = 5;

        Rect playerHitBox;

        ImageBrush bg = new ImageBrush();
        TranslateTransform bgTransform = new TranslateTransform();

        public MainWindow(string shipImagePath)
        {
            playerShipPath = shipImagePath;
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
                     playerSpeed = 14;
                    
                }

                if (shipImagePath == "/images/2.png")
                {
                    playerSpeed = 10;
                }
                if (shipImagePath == "/images/3.png")
                {
                    playerSpeed = 14;
            }
                if (shipImagePath == "/images/4.png")
                {
                    
                }
                if (shipImagePath == "/images/5.png")
                {
                    playerSpeed = 10;
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

            scoreText.Content = "Score: " + score;
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
                                    score -= 1;
                                }
                                else
                                {
                                    score += 1;
                                }


                            }
                        }
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
                            score += 5;
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
            }

            foreach (Rectangle i in itemRemover)
            {
                MyCanvas.Children.Remove(i);
            }


            if (damage > 99 &&playerShipPath == "/images/player.png")
            {
                CompositionTarget.Rendering -= GameLoop;
                damageText.Content = "Damage: 100";
                damageText.Foreground = Brushes.Red;
                MessageBox.Show("Captain You have destroyed " + score + " Alien Ships" + Environment.NewLine + "Press Ok to Play Again", "MOO Says: ");

                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                this.Close();

            }

            if (score >= 70 && !bossActive)
            {
                bossActive = true;
                damage = 0;
                bossTime = 0;
            }

            if (bossActive)
            {
                if (boss == null)
                {
                    bossTime += 1;
                    if (bossTime >= 150)
                    {
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
                    }

                }
                else if (Canvas.GetTop(boss) < 15)
                {
                    Canvas.SetTop(boss, Canvas.GetTop(boss) + (30 * delta));
                }


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
    }
}