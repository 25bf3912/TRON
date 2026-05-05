using CustomConsole;
using LaglessKeyboardInput;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Text;
using NAudio.Wave;
using TRON;
using System.Security.Cryptography;

namespace Void
{
    internal static class Game
    {
        internal static int tick { get; private set; }
        private static int desiredFps;
        private static int pause;
        internal static bool isRunning { get; private set; }
        internal static List<string> currentKeystrokes;
        internal static Player player;
        internal static Raycaster r { get; private set; }
        private static Stopwatch timer;
        internal static List<Entity> allEntities { get; private set; }
        internal static bool loading;
        internal static IntCoordinates start;
        static Game()
        {
            isRunning = true;
            tick = 0;
            desiredFps = 60;
            pause = 1000 / desiredFps;
            currentKeystrokes = new List<string>();
            player = new Player(0, new Coordinates() { x = (int)start.x, y = (int)start.y });
            r = new Raycaster();
            timer = new Stopwatch();
            allEntities = new List<Entity>();
            AddEntites();
            loading = true;
        }
        private static void AddEntites(int count = 32)
        {
            Random rng = new Random();
            int x, y;
            x = 1; 
            y = 1;
            for (int i = 0; i < count; i++)
            {bool validPosition = false;
                while (!validPosition)
                {
                    x = rng.Next(1, World.grid.GetLength(0) - 2);
                    y = rng.Next(1, World.grid.GetLength(1) - 2);
                    if (World.grid[x, y] == ' ') validPosition = true;
                }
                allEntities.Add(new Recognizer((20, 20, 20), new Coordinates(x, y), 0, new Coordinates(0, 0)));
            }
        }
        internal static void Tick()
        {
            timer.Restart();

            Keyboard.CurrentKey(out currentKeystrokes);
            if (!isRunning)
                return;

            player.Move();
            player.Rotate();
            //player.TakeInputs();
            foreach (Entity e in allEntities)
            {
                e.Rotate();
                e.Move();
                if (e.IsColliding(player))
                    Environment.Exit(0);
            }
            //player.disc.Move();
            r.Raycast(player.position, player.dir);
            Renderer.Render();
            CheckForActionKeys();
            World.DrawHud();
            ConsoleBuffer.Draw();
            tick++;

            timer.Stop();

            int elapsedTime = (int)timer.ElapsedMilliseconds;
            int sleepTime = pause - elapsedTime;

            if (sleepTime > 0)
                Thread.Sleep(sleepTime);
            ConsoleBuffer.ResizeBuffer();
            ConsoleBuffer.Clear();
        }
        private static void CheckForActionKeys()
        {
            //if (currentKeystrokes.Contains("ESCAPE"))
            //{ isRunning = !isRunning; Thread.Sleep(50); Console.ReadKey(); }
            if (currentKeystrokes.Contains("R"))
                World.GenerateGrid(out Game.start);
            if (currentKeystrokes.Contains("TAB"))
            { ConsoleBuffer.Clear(); World.DrawMinimap(); Thread.Sleep(50); Console.ReadKey(); }
        }
    }
}
