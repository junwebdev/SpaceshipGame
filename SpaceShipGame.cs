using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace SpaceShipGame
{
    public class SpaceShipGame
    {
        private SpaceShip _spaceShip;
        private Window _gameWindow;
        private List<Asteroid> _asteroids = new List<Asteroid>();
        private Bitmap _heartBitmap;
        private DateTime _startTime;

        public bool Quit => _spaceShip.Quit;

        public SpaceShipGame(Window gameWindow)
        {
            _gameWindow = gameWindow;
            _spaceShip = new SpaceShip(_gameWindow);

            if (!SplashKit.HasBitmap("Heart"))
            {
                _heartBitmap = new Bitmap("Heart", "Heart.png");
            }
            else
            {
                _heartBitmap = SplashKit.BitmapNamed("Heart");
            }

            _startTime = DateTime.Now;
        }

        private void DrawHeartIcons()
        {
            const int heartSize = 20;
            const int heartPadding = 18;
            int heartY = heartPadding;

            for (int i = 0; i < _spaceShip.Lives; i++)
            {
                _heartBitmap.Draw(_gameWindow.Width - heartSize - heartPadding, heartY);
                heartY += heartSize + heartPadding;
            }
        }

        public void HandleInput()
        {
            _spaceShip.HandleInput();
            _spaceShip.StayOnWindow(_gameWindow);
        }

        public void Draw()
        {
            _gameWindow.Clear(Color.Black);

            foreach (var asteroid in _asteroids)
            {
                asteroid.Draw();
            }

            _spaceShip.Draw();

            DrawHeartIcons();

            SplashKit.DrawText($"Score: {_spaceShip.Score}", Color.White, 10, 10);

            _gameWindow.Refresh(60);
        }

        public void Update()
        {
            UpdateAsteroids();
            CheckCollisions();

            TimeSpan elapsedTime = DateTime.Now - _startTime;

            if (elapsedTime.TotalMilliseconds >= 1000)
            {
                _spaceShip.IncreaseScore();
                _startTime = DateTime.Now;
            }

            if (ShouldAddRandomAsteroid())
            {
                _asteroids.Add(new Asteroid(_gameWindow, _spaceShip));
            }

            List<Bullet> bulletsToRemove = new List<Bullet>();

            foreach (var bullet in _spaceShip.Bullets)
            {
                bullet.Update();
                if (bullet.ShouldRemove)
                {
                    bulletsToRemove.Add(bullet);
                    continue;
                }

                foreach (var asteroid in _asteroids)
                {
                    if (bullet.CollidedWith(asteroid))
                    {
                        _spaceShip.IncreaseScore();
                        bulletsToRemove.Add(bullet);
                        _asteroids.Remove(asteroid);
                        break;
                    }
                }
            }

            foreach (var bullet in bulletsToRemove)
            {
                _spaceShip.Bullets.Remove(bullet);
            }
        }

        private void UpdateAsteroids()
        {
            foreach (var asteroid in _asteroids)
            {
                asteroid.Update();
            }

            for (int i = _asteroids.Count - 1; i >= 0; i--)
            {
                if (_asteroids[i].IsOffscreen(_gameWindow))
                {
                    _asteroids.RemoveAt(i);
                }
            }
        }

        private void CheckCollisions()
        {
            var asteroidsToRemove = new List<Asteroid>();

            foreach (var asteroid in _asteroids)
            {
                if (_spaceShip.CollidedWith(asteroid))
                {
                    _spaceShip.LoseLife();
                    asteroidsToRemove.Add(asteroid);
                }
            }

            foreach (var asteroidToRemove in asteroidsToRemove)
            {
                _asteroids.Remove(asteroidToRemove);
            }
        }

        private bool ShouldAddRandomAsteroid()
        {
            double probability = 0.02;
            return SplashKit.Rnd() < probability;
        }
    }
}
