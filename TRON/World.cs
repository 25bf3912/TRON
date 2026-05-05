using CustomConsole;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text;
using Void;

namespace TRON
{
    internal static class World
    {
        internal static char[,] grid { get; private set; }
        static Random rng;
        static World()
        {
            grid = new char[128, 128];
            rng = new Random();
            GenerateGrid(out Game.start);
            Game.loading = false;
        }
        internal static bool GenerateGrid(out IntCoordinates start)
        {
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    grid[i, j] = '█';
            start.x = 1;
            start.y = 1;
            start.x = start.x % 2 == 0 ? start.x - 1 : start.x;
            start.y = start.y % 2 == 0 ? start.y - 1 : start.y;
            bool[,] visited = new bool[width, height];
            Stack<IntCoordinates> stack = new Stack<IntCoordinates>();
            visited[start.x, start.y] = true;
            grid[start.x, start.y] = ' ';
            stack.Push(start);
            while (stack.Count > 0)
            {
                IntCoordinates current = stack.Peek();
                List<IntCoordinates> neighbors = GetUnvisitedNeighbors(current, visited);
                if (neighbors.Count == 0)
                {
                    stack.Pop();
                    continue;
                }
                IntCoordinates next = neighbors[rng.Next(neighbors.Count)];
                int wallX = (current.x + next.x) / 2;
                int wallY = (current.y + next.y) / 2;
                grid[wallX, wallY] = ' ';
                grid[next.x, next.y] = ' ';
                visited[next.x, next.y] = true;
                stack.Push(next);
            }
            grid[width - 3, height - 3] = 'X';
            DrawRoom(8, 0, true);
            for (int i = 0; i < 128; i++)
                DrawRoom(rng.Next(8));
            for (int i = 0; i < 32; i++)
                DrawRoom(rng.Next(16));
            for (int i = 0; i < 8; i++)
                DrawRoom(rng.Next(32));
            for (int i = 0; i < 16; i++)
                DrawRoom(rng.Next(8), 0);
            GenerateOutline();
            return true;
        }
        internal static void DrawRoom(int size = 10, double density = 0.3, bool startingRoom = false)
        {
            int x, y;
            if (!startingRoom)
{               x = rng.Next(1, grid.GetLength(0) - 2 - size);
                y = rng.Next(1, grid.GetLength(1) - 2 - size);
            }
            else { x = 1; y = 1; }
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    grid[x + i, y + j] = ' ';
                    if (rng.NextDouble() < density)
                        grid[x + i, y + j] = '█';
                }
        }
        private static List<IntCoordinates> GetUnvisitedNeighbors(IntCoordinates node, bool[,] visited)
        {
            int width = visited.GetLength(0);
            int height = visited.GetLength(1);
            List<IntCoordinates> neighbors = new List<IntCoordinates>();
            int[,] directions = new int[,]
            {
        { 2, 0 },
        { -2, 0 },
        { 0, 2 },
        { 0, -2 }
            };
            for (int i = 0; i < 4; i++)
            {
                int tempX = node.x + directions[i, 0];
                int tempY = node.y + directions[i, 1];

                if (tempX > 0 && tempY > 0 && tempX < width - 1 && tempY < height - 1)
                {
                    if (!visited[tempX, tempY])
                    {
                        neighbors.Add(new IntCoordinates() { x = tempX, y = tempY });
                    }
                }
            }
            return neighbors;
        }
        internal static void DrawMinimap()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                    ConsoleBuffer.Write(i, j, new string(grid[i, j], 2), 255, 255, 255);
            ConsoleBuffer.Write((int)Game.player.position.x, (int)Game.player.position.y, "█", 255, 0, 0);
            foreach (Entity e in Game.allEntities)
                ConsoleBuffer.Write((int)e.position.x, (int)e.position.y, "█", 0, 255, 0);
        }
        internal static void DrawHud()
        {
            DrawCrosshair();
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
        private static void GenerateOutline()
        {
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);

            for (int i = 0; i < width; i++)
            {
                grid[i, 0] = '█';
                grid[i, height - 1] = '█';
            }

            for (int j = 0; j < height; j++)
            {
                grid[0, j] = '█';
                grid[width - 1, j] = '█';
            }
        }
    }
}
