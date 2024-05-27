using SplashKitSDK;

namespace SpaceShipGame
{
    public class Asteroid
    {
        private Vector2D _velocity;
        public double X { get; protected set; }
        public double Y { get; protected set; }
        public Bitmap Bitmap { get; }
        private const int SPEED = 4;

        public Asteroid(Window gameWindow, SpaceShip player)
        {
            if (!SplashKit.HasBitmap("Asteroid"))
            {
                Bitmap = new Bitmap("Asteroid", "Asteroid.png");
            }
            else
            {
                Bitmap = SplashKit.BitmapNamed("Asteroid");
            }

            if (SplashKit.Rnd() < 0.5)
            {
                X = SplashKit.Rnd(gameWindow.Width);
                Y = SplashKit.Rnd() < 0.5 ? -Bitmap.Height : gameWindow.Height;
            }
            else
            {
                X = SplashKit.Rnd() < 0.5 ? -Bitmap.Width : gameWindow.Width;
                Y = SplashKit.Rnd(gameWindow.Height);
            }

            Point2D fromPt = new Point2D() { X = X, Y = Y };
            Point2D toPt = new Point2D() { X = player.X, Y = player.Y };
            Vector2D dir = SplashKit.UnitVector(SplashKit.VectorPointToPoint(fromPt, toPt));
            _velocity = SplashKit.VectorMultiply(dir, SPEED);
        }

        public void Draw()
        {
            Bitmap.Draw((int)X, (int)Y);
        }

        public void Update()
        {
            X += _velocity.X;
            Y += _velocity.Y;
        }

        public bool IsOffscreen(Window gameWindow)
        {
            return X < -Bitmap.Width || X > gameWindow.Width || Y < -Bitmap.Height || Y > gameWindow.Height;
        }
    }
}
