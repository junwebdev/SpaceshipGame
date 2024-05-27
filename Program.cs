using SplashKitSDK;

namespace SpaceShipGame
{
    public class Program
    {
        public static void Main()
        {
            Window gameWindow = new Window("SpaceShip Game", 800, 600);
            SpaceShipGame game = new SpaceShipGame(gameWindow);

            while (!game.Quit)
            {
                SplashKit.ProcessEvents();
                game.HandleInput();
                game.Update();
                game.Draw();
            }
        }
    }
}
