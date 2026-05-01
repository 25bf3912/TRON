using CustomConsole;
using System;
using System.Collections.Generic;
using System.Text;
using Void;

namespace TRON
{
    internal class Recognizer : Entity
    {
        private (byte red, byte green, byte blue) colour;
        static Random rng = new Random();
        public Recognizer((byte red, byte green, byte blue) colour, Coordinates position, double distance, Coordinates positionToDraw) : base(distance, positionToDraw)
        {
            this.colour = colour;
            this.position = position;
            dir = 0;
            speed = 0.025;
        }
        internal override void Draw()
        {
            int height = (int)(ConsoleBuffer.Height / distance);
            int width = (int)(height * 2.5);
            int startY = Math.Max(0, (ConsoleBuffer.Height / 2) - (height / 2));
            int endY = Math.Min(ConsoleBuffer.Height - 1, startY + height);
            int leftX = (int)positionToDraw.x - width / 2;
            int rightX = (int)positionToDraw.x + width / 2;
            if (distance < 0.1) return;

            for (int column = leftX; column <= rightX; column++)
            {
                if (column < 0 || column >= ConsoleBuffer.Width) continue;
                bool isEdge = column == leftX || column == rightX;
                ConsoleBuffer.Write(column, startY, "█", 180, 40, 40);
                for (int i = startY + 1; i < endY; i++)
                    if (isEdge)
                        ConsoleBuffer.Write(column, i, "█", 180, 40, 40);
                    else
                        ConsoleBuffer.Write(column, i, "█", 50, 50, 50);
                ConsoleBuffer.Write(column, endY, "█", 180, 40, 40);
            }
        }
        internal override void Move()
        {
            double forwardX = Math.Cos(dir);
            double forwardY = Math.Sin(dir);

            double moveX = 0;
            double moveY = 0;

            moveX += forwardX; // always move forwards
            moveY += forwardY;

            // normalise diagonal movement
            double length = Math.Sqrt(moveX * moveX + moveY * moveY);
            if (length > 0)
            {
                moveX /= length;
                moveY /= length;
            }
            double newX = position.x + moveX * speed;
            double newY = position.y + moveY * speed;

            if (World.grid[(int)newX, (int)position.y] == ' ')
                position.x = newX;

            if (World.grid[(int)position.x, (int)newY] == ' ')
                position.y = newY;

            double dx = Game.player.position.x - position.x;
            double dy = Game.player.position.y - position.y;

            double angleToEnemy = Math.Atan2(position.y - Game.player.position.y,
                                  position.x - Game.player.position.x);
            double relativeAngle = angleToEnemy - Game.player.dir;
            distance = Math.Cos(relativeAngle) * Math.Sqrt(dx * dx + dy * dy);

            // wrap angle
            while (relativeAngle > Math.PI) relativeAngle -= 2 * Math.PI;
            while (relativeAngle < -Math.PI) relativeAngle += 2 * Math.PI;

            positionToDraw.x = (ConsoleBuffer.Width / 2) + (int)(relativeAngle * (ConsoleBuffer.Width / 2));
        }
        internal override void Rotate()
        {
            dir = Math.Atan2(Game.player.position.y - position.y, Game.player.position.x - position.x);
        }
    }
}