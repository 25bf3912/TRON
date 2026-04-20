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
        private static Raycaster r;
        private static Stopwatch timer;
        private static List<Entity> allEntities;
        static Game()
        {
            isRunning = true;
            tick = 0;
            desiredFps = 60;
            pause = 1000 / desiredFps;
            currentKeystrokes = new List<string>();
            player = new Player();
            r = new Raycaster();
            timer = new Stopwatch();
            allEntities = new List<Entity>();
        }
        internal static void Tick()
        {
            timer.Restart();

            Keyboard.CurrentKey(out currentKeystrokes);
            CheckForActionKeys();

            if (!isRunning)
                return;

            player.Move();
            player.Rotate();
            player.TakeInputs();
            foreach (Entity e in allEntities)
            {
                e.Move();
            }
            player.disc.Move();
            r.Raycast(player.position, player.dir);
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
            if (currentKeystrokes.Contains("ESCAPE"))
                isRunning = !isRunning;
            if (currentKeystrokes.Contains("R"))
                World.GenerateGrid();
        }
    }
    internal static class World
    {
        internal static char[,] grid { get; private set; }
        static World()
        {
            grid = new char[64,32];
            GenerateGrid();
        }
        internal static void GenerateGrid()
        {
            Random rng = new Random();

            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                    if (i == 0 || j == 0 || i == grid.GetLength(0) - 1 || j == grid.GetLength(1) - 1)
                        grid[i, j] = '█';
                    else
                        grid[i, j] = rng.NextDouble() < 0.25 ? '█' : ' ';
            grid[1, 1] = ' ';
        }
        internal static void DrawMinimap()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                    ConsoleBuffer.Write(i, j, new string(grid[i, j], 2), 255, 255, 255);
            ConsoleBuffer.Write((int)Game.player.position.x, (int)Game.player.position.y, "█", 255, 0, 0);
            ConsoleBuffer.Write((int)Game.player.disc.position.x, (int)Game.player.disc.position.y, "█", 0, 255, 0);
        }
        internal static void DrawHud()
        {
            DrawCrosshair();
            DrawMinimap();
        }
        private static void DrawCrosshair()
        {
            int centerX = ConsoleBuffer.Width / 2;
            int centerY = ConsoleBuffer.Height / 2;
            int sizeX = 5;
            int sizeY = 2;

            for (int x = centerX - sizeX + 1; x <= centerX + sizeX; x++)
            {
                if (x == centerX || x == centerX + 1) continue;
                ConsoleBuffer.Write(x, centerY, "█", 255, 255, 255);
            }
            for (int y = centerY - sizeY; y <= centerY + sizeY; y++)
            {
                if (y == centerY) continue;
                ConsoleBuffer.Write(centerX, y, "██", 255, 255, 255);
            }
        }
    }
    
}
