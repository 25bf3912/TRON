using CustomConsole;
using System;
using System.Collections.Generic;
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
            grid = new char[2048, 2048];
            rng = new Random();
            GenerateGrid();
            GenerateOutline();
            Game.loading = false;
        }
        internal static async Task<bool> GenerateGrid()
        {
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    grid[i, j] = '█';
            IntCoordinates start = new IntCoordinates()
            {
                x = rng.Next(1, width - 1),
                y = rng.Next(1, height - 1)
            };
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
            GenerateOutline();
            return true;
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
            ConsoleBuffer.Write((int)Game.allEntities[0].position.x, (int)Game.allEntities[0].position.y, "█", 0, 255, 0);
        }
        internal static void DrawHud()
        {
            DrawCrosshair();
            //DrawMinimap();
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
