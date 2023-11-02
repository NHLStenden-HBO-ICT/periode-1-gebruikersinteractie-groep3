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
        public class Ground : Platform
        {
            // Constructors
            public Ground(float x, float y, float width) : this(x, y, width, Colors.Green) { }

            public Ground(float x, float y, float width, Color color, Rectangle? element = null) : base(x, y, width, color, element)
            {
                platform = new(x, 0, width, y);
            }
        }

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
            protected Rect platform;

            // Properties
            public float X { get; private set; } // Left
            public float Y { get; private set; } // Bottom
            public float Length { get; private set; }
            public Color Color { get; set; }
            public Rectangle? Element { get; internal set; }

            // Constructors
            public Platform(float x, float y, float length) : this(x, y, length, Color.FromRgb(0,0,0)) { }
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
                if (Canvas.GetBottom(player) > Canvas.GetBottom(Element))
                    return 1;
                else if (lastCoordinate.y <= Canvas.GetBottom(Element))
                    return 2;
                else if (lastCoordinate.x <= Canvas.GetLeft(Element) && (Canvas.GetLeft(Element) <= Canvas.GetLeft(player) + player.Width))
                    return 3;
                else if (lastCoordinate.x >= Canvas.GetLeft(Element) + Element.Width && (Canvas.GetLeft(Element) + Element.Width >= Canvas.GetLeft(player)))
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

        public float End { get; private set; } // X coordinate for level end
        public List<Ground> Grounds { get; private set; }
        public List<Coin> Coins { get; private set; }
        public List<Platform> Platforms { get; private set; }
        public List<Enemy> Enemies { get; private set; }

        public Level(
            float end,
            List<Ground> grounds,
            List<Coin> coins, 
            List<Platform> platforms,
            List<Enemy> enemies
            )
        {
            Grounds = grounds;
            End = end;
            Coins = coins;
            Platforms = platforms;
            Enemies = enemies;
        }

        public bool Finished(Rectangle player)
        {
            return Canvas.GetLeft(player) + player.Width >= End;
        }

        // returns true if on top
        public bool GroundCollision(Canvas canvas, Rectangle player, (double x, double y) lastCoordinate)
        {
            foreach (var ground in Grounds)
            {
                switch(ground.IntersectsWithDirectional(player, lastCoordinate))
                {
                    case 1:
                        Canvas.SetBottom(player, ground.Y);
                        return true;
                    case 3:
                        Canvas.SetLeft(player, ground.X + player.Width);
                        break;
                    case 4:
                        Canvas.SetLeft(player, ground.X + ground.Length);
                        break;
                    default:
                        break;
                }
            }
            return false;
        }

        public int CoinCollision(Canvas canvas, Rectangle player)
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
            foreach ( var ground in Grounds)
            {
                var groundElement = new Rectangle()
                {
                    Height = ground.Y + 100,
                    Width = ground.Length,
                    Fill = new SolidColorBrush(Colors.Green)
                };
                Canvas.SetBottom(groundElement, -100);
                Canvas.SetLeft(groundElement, ground.X);
                canvas.Children.Add(groundElement);
                ground.Element = groundElement;
            }

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


            foreach ( var platform in Platforms)
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

            var finish = new Rectangle()
            {
                Height = 1000,
                Width = 90,
                Fill = new DrawingBrush()
                {
                    TileMode = TileMode.Tile,
                    Viewport = new(0, 0, 32, 32),
                    ViewportUnits = BrushMappingMode.Absolute,
                    Drawing = new GeometryDrawing()
                    {
                        Geometry = Geometry.Parse("M0,0 H1 V1 H2 V2 H1 V1 H0Z"),
                        Brush = new SolidColorBrush(Colors.Black)
                    }
                }
            };
            Canvas.SetBottom(finish, 0);
            Canvas.SetLeft(finish, End);
            canvas.Children.Add(finish);
        }
    }
}
