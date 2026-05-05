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
            Console.WriteLine("Have you read README.md at https://github.com/25bf3912/TRON/blob/master/README.md (Y/N): ");
            if (Console.ReadLine().ToUpper().Substring(0, 1) != "Y") { Console.WriteLine("Read README.md at https://github.com/25bf3912/TRON/blob/master/README.md before continuing"); Console.ReadKey(); Environment.Exit(0); }
            Console.WriteLine("Enter the *verification code* from README.md: ");
            if (Console.ReadLine().ToUpper() != "B8A3E7") { Console.WriteLine("Incorrect verification code. ");  Console.WriteLine("Read README.md at https://github.com/25bf3912/TRON/blob/master/README.md before continuing"); Console.ReadKey(); Environment.Exit(0); }
            CalibrateScreen(944, 355); // in cmd.exe do mode con: width=944 lines=206
            Menu menu = new Menu(ConsoleBuffer.Width / 2 - 50, ConsoleBuffer.Height / 2 - 12, 100, 25, (255, 255, 255), "\nCALIBRATE YOUR SCREEN!\n\nPRESS ALT+ENTER TO ENTER FULLSCREEN.\nZOOM OUT UNTIL YOUR SCREEN BECOMES GREEN.\n\nPRESS ANY KEY TO START CALIBRATION.", false);
            //using (var audioFile = new AudioFileReader("INIT.m4a"))
            //using (var outputDevice = new WaveOutEvent())
            //outputDevice.Init(audioFile);
            //outputDevice.Play();

            //Renderer.LoadingBar(200);
            Console.ReadKey();
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