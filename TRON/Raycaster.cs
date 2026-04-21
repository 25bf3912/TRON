using CustomConsole;
using LaglessKeyboardInput;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Web;
using TRON;

namespace Void
{
    internal class Raycaster
    {
        private int resolution;
        internal List<Drawable> buffer;
        internal Raycaster()
        {
            resolution = ConsoleBuffer.Width;
            buffer = new List<Drawable>();
        }
        internal void Raycast(Coordinates position, double dir)
        {
            Ray r = new Ray();
            double lastLineHeight = 0;
            double previousJump = 0;
            buffer.Clear();
            for (int i = 0; i < resolution; i++)
            {
                double cameraX = 2.0 * i / resolution - 1.0;
                double rayDir = dir + cameraX * (Math.PI / 3);

                double lineHeight = r.Cast(position, rayDir);

                double distance = (double)ConsoleBuffer.Height / lineHeight;

                if (double.IsInfinity(lineHeight) || double.IsNaN(lineHeight))
                    lineHeight = 1000;

                if (i == 0) lastLineHeight = lineHeight;

                int screenMidPoint = ConsoleBuffer.Height / 2;
                int startY = screenMidPoint - ((int)lineHeight / 2);
                int endY = screenMidPoint + ((int)lineHeight / 2);

                if (startY < 0) startY = 0;

                if (endY >= ConsoleBuffer.Height) endY = ConsoleBuffer.Height - 1;

                if (Math.Abs(previousJump - Math.Abs(lineHeight - lastLineHeight)) > 0.2)
                    buffer.Add(new Wall((125, 253, 254), startY, endY, distance, new Coordinates(i, 0)));
                else
                    if (r.side == 0)
                        buffer.Add(new Wall((10, 10, 10), startY, endY, distance, new Coordinates(i, 0)));
                    else
                        buffer.Add(new Wall((15, 15, 15), startY, endY, distance, new Coordinates(i, 0)));
                previousJump = Math.Abs(lineHeight - lastLineHeight);
                lastLineHeight = lineHeight;
            }
            foreach (Entity e in Game.allEntities)
                buffer.Add(e);
            SortBuffer();
        }
        internal void SortBuffer()
        {
            buffer.Sort();
            buffer.Reverse();
        }
    }
    internal class Ray
    {
        private bool hitWall;
        private Coordinates position;
        private double dir;
        private int stepX;
        private int stepY;
        private double distanceToNextXBoundary;
        private double distanceToNextYBoundary;
        private double deltaX;
        private double deltaY;
        private int x;
        private int y;
        internal int side;
        private double distance;
        private Coordinates hit;

        internal Ray()
        {
            hitWall = false;
        }
        private void CalculateValuesForCasting(Coordinates position, double dir)
        {
            this.position = position;
            this.dir = dir;
            hitWall = false;
            double nextGridX;
            double nextGridY;
            double rayDirX = Math.Cos(dir);
            double rayDirY = Math.Sin(dir);
            x = (int)position.x;
            y = (int)position.y;

            if (rayDirX < 0) stepX = -1;
            else stepX = 1;
            if (rayDirY < 0) stepY = -1;
            else stepY = 1;

            if (rayDirX > 0) nextGridX = x + 1;
            else nextGridX = x;
            if (rayDirY > 0) nextGridY = y + 1;
            else nextGridY = y;

            if (rayDirX == 0) distanceToNextXBoundary = double.PositiveInfinity;
            else distanceToNextXBoundary = (nextGridX - position.x) / rayDirX;

            if (rayDirY == 0) distanceToNextYBoundary = double.PositiveInfinity;
            else distanceToNextYBoundary = (nextGridY - position.y) / rayDirY;

            if (rayDirX == 0) deltaX = double.PositiveInfinity;
            else deltaX = Math.Abs(1 / rayDirX);

            if (rayDirY == 0) deltaY = double.PositiveInfinity;
            else deltaY = Math.Abs(1 / rayDirY);
        }
        internal double Cast(Coordinates position, double dir)
        {
            CalculateValuesForCasting(position, dir);
            while (!hitWall)
            {
                if (distanceToNextXBoundary < distanceToNextYBoundary)
                {
                    x += stepX;
                    distanceToNextXBoundary += deltaX;
                    side = 0;
                }
                else
                {
                    y += stepY;
                    distanceToNextYBoundary += deltaY;
                    side = 1;
                }
                if (World.grid[x, y] == '█')
                    hitWall = true;
            }
            if (side == 0) distance = distanceToNextXBoundary - deltaX;
            else distance = distanceToNextYBoundary - deltaY;

            hit.x = this.position.x + distance * Math.Cos(dir);
            hit.y = this.position.y + distance * Math.Sin(dir);

            double lineHeight = ConsoleBuffer.Height / distance;
            return lineHeight;
        }
    }
}