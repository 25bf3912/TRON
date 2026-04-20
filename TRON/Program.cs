using CustomConsole;

namespace Void
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
            Menu menu = new Menu(ConsoleBuffer.Width / 2 - 50, ConsoleBuffer.Height / 2 - 12, 100, 25, (255, 255, 255), "PAUSE MENU");
            while (true)
            {
                Game.Tick();
                if (!Game.isRunning)
                {
                    menu.DrawBox();
                    ConsoleBuffer.Draw();
                }
            }

        }
    }
}