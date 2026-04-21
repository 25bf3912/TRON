using CustomConsole;
using System;
using System.Collections.Generic;
using System.Text;
using Void;

namespace TRON
{
    internal static class World
    {
        internal static char[,] grid { get; private set; }
        static World()
        {
            grid = new char[64, 32];
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
