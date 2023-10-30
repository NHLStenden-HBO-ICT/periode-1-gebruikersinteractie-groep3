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
using static ProjectGameInteraction.Level;

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
            public float X { get; private set; }
            public float Y { get; private set; }
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
            public float X { get; private set; }
            public float Y { get; private set; }
            public float Length { get; private set; }
            public Rectangle? Element { get; internal set; }

            // Constructors
            public Platform(float x, float y, float length) : this(x, y, length, null) { }
            public Platform(float x, float y, float length, Rectangle? element)
            {
                X = x;
                Y = y;
                Length = length;
                Element = element;
                platform = new(X, Y, Length, PLATFORMHEIGHT);
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

        public List<Coin> Coins { get; private set; }
        public List<Platform> Platforms { get; private set; }
        public List<(float x, float y, UIElement? element)> Enemies { get; private set; }

        public Level() : this(new(), new(), new())
        {
        }

        public Level(
            List<Coin> coins, 
            List<Platform> platforms,
            List<(float x, float y, UIElement? element)> enemies
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
            List<UIElement> coinElements = new();
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
                coinElements.Add(coinElement);
            }


            foreach ( Platform platform in Platforms)
            {
                var platformElement = new Rectangle()
                {
                    Width = platform.Length,
                    Height = Platform.PLATFORMHEIGHT,
                    Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0))
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
                    Height = 50,
                    Width = 40,
                    Fill = new SolidColorBrush(Color.FromRgb(0, 0, 255))
                };
                Canvas.SetBottom(enemyElement, enemy.y);
                Canvas.SetLeft(enemyElement, enemy.x);
                canvas.Children.Add(enemyElement);
            }
        }
    }
}
