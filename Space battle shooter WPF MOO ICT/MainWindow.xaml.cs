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
using System.Windows.Threading;

namespace Space_battle_shooter_WPF_MOO_ICT
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        bool moveLeft, moveRight;
        List<Rectangle> itemRemover = new List<Rectangle>();
        Random rand = new Random();

        private List<string> shipImage;

        int abrantesDmg = 0;
        bool bossfight = false;

        int alliesCounter = 100;
        int enemySpriteCounter = 0;
        int enemyCounter = 100;
        int playerSpeed = 12;
        double limit = 50;
        int score = 0;
        int damage = 0;
        double enemySpeed = 5;
        string playerShipPath;

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
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

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
            bgTransform.Y += 5;

            //Frequencia com que aliados vão aparecer
            alliesCounter -= 1;
            if (alliesCounter < 0 && (bossfight ==false))
            {
                MakeAllies(playerShipPath);
                alliesCounter = rand.Next(200, 500);

            }

            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

            enemyCounter -= 1;

            if (limit > 20)
            {
                limit -= 0.05;
            }

            enemySpeed += 0.005;

            scoreText.Content = "Score: " + score;
            damageText.Content = "Damage " + damage;

            if (enemyCounter < 0 && (bossfight == false))
            {
                MakeEnemies();
                enemyCounter = (int)limit;
            }

            if (moveLeft == true && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }
            if (moveRight == true && Canvas.GetLeft(player) + player.Width < MyCanvas.ActualWidth)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
            }


            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);

                    Rect bulletHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (Canvas.GetTop(x) < 10)
                    {
                        itemRemover.Add(x);
                    }

                    foreach (var y in MyCanvas.Children.OfType<Rectangle>())
                    {
                        if (bossfight)
                        {
                            itemRemover.Add(y);
                        }
                        if (y is Rectangle && (string)y.Tag == "enemy")
                        {
                            Rect enemyHit = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            if (bulletHitBox.IntersectsWith(enemyHit))
                            {
                                itemRemover.Add(x);
                                itemRemover.Add(y);
                                score++;
                            }
                        } else if (y is Rectangle && (string)y.Tag == "allie")
                        {
                            Rect allieHit = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            if (bulletHitBox.IntersectsWith(allieHit))
                            {
                                itemRemover.Add(x);
                                itemRemover.Add(y);
                                damage += 20;
                            }

                        }



                    }

                    //Ativa o modo bossfight
                    if (score > 10)
                    {
                        damage = 0;
                        score = 0;
                        bossfight = true;
                    }

                }

                if (x is Rectangle && (string)x.Tag == "enemy")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + enemySpeed);

                    if (Canvas.GetTop(x) > 750 )
                    {
                        itemRemover.Add(x);
                        damage += 10;
                    }

                    if (bossfight)
                    {
                        itemRemover.Add(x);
                    }

                    Rect enemyHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);


                    if (playerHitBox.IntersectsWith(enemyHitBox))
                    {
                        itemRemover.Add(x);
                        damage += 5;
                    }

                    

                }

                //alliados avançam e reduzem o dano se passarem, mas se baterem comm o player, dão o dobro de dano
                if (x is Rectangle && (string)x.Tag == "allie")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + enemySpeed);

                    if (Canvas.GetTop(x) > 750)
                    {
                        itemRemover.Add(x);

                        damage -= 10;



                       
                        

                    }

                    Rect allieHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);


                    if (playerHitBox.IntersectsWith(allieHitBox))
                    {
                        itemRemover.Add(x);
                        damage += 20;
                    }

                }
            }

            foreach (Rectangle i in itemRemover)
            {
                MyCanvas.Children.Remove(i);
            }

            //Verifica dano das naves
            if (damage > 99 && ((playerShipPath == "/images/player.png") || (playerShipPath == "/images/4.png")))
            {
                gameTimer.Stop();
                damageText.Content = "Damage: 100";
                damageText.Foreground = Brushes.Red;
                MessageBox.Show("Captain You have destroyed " + score + " Alien Ships" + Environment.NewLine + "Press Ok to Play Again", "MOO Says: ");

                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                this.Close();

            }
            else if (damage > 70 && ((playerShipPath == "/images/1.png") || (playerShipPath == "/images/3.png")))
            {
                gameTimer.Stop();
                damageText.Content = "Damage: 100";
                damageText.Foreground = Brushes.Red;
                MessageBox.Show("Captain You have destroyed " + score + " Alien Ships" + Environment.NewLine + "Press Ok to Play Again", "MOO Says: ");

                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                this.Close();


            }
            else if (damage > 120 && ((playerShipPath == "/images/2.png") || (playerShipPath == "/images/5.png")))
            {
                gameTimer.Stop();
                damageText.Content = "Damage: 100";
                damageText.Foreground = Brushes.Red;
                MessageBox.Show("Captain You have destroyed " + score + " Alien Ships" + Environment.NewLine + "Press Ok to Play Again", "MOO Says: ");

                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                this.Close();



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
            gameTimer.Stop();
        }

        private void MakeEnemies()
        {
            ImageBrush enemySprite = new ImageBrush();

            enemySpriteCounter = rand.Next(1, 5);

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
            }

            Rectangle newEnemy = new Rectangle
            {
                Tag = "enemy",
                Height = 50,
                Width = 56,
                Fill = enemySprite
            };

            Canvas.SetTop(newEnemy, -100);
            Canvas.SetLeft(newEnemy, rand.Next(30, 430));
            MyCanvas.Children.Add(newEnemy);

        }
        private void MakeAllies(string playerImagePath)
        {
            ImageBrush allie = new ImageBrush();

            allie.ImageSource = new BitmapImage(new Uri("pack://application:,,," + playerImagePath));
            
            if (playerImagePath == "/images/player.png")
            {
                RotateTransform rotate = new RotateTransform();
                rotate.Angle = 180;
                rotate.CenterX = 0.5;
                rotate.CenterY = 0.5;
                allie.RelativeTransform = rotate;
            }
            

            Rectangle newAllie = new Rectangle
            {
                Tag = "allie",
                Height = 50,
                Width = 56,
                Fill = allie
            };

            Canvas.SetTop(newAllie, -100);
            Canvas.SetLeft(newAllie, rand.Next(30, 430));
            MyCanvas.Children.Add(newAllie);

        }
    }
}
