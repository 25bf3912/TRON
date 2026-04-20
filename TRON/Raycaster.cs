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

namespace Void
{
    internal class Raycaster
    {
        private int resolution;
        internal Raycaster()
        {
            resolution = ConsoleBuffer.Width;
        }
        internal void Raycast(Coordinates position, double dir)
        {
            Ray r = new Ray();
            double lastDistance = 0;
            double previousJump = 0;
            for (int i = 0; i < resolution; i++)
            {
                double cameraX = 2.0 * i / resolution - 1.0;
                double rayDir = dir + cameraX * (Math.PI / 3);

                double distance = r.Cast(position, rayDir);
                if (double.IsInfinity(distance) || double.IsNaN(distance))
                    distance = 1000;
                if (i == 0) lastDistance = distance;
                int lineHeight = (int)(distance);
                int screenMidPoint = ConsoleBuffer.Height / 2;

                int startY = screenMidPoint - (lineHeight / 2);
                int endY = screenMidPoint + (lineHeight / 2);

                if (startY < 0) startY = 0;
                if (endY >= ConsoleBuffer.Height) endY = ConsoleBuffer.Height - 1;

                if (Math.Abs(previousJump - Math.Abs(distance - lastDistance)) > 0.1)
                    r.DrawVerticalLine(i, startY, endY, 125, 253, 254, true);
                else
                    r.DrawVerticalLine(i, startY, endY, 125, 253, 254);
                previousJump = Math.Abs(distance - lastDistance);
                lastDistance = distance;
            }
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
        private int side;
        private double distance;
        private Coordinates hit;

        internal Ray()
        {
            hitWall = false;
        }
        internal void DrawVerticalLine(int x, int startY, int endY, byte r, byte g, byte b, bool verticalStripe = false, char c = '█')
        {
            ConsoleBuffer.Write(x, startY, c.ToString(), r, g, b);
            if (!verticalStripe)
                for (int i = startY + 1; i < endY; i++)
                        if (side == 0)
                            ConsoleBuffer.Write(x, i, c.ToString(), 10, 10, 10);
                        else
                            ConsoleBuffer.Write(x, i, c.ToString(), 15, 15, 15);
            else
                for (int i = startY + 1; i < endY; i++)
                    ConsoleBuffer.Write(x, i, c.ToString(), r, g, b);
            ConsoleBuffer.Write(x, endY, c.ToString(), r, g, b);
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
        internal bool IsOnEdge()
        {
            const double threshold = 0.001;
            double remainderX = hit.x % 1;
            double remainderY = hit.y % 1;
            return ((remainderX < threshold || (1 - remainderX) > threshold) || (remainderY < threshold || (1 - remainderY) > threshold));
        }
    }
}