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

        private bool moveLeft, moveRight, jump, onGround;
        private double speedX, speedY, speed = 10;
        private const int LEVELTIME = 300;
        private (double x, double y) lastCoordinate;


        public GameWindow()
        {
            InitializeComponent();
            lastCoordinate = (Canvas.GetLeft(Player), Canvas.GetBottom(Player));
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
                    break;
                case Key.Left:
                case Key.A:
                    moveLeft = true;
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
                    moveRight = false;
                    break;
                case Key.Left:
                case Key.A:
                    moveLeft = false;
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
            // Movement
            if (moveLeft && !moveRight)
                speedX = -speed;
            else if (moveRight && !moveLeft)
                speedX = speed;
            else
                speedX = 0;


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
            Rect rect1 = new(Canvas.GetLeft(Player), Canvas.GetBottom(Player), Player.Width, Player.Height);
            Rect rect2 = new(Canvas.GetLeft(Ground), Canvas.GetBottom(Ground), Ground.Width, Ground.Height);
            if (rect1.IntersectsWith(rect2))
            {
                speedY = 0;
                Canvas.SetBottom(Player, Canvas.GetBottom(Ground) + Ground.Height);
                onGround = true;
            }

            Rect platformRect1 = new(Canvas.GetLeft(platform1), Canvas.GetBottom(platform1), platform1.Width, platform1.Height);

            if (lastCoordinate.y >= Canvas.GetBottom(platform1) + platform1.Height && Canvas.GetBottom(Player) > Canvas.GetBottom(platform1) && (playerRect.IntersectsWith(platformRect1)))
            {
                speedY = 0;
                Canvas.SetBottom(Player, Canvas.GetBottom(platform1) + platform1.Height);
                onGround = true;
            }
            else if (lastCoordinate.y <= Canvas.GetBottom(platform1) && (playerRect.IntersectsWith(platformRect1)))
            {
                speedY = 0;
                Canvas.SetBottom(Player, Canvas.GetBottom(platform1) - Player.Height);
                onGround = false;
            }
            else if (lastCoordinate.x <= Canvas.GetLeft(platform1) && (Canvas.GetLeft(platform1) <= Canvas.GetLeft(Player) + Player.Width) && playerRect.IntersectsWith(platformRect1))
            {
                speedX = 0;
                Canvas.SetLeft(Player, Canvas.GetLeft(platform1) - Player.Width);
            }
            else if (lastCoordinate.x >= Canvas.GetLeft(platform1) + platform1.Width && (Canvas.GetLeft(platform1) + platform1.Width >= Canvas.GetLeft(Player)) && playerRect.IntersectsWith(platformRect1))
            {
                speedX = 0;
                Canvas.SetLeft(Player, Canvas.GetLeft(platform1) + platform1.Width);
            }

            lastCoordinate = (Canvas.GetLeft(Player), Canvas.GetBottom(Player));
        }
    }
}
