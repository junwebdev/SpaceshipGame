using SplashKitSDK;

namespace SpaceShipGame
{
    public class Bullet
    {
        private const int SPEED = 8;
        private const int RADIUS = 5;
        private Vector2D Velocity { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool ShouldRemove { get; set; }

        public Bullet(int startX, int startY, Point2D target)
        {
            X = startX;
            Y = startY;
            Point2D fromPt = new Point2D() { X = startX, Y = startY };
            Vector2D dir = SplashKit.UnitVector(SplashKit.VectorPointToPoint(fromPt, target));
            Velocity = SplashKit.VectorMultiply(dir, SPEED);
        }

        public void Draw()
        {
            SplashKit.FillCircle(Color.Yellow, X, Y, RADIUS);
        }

        public void Update()
        {
            X += (int)Velocity.X;
            Y += (int)Velocity.Y;

            if (X < 0 || X > SplashKit.ScreenWidth() || Y < 0 || Y > SplashKit.ScreenHeight())
            {
                ShouldRemove = true;
            }
        }

        public bool CollidedWith(Asteroid asteroid)
        {
            return SplashKit.CirclesIntersect(
                SplashKit.CircleAt(X, Y, RADIUS),
                SplashKit.CircleAt((int)asteroid.X + asteroid.Bitmap.Width / 2, (int)asteroid.Y + asteroid.Bitmap.Height / 2, asteroid.Bitmap.Width / 2)
            );
        }
    }
}
