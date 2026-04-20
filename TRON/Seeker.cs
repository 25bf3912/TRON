using System;
using System.Collections.Generic;
using System.Text;

namespace Void
{
    internal class Seeker : Entity
    {
        private int radius;
        static Random rng;
        internal Seeker()
        {
            speed = 0.1;
            rng = new Random();
        }
        internal override void Move()
        {
            double forwardX = Math.Cos(dir);
            double forwardY = Math.Sin(dir);

            double rightX = -Math.Sin(dir);
            double rightY = Math.Cos(dir);

            double moveX = 0;
            double moveY = 0;
            switch (rng.Next(0, 3))
            {
                case 0:
                    moveX += forwardX;
                    moveY += forwardY;
                    break;
                case 1:
                    moveX -= forwardX;
                    moveY -= forwardY;
                    break;
                case 2:
                    moveX += rightX;
                    moveY += rightY;
                    break;
                case 3:
                    moveX -= rightX;
                    moveY -= rightY;
                    break;
            }
            // normalise diagonal movement
            double length = Math.Sqrt(moveX * moveX + moveY * moveY);
            if (length > 0)
            {
                moveX /= length;
                moveY /= length;
            }
            double newX = position.x + moveX * speed;
            double newY = position.y + moveY * speed;
            if (World.grid[(int)newX, (int)position.y] == 0)
                position.x = newX;

            if (World.grid[(int)position.x, (int)newY] == 0)
                position.y = newY;
        }
        internal override void Rotate()
        {
            dir += rng.NextDouble();
            dir %= (Math.PI * 2);
            if (dir < 0)
                dir += Math.PI * 2;
        }
        internal void Draw()
        {

        }
    }
}
