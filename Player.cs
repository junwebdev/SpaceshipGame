using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace SpaceShipGame
{
    public class SpaceShip
    {
        private Bitmap _bitmap;
        private const int BULLET_DELAY = 5;
        private int _bulletTimer = BULLET_DELAY;

        public int X { get; private set; }
        public int Y { get; private set; }
        public bool Quit { get; private set; }
        public int Lives { get; private set; }
        public int Score { get; private set; }
        public List<Bullet> Bullets { get; private set; }

        public SpaceShip(Window gameWindow)
        {
            _bitmap = new Bitmap("SpaceShip", "SpaceShip.png");
            X = (gameWindow.Width - _bitmap.Width) / 2;
            Y = (gameWindow.Height - _bitmap.Height) / 2;
            Lives = 3;
            Score = 0;
            Bullets = new List<Bullet>();
        }

        public void HandleInput()
        {
            if (SplashKit.KeyDown(KeyCode.UpKey)) Y -= 5;
            if (SplashKit.KeyDown(KeyCode.DownKey)) Y += 5;
            if (SplashKit.KeyDown(KeyCode.LeftKey)) X -= 5;
            if (SplashKit.KeyDown(KeyCode.RightKey)) X += 5;

            if (SplashKit.KeyTyped(KeyCode.SpaceKey))
            {
                if (_bulletTimer <= 0)
                {
                    Bullets.Add(new Bullet(X + _bitmap.Width / 2, Y + _bitmap.Height / 2, SplashKit.MousePosition()));
                    _bulletTimer = BULLET_DELAY;
                }
            }

            if (SplashKit.KeyTyped(KeyCode.EscapeKey))
            {
                Quit = true;
            }

            _bulletTimer--;
        }

        public void Draw()
        {
            _bitmap.Draw(X, Y);
            foreach (var bullet in Bullets)
            {
                bullet.Draw();
            }
        }

        public void StayOnWindow(Window gameWindow)
        {
            const int GAP = 10;

            if (X < GAP) X = GAP;
            if (Y < GAP) Y = GAP;
            if (X > gameWindow.Width - _bitmap.Width - GAP) X = gameWindow.Width - _bitmap.Width - GAP;
            if (Y > gameWindow.Height - _bitmap.Height - GAP) Y = gameWindow.Height - _bitmap.Height - GAP;
        }

        public bool CollidedWith(Asteroid asteroid)
        {
            return SplashKit.CirclesIntersect(
                SplashKit.CircleAt(X + _bitmap.Width / 2, Y + _bitmap.Height / 2, _bitmap.Width / 2),
                SplashKit.CircleAt((int)asteroid.X + asteroid.Bitmap.Width / 2, (int)asteroid.Y + asteroid.Bitmap.Height / 2, asteroid.Bitmap.Width / 2)
            );
        }

        public void LoseLife()
        {
            Lives--;
            if (Lives <= 0)
            {
                Quit = true;
            }
        }

        public void IncreaseScore()
        {
            Score++;
        }
    }
}
