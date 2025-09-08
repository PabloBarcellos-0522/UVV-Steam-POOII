using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAnimatedGif;

namespace Space_battle_shooter_WPF_MOO_ICT
{
    /// <summary>
    /// Lógica interna para Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        private List<string> shipImages;
        private int currentShipIndex;

        public Menu()
        {
            InitializeComponent();
            shipImages = new List<string>
            {
                "/images/player.png",
                "/images/1.png",
                "/images/2.png",
                "/images/3.png",
                "/images/4.png",
                "/images/5.png"
            };
            currentShipIndex = 0;
            ShowCurrentShip();
        }

        private void ShowCurrentShip()
        {
            playerShipImage.Source = new BitmapImage(new Uri("pack://application:,,," + shipImages[currentShipIndex]));
        }

        private void prevButton_Click(object sender, RoutedEventArgs e)
        {
            currentShipIndex--;
            if (currentShipIndex < 0)
            {
                currentShipIndex = shipImages.Count - 1;
            }
            ShowCurrentShip();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            currentShipIndex++;
            if (currentShipIndex >= shipImages.Count)
            {
                currentShipIndex = 0;
            }
            ShowCurrentShip();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow gameWindow = new MainWindow(shipImages[currentShipIndex]);
            gameWindow.ShowDialog();
            this.Show();
        }

        
    }
}
