﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
using static ProjectGameInteraction.Level;

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

        private void PauseButtonClick(object sender, RoutedEventArgs e)
        {
            gameTimer.Stop();
            levelTimer.Stop();
            MyPopup.IsOpen = true;
        }


        private void ResumeButtonClick(object sender, RoutedEventArgs e)
        {
            gameTimer.Start();
            levelTimer.Start();
            MyPopup.IsOpen = false;
            GameCanvas.Focus();
        }

        private void HoofdmenuClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private double cameraOffsetX = 0; // Track the camera offset
        private const double GAMEWINDOWWIDTH = 800;

        private Level level = new(
            new() { new(650f, 60f), new(250f, 60f), new(400f, 60f) },
            new() { new(400f, 130f, 200f), new(800f, 130f, 200f) },
            new() { new(600f, 40, 3f), new(1000f, 40, 3f) }
        );
        private int collectedCoins = 0;

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

            // Gamewindow in full window 
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            level.Draw(GameCanvas);

            
        }

   


        
        private int levelTime;
        private void LevelTick(object? sender, EventArgs e) 
        {
            if (levelTime == 0)
            {
                GameOverScherm GOwindow = new();
                GOwindow.Show();
                Close();
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
            double cameraCenterX = GAMEWINDOWWIDTH / 2;
            cameraOffsetX = playerX - cameraCenterX;

            if (cameraOffsetX < 0)
            {
                cameraOffsetX = 0;
            }
            // Adjust the canvas position to follow the player
            GameCanvas.RenderTransform = new TranslateTransform(-cameraOffsetX, 0);
            // Adjust the timer position to follow the player
            TimerLabel.RenderTransform = new TranslateTransform(cameraOffsetX, 0);
            
            Coins_Count_Symbol.RenderTransform = new TranslateTransform(cameraOffsetX, 0);
            coinCountTextBlock.RenderTransform = new TranslateTransform(cameraOffsetX, 0);
            ResumeButton.RenderTransform = new TranslateTransform(cameraOffsetX, 0);
            PauseButton.RenderTransform = new TranslateTransform(cameraOffsetX, 0);


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
                onGround = false;
            }

            // Player sprite movement
            Canvas.SetLeft(Player, Canvas.GetLeft(Player) + speedX);
            Canvas.SetBottom(Player, Canvas.GetBottom(Player) + speedY);


            // Walls so player can't run off screen left side at the start
            if (Canvas.GetLeft(Player) <= 0)
            {
                
                Canvas.SetLeft(Player, 0);
            }



            

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
            foreach (var platform in level.Platforms)
            {
                switch(platform.IntersectsWithDirectional(Player, lastCoordinate))
                {
                    case 1:
                        speedY = 0;
                        Canvas.SetBottom(Player, platform.Y + Level.Platform.PLATFORMHEIGHT);
                        onGround = true;
                        break;
                    case 2:
                        speedY = 0;
                        Canvas.SetBottom(Player, platform.Y - Player.Height);
                        onGround = false;
                        break;
                    case 3:
                        speedX = 0;
                        Canvas.SetLeft(Player, platform.X - Player.Width);
                        break;
                    case 4:
                        speedX = 0;
                        Canvas.SetLeft(Player, platform.X + platform.Length);
                        break;
                    default:
                        break;
                }
            }

            List<Enemy> enemiesToRemove = new();
            foreach (var enemy in level.Enemies)
            {
                // check if enemy element exists
                if (enemy.Element == null) continue;

                Rect enemyRect = new(Canvas.GetLeft(enemy.Element), Canvas.GetBottom(enemy.Element), Enemy.ENEMYWIDTH, Enemy.ENEMYHEIGHT);
                if (Canvas.GetLeft(enemy.Element) <= 0)
                    enemy.Speed *= -1;


                // Enemy movement
                Canvas.SetLeft(enemy.Element, Canvas.GetLeft(enemy.Element) - enemy.Speed);

                // Enemy Collision
                if (!enemyRect.IntersectsWith(playerRect)) continue; // no collision

                if (lastCoordinate.y > Canvas.GetBottom(enemy.Element) + Enemy.ENEMYHEIGHT) // player jumps on top of enemy
                {
                    speedY = 30;
                    enemiesToRemove.Add(enemy);

                }
                else
                {
                    // Enemy turns around (for collision with walls) (is temporary until walls added)
                    enemySpeed *= -1;

                    gameTimer.Stop();
                    GameOverScherm GOwindow = new();
                    GOwindow.Show();
                    Close();
                }
            }
            // Remove killed enemies from level
            foreach (var enemy in enemiesToRemove)
            {
                level.Enemies.Remove(enemy);
                GameCanvas.Children.Remove(enemy.Element);
            }


            // Player out of bounds
            if (Canvas.GetBottom(Player) < -100)
            {
                gameTimer.Stop();
                GameOverScherm GOwindow = new();
                GOwindow.Show();
                Close();
            }


            collectedCoins += level.CheckCoinCollision(GameCanvas, Player);
            coinCountTextBlock.Text = collectedCoins.ToString();

            lastCoordinate = (Canvas.GetLeft(Player), Canvas.GetBottom(Player));
        }
        
    }
}
