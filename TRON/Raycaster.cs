using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using CustomConsole;
using LaglessKeyboardInput;

namespace Void
{
    internal class Raycaster
    {
        private int resolution;
        static readonly (byte r, byte g, byte b)[] wallColours =
        {
            (235, 225, 170), // bright
            (210, 200, 150),
            (180, 170, 130),
            (150, 140, 110),
            (120, 110, 90),
            (90, 85, 70),
            (60, 60, 60),
            (40, 40, 40),
            (25, 25, 25),
            (10, 10, 10)     // dark
        };
        internal Raycaster()
        {
            resolution = ConsoleBuffer.Width;
        }
        internal void Render() { }
        private void VoidFog(double distance, out byte red, out byte green, out byte blue)
        {
            const double fogStartDistance = 10;
            const double fogEndDistance = 200.0;

            if (double.IsNaN(distance) || double.IsInfinity(distance))
                distance = fogEndDistance;

            distance = Math.Clamp(distance, fogStartDistance, fogEndDistance);

            double t = (distance - fogStartDistance) / (fogEndDistance - fogStartDistance);
            t = Math.Clamp(t, 0.0, 1.0);

            const double nearRed = 235, nearGreen = 225, nearBlue = 170;

            const double farRed = 10, farGreen = 10, farBlue = 10;

            red = (byte)(farRed + (nearRed - farRed) * t);
            green = (byte)(farGreen + (nearGreen - farGreen) * t);
            blue = (byte)(farBlue + (nearBlue - farBlue) * t);

            red = Math.Clamp(red, (byte)0, (byte)255);
            green = Math.Clamp(green, (byte)0, (byte)255);
            blue = Math.Clamp(blue, (byte)0, (byte)255);
        }
        private void DrawFloor()
        {
            int midHeight = ConsoleBuffer.Height / 2;
            for (int i = ConsoleBuffer.Height; i >= midHeight; i--) 
            {
                VoidFog(Math.Pow(i, 0.8), out byte red, out byte green, out byte blue);
                ConsoleBuffer.Write(0, i, new string('█', ConsoleBuffer.Width), red, green, blue);
            }
        }
        private void DrawCeiling()
        {
            int midHeight = ConsoleBuffer.Height / 2;
            for (int i = 0; i <= midHeight; i++)
            {
                int mirroredY = midHeight - i;
                VoidFog(Math.Pow(midHeight - 1, 0.99), out byte red, out byte green, out byte blue);
                ConsoleBuffer.Write(0, i, new string('█', ConsoleBuffer.Width), red, green, blue);
            }
        }
        internal void Raycast(Coordinates position, double dir)
        {
            Ray r = new Ray();
            DrawFloor();
            //DrawCeiling();
            for (int i = 0; i < resolution; i++)
            {
                double cameraX = 2.0 * i / resolution - 1.0;
                double rayDir = dir + cameraX * (Math.PI / 3);

                double distance = r.Cast(position, rayDir);
                if (double.IsInfinity(distance) || double.IsNaN(distance))
                    distance = 1000;

                int lineHeight = (int)(distance);
                int screenMidPoint = ConsoleBuffer.Height / 2;

                int startY = screenMidPoint - (lineHeight / 2);
                int endY = screenMidPoint + (lineHeight / 2);

                if (startY < 0) startY = 0;
                if (endY >= ConsoleBuffer.Height) endY = ConsoleBuffer.Height - 1;

                VoidFog(distance, out byte red, out byte green, out byte blue);
                r.DrawVerticalLine(i, startY, endY, red, green, blue);
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
        internal Ray()
        {
            hitWall = false;
        }
        internal void DrawVerticalLine(int x, int startY, int endY, byte r, byte g, byte b, char c = '█')
        {
            for (int y = startY; y <= endY; y++)
            {
                ConsoleBuffer.Write(x, y, c.ToString(), r, g, b);
            }
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
                if (World.grid[x, y] > 0)
                    hitWall = true;
            }
            if (side == 0) distance = distanceToNextXBoundary - deltaX;
            else distance = distanceToNextYBoundary - deltaY;
            double lineHeight = ConsoleBuffer.Height / distance;
            return lineHeight;
        }
    }
}