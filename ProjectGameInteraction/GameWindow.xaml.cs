using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

using System.Windows.Threading;

namespace ProjectGameInteraction
{
    public partial class GameWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        private bool moveLeft, moveRight, jump;
        private double speedX, speedY, speed = 5, ;

   

        public GameWindow()
        {
            InitializeComponent();
            GameCanvas.Focus();

            gameTimer.Interval = TimeSpan.FromMilliseconds(16);
            gameTimer.Tick += GameTick!;
            gameTimer.Start();
        }


        private void IsKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D:
                    moveRight = true;
                    speedX = speed;
                    break;
                case Key.A:
                    moveLeft = true;
                    speedX = -speed;
                    break;
                case Key.Space:
                    jump = true;
                    break;
            }
        }

        private void IsKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D:
                    moveLeft = false;
                    speedX = 0;
                    break;
                case Key.A:
                    moveRight = false;
                    speedX = 0;
                    break;
                case Key.Space:
                    jump = false;
                    break;
            }
        }

        private void GameTick(object sender, EventArgs e)
        {
            


            Canvas.SetLeft(Player, Canvas.GetLeft(Player) + speedX);
            Canvas.SetTop(Player, Canvas.GetTop(Player) - speedY);

        }
    }
}
