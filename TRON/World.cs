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
            grid = new char[64, 32];
            rng = new Random();
            GenerateGrid();
            GenerateOutline();
        }
        internal static bool GenerateGrid()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                    grid[i, j] = '█'; // fill

            // maze generation algorithm
            bool[,] gridVisited = new bool[grid.GetLength(0), grid.GetLength(1)];
            IntCoordinates node = new IntCoordinates() { x = rng.Next(grid.GetLength(0)), y = rng.Next(grid.GetLength(1)) };
            Queue<IntCoordinates> nodes = new Queue<IntCoordinates>();
            while (true)
            {
                DrawMinimap();
                gridVisited[node.x, node.y] = true;
                nodes.Enqueue(node);
                IntCoordinates? next = GetAvailableAdjacentCell(node, gridVisited);
                while (next == null)
                {
                    if (!nodes.TryPeek(out IntCoordinates i)) return true;
                    node = nodes.Dequeue();
                    next = GetAvailableAdjacentCell(node, gridVisited);
                }
                grid[node.x, node.y] = ' ';
                node = (IntCoordinates)next;
            }
        }
        private static void GenerateOutline()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                    if (i == 0 || j == 0 || i == grid.GetLength(0) - 1 || j == grid.GetLength(1) - 1)
                        grid[i, j] = '█'; // surround by square wall
        }
        private static IntCoordinates? GetAvailableAdjacentCell(IntCoordinates node, bool[,] gridVisited)
        {
            List<IntCoordinates?> availableNodes = new List<IntCoordinates?>();
            try
            {
                if (!gridVisited[node.x + 1, node.y])
                    availableNodes.Add(new IntCoordinates() { x = node.x + 1, y = node.y });
            } catch (Exception) { };
            try
            {
                if (!gridVisited[node.x - 1, node.y])
                    availableNodes.Add(new IntCoordinates() { x = node.x - 1, y = node.y });
            } catch (Exception) { };
            try
            {
                if (!gridVisited[node.x + 1, node.y + 1])
                    availableNodes.Add(new IntCoordinates() { x = node.x + 1, y = node.y + 1 });
            } catch (Exception) { };
            try
            {
                if (!gridVisited[node.x + 1, node.y - 1])
                    availableNodes.Add(new IntCoordinates() { x = node.x + 1, y = node.y - 1 });
            } catch (Exception) { };
            try
            {
                if (!gridVisited[node.x - 1, node.y - 1])
                    availableNodes.Add(new IntCoordinates() { x = node.x - 1, y = node.y - 1 });
            } catch (Exception) { };
            try
            {
                if (!gridVisited[node.x, node.y - 1])
                    availableNodes.Add(new IntCoordinates() { x = node.x, y = node.y - 1 });
            } catch (Exception) { };
            try
            {
                if (!gridVisited[node.x, node.y + 1])
                    availableNodes.Add(new IntCoordinates() { x = node.x, y = node.y + 1 });
            } catch (Exception) { };
            try
            {
                if (!gridVisited[node.x + 1, node.y])
                    availableNodes.Add(new IntCoordinates() { x = node.x - 1, y = node.y + 1 });
            } catch (Exception) { };
            if (availableNodes.Count() == 0) return null;
            return availableNodes[rng.Next(availableNodes.Count())];
            throw new NotImplementedException();
        }
        internal static void DrawMinimap()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                    ConsoleBuffer.Write(i, j, new string(grid[i, j], 2), 255, 255, 255);
            ConsoleBuffer.Write((int)Game.player.position.x, (int)Game.player.position.y, "█", 255, 0, 0);
            ConsoleBuffer.Write((int)Game.allEntities[0].position.x, (int)Game.allEntities[0].position.x, "█", 0, 255, 0);
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
