using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        private bool moveLeft, moveRight, jump, onGround = true;
        private double speedX, speedY, speed = 10;


        public GameWindow()
        {
            InitializeComponent();
            GameCanvas.Focus();

            gameTimer.Interval = TimeSpan.FromMilliseconds(16);
            gameTimer.Tick += GameTick;
            gameTimer.Start();
        }

        private void IsKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Right:
                case Key.D:
                    moveRight = true;
                    speedX = speed;
                    break;
                case Key.Left:
                case Key.A:
                    moveLeft = true;
                    speedX = -speed;
                    break;
                case Key.Up:
                case Key.W:
                case Key.Space:
                    jump = true;
                    break;
            }
        }

        private void IsKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Right:
                case Key.D:
                    moveLeft = false;
                    speedX = 0;
                    break;
                case Key.Left:
                case Key.A:
                    moveRight = false;
                    speedX = 0;
                    break;
                case Key.Up:
                case Key.W:
                case Key.Space:
                    jump = false;
                    break;
            }
        }

        private void GameTick(object? sender, EventArgs e)
        {
            // Jump
            if (jump && onGround)
            {
                speedY = 40;
                onGround = false;
            }

            // Gravity
            if (speedY > -100)
            {
                speedY -= 5;
            }

            // Player sprite movement
            Canvas.SetLeft(Player, Canvas.GetLeft(Player) + speedX);
            Canvas.SetBottom(Player, Canvas.GetBottom(Player) + speedY);

            // Ground Collision
            Rect playerRect = new(Canvas.GetLeft(Player), Canvas.GetBottom(Player), Player.Width, Player.Height);
            Rect groundRect = new(Canvas.GetLeft(Ground), Canvas.GetBottom(Ground), Ground.Width, Ground.Height);
            if (playerRect.IntersectsWith(groundRect))
            {
                speedY = 0;
                Canvas.SetBottom(Player, Canvas.GetBottom(Ground) + Ground.Height);
                onGround = true;
            }


            Rect platformRect1 = new(Canvas.GetLeft(platform1), Canvas.GetBottom(platform1), platform1.Width, platform1.Height);

            if (Canvas.GetBottom(Player) > Canvas.GetBottom(platform1) && (playerRect.IntersectsWith(platformRect1)))
            {
                speedY = 0;
                Canvas.SetBottom(Player, Canvas.GetBottom(platform1) + platform1.Height);
                onGround = true;
            }
            else if ((playerRect.IntersectsWith(platformRect1)))
            {
                speedY = 0;
                Canvas.SetBottom(Player, Canvas.GetBottom(platform1) - Player.Height);
                onGround = false;
            }
        }
    }
}
