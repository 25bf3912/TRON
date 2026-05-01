using CustomConsole;
using NAudio.Wave;

namespace Void
{
    internal class Program
    {
        static void CalibrateScreen(int targetWidth, int targetHeight, int timeToLive = 150)
        {
            while (true)
            {
                if (ConsoleBuffer.Width == targetWidth && ConsoleBuffer.Height == targetHeight)
                {
                    ConsoleBuffer.Fill('@', 0, 255, 0);
                    ConsoleBuffer.Draw();
                    ConsoleBuffer.ResizeBuffer();
                    timeToLive--;
                }
                else
                {
                    ConsoleBuffer.Fill('@', 255, 0, 0);
                    ConsoleBuffer.Draw();
                    ConsoleBuffer.ResizeBuffer();
                    timeToLive = 150;
                    Thread.Sleep(20);
                }
                if (timeToLive < 1)
                    break;
            }
        }
        static void Main(string[] args)
        {
            Console.Title = "TRON";
            CalibrateScreen(944, 355); // in cmd.exe do mode con: width=944 lines=206
            Menu menu = new Menu(ConsoleBuffer.Width / 2 - 50, ConsoleBuffer.Height / 2 - 12, 100, 25, (255, 255, 255), "PAUSE MENU");
            //using (var audioFile = new AudioFileReader("INIT.m4a"))
            //using (var outputDevice = new WaveOutEvent())
            {
                //outputDevice.Init(audioFile);
                //outputDevice.Play();
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
}