using CustomConsole;
using LaglessKeyboardInput;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Text;

namespace Void
{
    internal static class Game
    {
        private static int tick;
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
            foreach (Entity e in allEntities)
            {
                e.Move();
            }

            r.Raycast(player.position, player.dir);

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
        }
    }
    internal static class World
    {
        internal static int[,] grid { get; private set; }
        static World() 
        {
            grid = GenerateGrid();
        }
        static int[,] GenerateGrid()
        {
            int size = 64;
            int[,] grid = new int[size, size];

            // fill walls
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    grid[y, x] = 1;

            void Carve(int x, int y, int w, int h)
            {
                if (w < 4 || h < 4) return;

                bool horizontal = w < h;

                if (horizontal)
                {
                    int wallY = y + 2 + Random.Shared.Next(h - 3);
                    int passageX = x + 1 + Random.Shared.Next(w - 2);

                    for (int i = x; i < x + w; i++)
                        grid[wallY, i] = 1;

                    grid[wallY, passageX] = 0;

                    Carve(x, y, w, wallY - y);
                    Carve(x, wallY + 1, w, y + h - wallY - 1);
                }
                else
                {
                    int wallX = x + 2 + Random.Shared.Next(w - 3);
                    int passageY = y + 1 + Random.Shared.Next(h - 2);

                    for (int i = y; i < y + h; i++)
                        grid[i, wallX] = 1;

                    grid[passageY, wallX] = 0;

                    Carve(x, y, wallX - x, h);
                    Carve(wallX + 1, y, x + w - wallX - 1, h);
                }
            }

            // open starting area
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    grid[y, x] = 0;

            Carve(0, 0, size, size);

            // border walls
            for (int i = 0; i < size; i++)
            {
                grid[0, i] = 1;
                grid[size - 1, i] = 1;
                grid[i, 0] = 1;
                grid[i, size - 1] = 1;
            }

            return grid;
        } // NEED TO REPLACE THIS
    }
    
}
