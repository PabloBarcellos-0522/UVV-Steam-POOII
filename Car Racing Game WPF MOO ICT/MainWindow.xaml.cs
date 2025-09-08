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

namespace Car_Racing_Game_WPF_MOO_ICT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer(); // create a new instance of the dispatcher time called gameTimer
        List<Rectangle> itemRemover = new List<Rectangle>(); // make a new list called item remove, this list will be used to remove any unused rectangles in the game 

        Random rand = new Random(); // make a new instance of the random class called rand

        ImageBrush playerImage = new ImageBrush(); // create a new image brush for the player
        ImageBrush starImage = new ImageBrush(); // create a new image brush for the star
        ImageBrush iceImage = new ImageBrush(); // create a new image brush for the star
        ImageBrush MultiplierImage= new ImageBrush();

        Rect playerHitBox; // this rect object will be used to calculate the player hit area with other objects

        // set the game integers including, speed for the traffic and road markings, player speed, car numbers, star counter and power mode counter
        int speed = 15;
        int playerSpeed = 10;
        int carNum;
        int PowerUpCounter = 30;
        int powerModeCounter = 200;
        

        // create 5 doubles one for time and other called i, this one will be used to animate the player car when we reach the power mode
        double time;
        double i;
        double score;
        
        double crashed;// carros destruidos no modo power up
        double nearMiss;// conta os quase acidentes, passar perto das "linhas "
        
        // we will need 4 boolean altogether for this game, since all of them will be false at the start we are defining them in one line. 
        bool moveLeft, moveRight,slowTime, gameOver, StarMode,IceMode,MultiplierMode;


        public MainWindow()
        {
            InitializeComponent();

            myCanvas.Focus(); // set the main focus of the program to the my canvas element, with this line it wont register the keyboard events

            gameTimer.Tick += GameLoop; // link the game timer event to the game loop event
            gameTimer.Interval = TimeSpan.FromMilliseconds(20); // this timer will run every 20 milliseconds

            StartGame(); // run the start game function
        }

        private void GameLoop(object sender, EventArgs e)
        {
            
            time += .05; // increase the score by .5 each tick of the timer
            score += 0.05; 
            score += crashed;
            score += nearMiss;
            crashed = 0;
            nearMiss = 0;

            PowerUpCounter -= 1; // reduce 1 from the star counter each tick

            //Alteração para sistema de pontuação
            scoreText.Content = "Pontuação: " + score.ToString("#.#") + " "; // this line will show the seconds passed in decimal numbers in the score text label

            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height); // assign the player hit box to the player

            // below are two if statements that are checking the player can move or right in the scene. 
            if (moveLeft == true && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
                //Movimento das bordas do near miss para esquerda
                Canvas.SetLeft(BorderNearLeft, Canvas.GetLeft(BorderNearLeft) - playerSpeed);
                Canvas.SetLeft(BorderNearRight, Canvas.GetLeft(BorderNearRight) - playerSpeed);
            }
            if (moveRight == true && Canvas.GetLeft(player) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
                //Movimento das bordas do near miss para Direita
                Canvas.SetLeft(BorderNearLeft, Canvas.GetLeft(BorderNearLeft) + playerSpeed);
                Canvas.SetLeft(BorderNearRight, Canvas.GetLeft(BorderNearRight) + playerSpeed);
            }




            // if the star counter integer goes below 1 then we run the make star function and also generate a random number inside of the star counter integer
            if (PowerUpCounter < 1)
            {
                MakePower(rand.Next(1,4));
                 
                PowerUpCounter = rand.Next(600, 900);
            }

            // below is the main game loop, inside of this loop we will go through all of the rectangles available in this game
            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {
                // first we search through all of the rectangles in this game

                // then we check if any of the rectangles has a tag called road marks
                if ((string)x.Tag == "roadMarks")
                {
                    // if we find any of the rectangles with the road marks tag on it then 

                    Canvas.SetTop(x, Canvas.GetTop(x) + speed); // move it down using the speed variable

                    // if the road marks goes below the screen then move it back up top of the screen
                    if (Canvas.GetTop(x) > 510)
                    {
                        Canvas.SetTop(x, -152);
                    }

                } // end of the road marks if statement

                // if we find a rectangle with the car tag on it
                if ((string)x.Tag == "Car")
                {
                    if ((IceMode) && (slowTime)) {
                        Canvas.SetTop(x, Canvas.GetTop(x) + 6); //Aplicação do poder de diminuir o tempo
                    }
                    else
                    {
                        Canvas.SetTop(x, Canvas.GetTop(x) + speed); // move the rectangle down using the speed variable
                    }
                        

                    // if the car has left the scene then run then run the change cars function with the current x rectangle inside of it
                    if (Canvas.GetTop(x) > 500)
                    {
                        ChangeCars(x, false);
                    }

                    // create a new rect called car hit box and assign it to the x which is the cars rectangle
                    Rect carHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    // if the player hit box and the car hit box collide and the power mode is ON
                    if (playerHitBox.IntersectsWith(carHitBox) && StarMode == true)
                    {
                        // run the change cars function with the cars rectangle X inside of it
                        ChangeCars(x, true);
                        //Pontos de veiculo destruido
                        crashed += 1;
                    }
                    else if (playerHitBox.IntersectsWith(carHitBox) && StarMode == false)
                    {
                        // if the power is OFF and car and the player collide then

                        //Explode o carro do jogador. WPF não aceita trocar a imagem denovo, por isso criar um novo objeto
                        ImageBrush explosionBrush = new ImageBrush();
                        explosionBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/explosao.png"));
                        player.Fill = explosionBrush;

                        gameTimer.Stop(); // stop the game timer
                        scoreText.Content += " Press Enter to replay"; // add this text to the existing text on the label
                        gameOver = true; // set game over boolean to true
                    }
                    // Implementação mecanica Near miss
                    Rect leftBorder = new Rect(Canvas.GetLeft(BorderNearLeft), Canvas.GetTop(BorderNearLeft), BorderNearLeft.Width, BorderNearLeft.Height);
                    Rect rightBorder = new Rect(Canvas.GetLeft(BorderNearRight), Canvas.GetTop(BorderNearRight), BorderNearRight.Width, BorderNearRight.Height);

                    //vai incrementar near miss se o jogador estiver fora da invencibilidade e as "bordas" passarem na hitbox de outro carro
                    if ((leftBorder.IntersectsWith(carHitBox) || rightBorder.IntersectsWith(carHitBox))&& StarMode == false)
                    {
                        nearMiss += 0.5;
                    }


                } // end of car if statement



                // if we find a rectangle with the star tag on it
                if ((string)x.Tag == "star" )
                {
                    // move it down the screen 5 pixels at a time
                    Canvas.SetTop(x, Canvas.GetTop(x) + 5);

                    // create a new rect with for the star and pass in the star X values inside of it
                    Rect starHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    


                    // if the player and the star
                    if (playerHitBox.IntersectsWith(starHitBox))
                    {
                        // add the star to the item remover list
                        itemRemover.Add(x);

                        // set power mode to true
                        StarMode = true;

                        // set power mode counter to 200
                        powerModeCounter = 200;

                    }
                    
                    

                    // if the star goes beyon 400 pixels then add it to the item remover list
                    if (Canvas.GetTop(x) > 400)
                    {
                        itemRemover.Add(x);
                    }

                } // end of start if statement


                if ( (string)x.Tag == "ice")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 5);
                    Rect iceHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    // if the player and the star, or ice collide then
                    if (playerHitBox.IntersectsWith(iceHitBox))
                    {
                        
                        // add the star to the item remover list
                        itemRemover.Add(x);

                        // set power mode to true
                        IceMode = true;

                        // set power mode counter to 200
                        powerModeCounter = 200;
                        // if the star goes beyon 400 pixels then add it to the item remover list
                        if (Canvas.GetTop(x) > 400)
                        {
                            itemRemover.Add(x);
                        }

                    }
                }

                if ((string)x.Tag == "multi")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 5);
                    Rect multiHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    // if the player and the star, or ice collide then
                    if (playerHitBox.IntersectsWith(multiHitBox))
                    {

                        // add the star to the item remover list
                        itemRemover.Add(x);

                        // set power mode to true
                        MultiplierMode = true;

                        // set power mode counter to 200
                        powerModeCounter = 200;
                        // if the star goes beyon 400 pixels then add it to the item remover list
                        if (Canvas.GetTop(x) > 400)
                        {
                            itemRemover.Add(x);
                        }

                    }
                }



            } // end of for each loop

            
            
            
            // if the power mode is true
            if (StarMode == true)
            {
                powerModeCounter -= 1; // reduce 1 from the power mode counter 
                // run the power up function
                PowerStar(powerModeCounter);
                

                
                // if the power mode counter goes below 1 
                if (powerModeCounter < 1)
                {
                    // set power mode to false
                    StarMode = false;
                    playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerImage.png"));
                    myCanvas.Background = Brushes.Gray;
                }
            }
            

            if ( IceMode)
            {
                SlowPower(powerModeCounter);
                if (slowTime)
                {
                    powerModeCounter -= 1; // reduce 1 from the power mode counter 
                                           
                    



                    // if the power mode counter goes below 1 
                    if (powerModeCounter < 1)
                    {
                        // set power mode to false
                        IceMode = false;
                        // if the mode is false then change the player car back to default and also set the background to gray
                        playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerImage.png"));
                        myCanvas.Background = Brushes.Gray;
                    }
                }
                
            }
            

            // each item we find inside of the item remove we will remove it from the canvas
            foreach (Rectangle y in itemRemover)
            {
                myCanvas.Children.Remove(y);
            }

            if (MultiplierMode == true)
            {
                powerModeCounter -= 1; // reduce 1 from the power mode counter 
                // run the power up function
                MultiplierPower(powerModeCounter);



                // if the power mode counter goes below 1 
                if (powerModeCounter < 1)
                {
                    // set power mode to false
                    score *= 2;
                    MultiplierMode = false;
                    playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerImage.png"));
                    myCanvas.Background = Brushes.Gray;
                }
            }
            


            // below are the score and speed configurations for the game
            // as you progress in the game you will score higher and traffic speed will go up



            if (time >= 15 && time < 25)
            {
                speed = 12;
            }

            if (time >= 30 && time < 50)
            {
                speed = 15;
            }
            if (time >= 30 && time < 40)
            {
                speed = 16;
            }
            if (time >= 40 && time < 50)
            {
                speed = 18;
            }
            if (time >= 50 && time < 80)
            {
                speed = 20;
            }

           

        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // key down function will listen for you the user to press the left or right key and it will change the designated boolean to true

            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }
            if (e.Key == Key.Right)
            {
                moveRight = true;
            }

            if (e.Key == Key.Space)
            {
                
                slowTime = true;
            }
        }

        private void OnKeyUP(object sender, KeyEventArgs e)
        {
            // when the player releases the left or right key it will set the designated boolean to false

            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }

            if (e.Key == Key.Space)
            {
                slowTime = false;
            }
            // in this case we will listen for the enter key aswell but for this to execute we will need the game over boolean to be true
            if (e.Key == Key.Enter && gameOver == true)
            {
                // if both of these conditions are true then we will run the start game function
                StartGame();
            }
        }

        private void StartGame()
        {
            
            // thi sis the start game function, this function to reset all of the values back to their default state and start the game

            speed = 8; // set speed to 8
            gameTimer.Start(); // start the timer

            // set all of the boolean to false
            moveLeft = false;
            moveRight = false;
            slowTime = false;

            gameOver = false;
            StarMode = false;
            IceMode =false;
            MultiplierMode = false;


            BorderNearRight.Visibility = Visibility.Collapsed;
            BorderNearLeft.Visibility = Visibility.Collapsed;

            // set score to 0
            time = 0;
            score = 0;
            nearMiss = 0;
            crashed = 0;
            // set the score text to its default content
            scoreText.Content = "Survived: 0 Seconds";
            // set up the player image and the star image from the images folder
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerImage.png"));
            starImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/star.png"));
            iceImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/ice.png"));
            MultiplierImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/2x.png"));
            // assign the player image to the player rectangle from the canvas
            player.Fill = playerImage;
            // set the default background colour to gray
            myCanvas.Background = Brushes.Gray;

            // run a initial foreach loop to set up the cars and remove any star in the game

            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {
                // if we find any rectangle with the car tag on it then we will
                if ((string)x.Tag == "Car")
                {
                    
                    
                    // set a random location to their top and left position
                    Canvas.SetTop(x, (rand.Next(100, 400) * -1));
                    Canvas.SetLeft(x, rand.Next(0, 430));
                    // run the change cars function
                    ChangeCars(x,false);
                }

                // if we find a star in the beginning of the game then we will add it to the item remove list
                if ((string)x.Tag == "star" || (string)x.Tag == "ice"|| (string)x.Tag == "multi")
                {
                    itemRemover.Add(x);
                }

            }
            // clear any items inside of the item remover list at the start
            itemRemover.Clear();
        }

        private void ChangeCars(Rectangle car, bool explosao)
        {
            if (explosao)
            {
                //muda a imagem do carro para uma explosão
                ImageBrush carImage = new ImageBrush(); // create a new image brush for the car image 
                carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/explosao.png"));
                car.Fill = carImage; // assign the chosen car image to the car rectangle


            }
            else
            {
                // we want the game to change the traffic car images as they leave the scene and come back to it again

                carNum = rand.Next(1, 6); // to start lets generate a random number between 1 and 6

                ImageBrush carImage = new ImageBrush(); // create a new image brush for the car image 

                // the switch statement below will see what number have generated for the car num integer and 
                // based on that number it will assign a different image to the car rectangle
                switch (carNum)
                {
                    case 1:
                        carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car1.png"));
                        break;
                    case 2:
                        carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car2.png"));
                        break;
                    case 3:
                        carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car3.png"));
                        break;
                    case 4:
                        carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car4.png"));
                        break;
                    case 5:
                        carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car5.png"));
                        break;
                    case 6:
                        carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car6.png"));
                        break;
                }

                car.Fill = carImage; // assign the chosen car image to the car rectangle

                // set a random top and left position for the traffic car
                do
                {
                    Canvas.SetTop(car, (rand.Next(100, 400) * -1));
                    Canvas.SetLeft(car, rand.Next(0, 430));

                } while (UsedPosition(car));
                    
                


            }

            
        }

        private void PowerStar(int powerModeCounter)
        {
            //Carro volta ao tamanho normal indicando que o tempo do power Up está terminando
            if (powerModeCounter < 40)
            {
                playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerImage.png"));
                myCanvas.Background = Brushes.LightCoral;

            }
            else
            {
                // this is the power up function, this function will run when the player collects the star in the game

                i += .5; // increase i by .5 


                // if i is greater than 4 then reset i back to 1
                if (i > 4)
                {
                    i = 1;
                }

                // with each increment of the i we will change the player image to one of the 4 images below

                switch (i)
                {
                    case 1:
                        playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/powermode1.png"));
                        break;
                    case 2:
                        playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/powermode2.png"));
                        break;
                    case 3:
                        playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/powermode3.png"));
                        break;
                    case 4:
                        playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/powermode4.png"));
                        break;
                }

                // change the background to light coral colour
                myCanvas.Background = Brushes.LightCoral;



            }

            


        }

        private void MakePower(int poderEscolhido)
        {

            switch (poderEscolhido)
            {

                case 1:
                    // this is the make star function
                    // this function will create a rectangle, assign the star image to and place it on the canvas

                    // creating a new star rectangle with its own properties inside of it
                    Rectangle newStar = new Rectangle
                    {
                        Height = 50,
                        Width = 50,
                        Tag = "star",
                        Fill = starImage
                    };

                    // set a random left and top position for the star
                    Canvas.SetLeft(newStar, rand.Next(0, 430));
                    Canvas.SetTop(newStar, (rand.Next(100, 400) * -1));

                    // finally add the new star to the canvas to be animated and to interact with the player
                    myCanvas.Children.Add(newStar);
                    break;

                case 2:
                    Rectangle newIce = new Rectangle
                    {
                        Height = 50,
                        Width = 50,
                        Tag = "ice",
                        Fill = iceImage
                    };

                    // set a random left and top position for the star
                    Canvas.SetLeft(newIce, rand.Next(0, 430));
                    Canvas.SetTop(newIce, (rand.Next(100, 400) * -1));

                    // finally add the new star to the canvas to be animated and to interact with the player
                    myCanvas.Children.Add(newIce);
                    break;

                case 3:
                    Rectangle newMultiplier = new Rectangle
                    {
                        Height = 50,
                        Width = 50,
                        Tag = "multi",
                        Fill = MultiplierImage
                    };

                    // set a random left and top position for the star
                    Canvas.SetLeft(newMultiplier, rand.Next(0, 430));
                    Canvas.SetTop(newMultiplier, (rand.Next(100, 400) * -1));

                    // finally add the new star to the canvas to be animated and to interact with the player
                    myCanvas.Children.Add(newMultiplier);
                    break;

            }


           
            

        }

        //Função vai dizer se aquela posição esta ou não disponivel, evitando carros no mesmo lugar
        private bool UsedPosition(Rectangle rec1)
        {

            if (rec1 == RecCarro1)
            {
                Rect carHitBox = new Rect(Canvas.GetLeft(rec1), Canvas.GetTop(rec1), rec1.Width, rec1.Height);
                Rect car2HitBox = new Rect(Canvas.GetLeft(RecCarro2), Canvas.GetTop(RecCarro2), RecCarro2.Width, RecCarro2.Height);
                if (carHitBox.IntersectsWith(car2HitBox))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Rect carHitBox = new Rect(Canvas.GetLeft(rec1), Canvas.GetTop(rec1), rec1.Width, rec1.Height);
                Rect car2HitBox = new Rect(Canvas.GetLeft(RecCarro1), Canvas.GetTop(RecCarro1), RecCarro1.Width, RecCarro1.Height);
                if (carHitBox.IntersectsWith(car2HitBox))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }


        }

        private void SlowPower(int powerEnding)
        {
            //Carro volta ao tamanho normal indicando que o tempo do power Up está terminando
            
            if (powerEnding < 40)
            {
                playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerImage.png"));
                myCanvas.Background = Brushes.Blue;

            }
            else
            {
                // this is the power up function, this function will run when the player collects the star in the game

                playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/powermode2.png"));
                

                // change the background to light coral colour
                myCanvas.Background = Brushes.DarkBlue;




            }

        }

        private void MultiplierPower(int powerEnding)
        {
            //Carro volta ao tamanho normal indicando que o tempo do power Up está terminando
            if (powerEnding < 40)
            {
                playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerImage.png"));
                myCanvas.Background = Brushes.DarkOrange;

            }
            else
            {
                // this is the power up function, this function will run when the player collects the star in the game

                playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/powermode3.png"));


                // change the background to light coral colour
                myCanvas.Background = Brushes.Yellow;




            }




        }
    }
}