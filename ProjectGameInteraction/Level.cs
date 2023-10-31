using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectGameInteraction
{
    public class Level
    {
        public class Coin
        {
            // Constants
            public const int COINDIAMATER = 32;

            // Variables
            private Rect coin;

            // Properties
            public float X { get; private set; } // Left
            public float Y { get; private set; } // Bottom
            public UIElement? Element { get; internal set; }

            // Constructors
            public Coin(float x, float y) : this(x,y,null) { }
            public Coin(float x, float y, UIElement? element)
            {
                X = x;
                Y = y;
                Element = element;
                coin = new(X, Y, COINDIAMATER, COINDIAMATER);
            }

            // Methods
            public bool IntersectsWith(Rect rect)
            {
                if (Element == null) return false;
                return coin.IntersectsWith(rect);
            }
        }

        public class Platform
        {
            // Constants
            public const int PLATFORMHEIGHT = 40;

            // Variables
            private Rect platform;

            // Properties
            public float X { get; private set; } // Left
            public float Y { get; private set; } // Bottom
            public float Length { get; private set; }
            public Color Color { get; set; }
            public Rectangle? Element { get; internal set; }

            // Constructors
            public Platform(float x, float y, float length) : this(x, y, length, Color.FromRgb(127,127,127)) { }
            public Platform(float x, float y, float length, Color color, Rectangle? element = null)
            {
                X = x;
                Y = y;
                Length = length;
                Element = element;
                platform = new(X, Y, Length, PLATFORMHEIGHT);
                Color = color;
            }

            // Methods
            public bool IntersectsWith(Rect rect)
            {
                if (Element == null) return false;
                return platform.IntersectsWith(rect);
            }

            /* returns  0 -> no intersection
             *          1 -> top
             *          2 -> bottom
             *          3 -> left
             *          4 -> right
             */
            public int IntersectsWithDirectional(Rectangle player, (double x, double y) lastCoordinate)
            {
                Rect playerRect = new(Canvas.GetLeft(player), Canvas.GetBottom(player), player.Width, player.Height);
                if (Element == null || !platform.IntersectsWith(playerRect)) return 0;
                if (Canvas.GetBottom(player) > Canvas.GetBottom(Element) && (playerRect.IntersectsWith(platform)))
                    return 1;
                else if (lastCoordinate.y <= Canvas.GetBottom(Element) && (playerRect.IntersectsWith(platform)))
                    return 2;
                else if (lastCoordinate.x <= Canvas.GetLeft(Element) && (Canvas.GetLeft(Element) <= Canvas.GetLeft(player) + player.Width) && playerRect.IntersectsWith(platform))
                    return 3;
                else if (lastCoordinate.x >= Canvas.GetLeft(Element) + Element.Width && (Canvas.GetLeft(Element) + Element.Width >= Canvas.GetLeft(player)) && playerRect.IntersectsWith(platform))
                    return 4;
                
              
                return 0;
            }
        }

        public class Enemy
        {
            // Constants
            public const int ENEMYHEIGHT = 50;
            public const int ENEMYWIDTH = 40;

            // Variables
            private Rect enemy;

            // Properties
            public float X { get; private set; } // Left
            public float Y { get; private set; } // Bottom
            public double Speed { get; set; }
            public float Height { get; private set; }
            public float Width { get; private set; }
            public Color Color { get; set; }
            public Rectangle? Element { get; internal set; }

            // Constructors
            public Enemy(float x, float y, float speed) : this(x, y, speed, Color.FromRgb(255, 0, 0)) { }
            public Enemy(float x, float y, float speed, Color color, float height = ENEMYHEIGHT, float width = ENEMYWIDTH, Rectangle? element = null)
            {
                X = x;
                Y = y;
                Speed = speed;
                Height = height;
                Width = width;
                Element = element;
                enemy = new(X, Y, ENEMYHEIGHT, ENEMYWIDTH);
                Color = color;
            }
        }

        public List<Coin> Coins { get; private set; }
        public List<Platform> Platforms { get; private set; }
        public List<Enemy> Enemies { get; private set; }

        public Level() : this(new(), new(), new())
        {
        }

        public Level(
            List<Coin> coins, 
            List<Platform> platforms,
            List<Enemy> enemies
            )
        {
            Coins = coins;
            Platforms = platforms;
            Enemies = enemies;
        }

        public int CheckCoinCollision(Canvas canvas, Rectangle player)
        {
            int collectedCoins = 0;
            Rect playerRect = new(Canvas.GetLeft(player), Canvas.GetBottom(player), player.Width, player.Height);
            List<Coin> coinsToRemove = new(); 
            foreach ( var coin in Coins )
            {
                if (coin.IntersectsWith(playerRect))
                {
                    coinsToRemove.Add(coin);
                    collectedCoins++;
                }
            }
            foreach ( var coin in coinsToRemove)
            {
                Coins.Remove(coin); 
                canvas.Children.Remove(coin.Element);
            }
            return collectedCoins;
        }

        // SHOULD ONLY BE CALLED ON LEVEL START
        public void Draw(Canvas canvas)
        {
            foreach ( var coin in Coins )
            {
                var coinElement = new Ellipse()
                {
                    Height = Coin.COINDIAMATER,
                    Width = Coin.COINDIAMATER,
                    Fill = new ImageBrush()
                    {
                        ImageSource = new BitmapImage(new Uri("Afbeeldingen\\3885422.png", UriKind.Relative))
                    }
                };
                Canvas.SetBottom(coinElement, coin.Y);
                Canvas.SetLeft(coinElement, coin.X);
                canvas.Children.Add(coinElement);
                coin.Element = coinElement;
            }


            foreach ( Platform platform in Platforms)
            {
                var platformElement = new Rectangle()
                {
                    Width = platform.Length,
                    Height = Platform.PLATFORMHEIGHT,
                    Fill = new SolidColorBrush(platform.Color)
                };
                Canvas.SetBottom(platformElement, platform.Y);
                Canvas.SetLeft(platformElement, platform.X);
                canvas.Children.Add(platformElement);
                platform.Element = platformElement;
            }

            foreach ( var enemy in Enemies )
            {
                var enemyElement = new Rectangle()
                {
                    Height = enemy.Height,
                    Width = enemy.Width,
                    Fill = new SolidColorBrush(enemy.Color)
                };
                Canvas.SetBottom(enemyElement, enemy.Y);
                Canvas.SetLeft(enemyElement, enemy.X);
                canvas.Children.Add(enemyElement);
                enemy.Element = enemyElement;
            }
        }
    }
}
