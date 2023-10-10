using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GameTimerExample
{
    public partial class GameWindow : Window
    {
        private ImageBrush playerSkin = new();
        private DispatcherTimer gameTimer = new();

        public GameWindow()
        {
            InitializeComponent();

            try
            {
                playerSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/test.png"));
                Player.Fill = playerSkin;
            } catch (IOException)
            {
                MessageBox.Show("Failed to load skin");
            }

            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += GameEngine;
            gameTimer.Start();

            GameCanvas.Focus();
            ResizeMode = ResizeMode.NoResize;
        }

        private bool moveLeft, moveRight, jump, duck, onGround;
        private (float x, float y) velocity = (0,0);

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    moveLeft = true;
                    break;
                case Key.Right:
                    moveRight = true;
                    break;
                case Key.Up:
                    jump = true;
                    break;
                case Key.Down:
                    duck = true;
                    break;
                case Key.Escape:
                    Close();
                    break;
                default:
                    break;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    moveLeft = false;
                    break;
                case Key.Right:
                    moveRight = false;
                    break;
                case Key.Up:
                    jump = false;
                    break;
                case Key.Down:
                    duck = false;
                    break;
                default:
                    break;
            }
        }

        private void GameEngine(object? sender, EventArgs e)
        {
            // Vertical movement
                // Jump
            if (jump && onGround)
            {
                velocity.y = 40;
                onGround = false;
            }

                // Gravity
            if (velocity.y > -100 && !onGround)
                velocity.y -= 5;

            // Horizontal movement
            if (!(moveLeft && moveRight) && !(!moveLeft && !moveRight))
            {
                // acceleration
                if (moveLeft && velocity.x > -14) velocity.x -= 3.5f;
                else if (moveRight && velocity.x < 14) velocity.x += 3.5f;
            } 
            else if (velocity.x != 0)
            {
                // deceleration
                if (velocity.x > 0)
                    velocity.x -= 3.5f;
                else
                    velocity.x += 3.5f;
            }

            // Move player sprite
            Canvas.SetLeft(Player, Canvas.GetLeft(Player) + velocity.x);
            Canvas.SetBottom(Player, Canvas.GetBottom(Player) + velocity.y);

            // Ground Collision
            Rect rect1 = new(Canvas.GetLeft(Player), Canvas.GetBottom(Player), Player.Width, Player.Height);
            Rect rect2 = new(Canvas.GetLeft(Ground), Canvas.GetBottom(Ground), Ground.Width, Ground.Height);
            if (rect1.IntersectsWith(rect2))
            {
                velocity.y = 0;
                Canvas.SetBottom(Player, Canvas.GetBottom(Ground) + Ground.Height);
                onGround = true;
            }
        }
    }
}
