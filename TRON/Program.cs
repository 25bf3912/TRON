using CustomConsole;
using Figgle.Fonts;
using LaglessKeyboardInput;
using NAudio.Midi;
using NAudio.Wave;
using System.Drawing;
using System.Text;
using TRON;

namespace Void
{
    internal class Program
    {
        static void CalibrateScreen(int targetWidth, int targetHeight, int timeToLive = 7)
        {
            Menu m = new Menu(ConsoleBuffer.Width / 2 - 50, ConsoleBuffer.Height / 2 - 12, 100, 25, (255, 255, 255), "\nCALIBRATE YOUR SCREEN!\n\nPRESS ALT+ENTER TO ENTER FULLSCREEN.\nZOOM OUT UNTIL YOUR SCREEN BECOMES GREEN.\n\nPRESS ANY KEY TO START CALIBRATION.", false);
            m.Display();
            ConsoleBuffer.Draw();
            Thread.Sleep(100);
            Console.ReadKey();
            while (true)
            {
                Keyboard.CurrentKey(out List<string> keys);
                if (ConsoleBuffer.Width == targetWidth && ConsoleBuffer.Height == targetHeight || keys.Contains("RETURN"))
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
                    timeToLive = 7;
                }
                if (timeToLive < 1)
                    break;
                Thread.Sleep(20);
            }
        }
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.Title = "TRON";
            CalibrateScreen(944, 355); // in cmd.exe do mode con: width=944 lines=206
            Menu menu = new Menu(ConsoleBuffer.Width / 2 - 50, ConsoleBuffer.Height / 2 - 12, 100, 25, (255, 255, 255), "PAUSE MENU", true);
            //using (var audioFile = new AudioFileReader("INIT.m4a"))
            //using (var outputDevice = new WaveOutEvent())
            //outputDevice.Init(audioFile);
            //outputDevice.Play();
            ConsoleBuffer.Fill(' ', 0, 0, 0);
            Renderer.LoadingBar(500);
            
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