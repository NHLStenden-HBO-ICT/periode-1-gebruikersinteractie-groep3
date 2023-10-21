using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
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
        DispatcherTimer levelTimer = new DispatcherTimer();

        private bool moveLeft, moveRight, jump, onGround;
        private double speedX, speedY, speed = 10;
        private (double x, double y) lastCoordinate;
        private double enemySpeed = 3;
        
        private const int LEVELTIME = 300;

        private double cameraOffsetX = 0; // Track the camera offset

        public GameWindow()
        {
            InitializeComponent();
            lastCoordinate = (Canvas.GetLeft(Player), Canvas.GetBottom(Player));
            GameCanvas.Focus();

            // game tick
            gameTimer.Interval = TimeSpan.FromMilliseconds(16);
            gameTimer.Tick += GameTick;
            gameTimer.Start();

            // level timer
            levelTimer.Interval = TimeSpan.FromSeconds(1);
            levelTimer.Tick += LevelTick;
            levelTimer.Start();
            levelTime = LEVELTIME;
            TimerLabel.Content = levelTime;
        }

        private int levelTime;
        private void LevelTick(object? sender, EventArgs e) 
        {
            if (levelTime == 0)
            {
                MainWindow window = new();
                Close();
                window.Show();
                levelTimer.Stop();
            } else
            {
                levelTime--;
                TimerLabel.Content = levelTime;
            }
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
            {
                speedX = -speed;
                
                
            }                
            else if (moveRight && !moveLeft)
            {
                speedX = speed;
                
            }
            else
            {
                speedX = 0;
            }

            double playerX = Canvas.GetLeft(Player);
            double canvasWidth = GameCanvas.ActualWidth;
            double cameraCenterX = canvasWidth / 2;
            cameraOffsetX = playerX - cameraCenterX;

            // Adjust the canvas position to follow the player
            GameCanvas.RenderTransform = new TranslateTransform(-cameraOffsetX, 0);

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


            // Walls so player can't run off screen left side at the start
            if (Canvas.GetLeft(Player) <= 0)
            {
                
                Canvas.SetLeft(Player, 0);
            }

            if (Canvas.GetLeft(Enemy1) <= 0)
            {
                enemySpeed *= -1 ;
                
            }


            // Enemy sprite movement & Rect
            Canvas.SetLeft(Enemy1, Canvas.GetLeft(Enemy1) - enemySpeed);
            Rect enemyRect = new(Canvas.GetLeft(Enemy1), Canvas.GetBottom(Enemy1), Enemy1.Width, Enemy1.Height);

            // Ground Collision
            Rect playerRect = new(Canvas.GetLeft(Player), Canvas.GetBottom(Player), Player.Width, Player.Height);
            Rect groundRect = new(Canvas.GetLeft(Ground), Canvas.GetBottom(Ground), Ground.Width, Ground.Height);
            if (playerRect.IntersectsWith(groundRect))
            {
                speedY = 0;
                Canvas.SetBottom(Player, Canvas.GetBottom(Ground) + Ground.Height);
                onGround = true;
            }

            // Platform Collision
            Rect platformRect1 = new(Canvas.GetLeft(platform1), Canvas.GetBottom(platform1), platform1.Width, platform1.Height);

            if (Canvas.GetBottom(Player) > Canvas.GetBottom(platform1) && (playerRect.IntersectsWith(platformRect1)))
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

           
            // Enemy Collision (TEMP)
            if (lastCoordinate.y > Canvas.GetBottom(Enemy1) + Enemy1.Height && playerRect.IntersectsWith(enemyRect))
            {
                speedY = 30;
                Canvas.SetBottom(Enemy1, -100);
                GameCanvas.Children.Remove(Enemy1);
                
            }
            else if (enemyRect.IntersectsWith(playerRect))
            {
                // Enemy turns around (for collision with walls) (is temporary until walls added)
                enemySpeed *= -1;

                MessageBox.Show("You died!");
                gameTimer.Stop();
                MainWindow window = new();
                window.Show();
                Close();
            }

            lastCoordinate = (Canvas.GetLeft(Player), Canvas.GetBottom(Player));
        }
        
    }
}
