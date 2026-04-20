using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using LaglessKeyboardInput;

namespace Void
{
    internal class Player : Entity
    {
        internal Player()
        {
            speed = 0.1;
            position.x = 2;
            position.y = 2;
        }
        internal override void Move()
        {
            if (Game.currentKeystrokes.Count == 0)
                return;

            double forwardX = Math.Cos(dir);
            double forwardY = Math.Sin(dir);

            double rightX = -Math.Sin(dir);
            double rightY = Math.Cos(dir);

            double moveX = 0;
            double moveY = 0;

            if (Game.currentKeystrokes.Contains("W"))
            {
                moveX += forwardX;
                moveY += forwardY;
            }
            if (Game.currentKeystrokes.Contains("S"))
            {
                moveX -= forwardX;
                moveY -= forwardY;
            }
            if (Game.currentKeystrokes.Contains("D"))
            {
                moveX += rightX;
                moveY += rightY;
            }
            if (Game.currentKeystrokes.Contains("A"))
            {
                moveX -= rightX;
                moveY -= rightY;
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
            if (Game.currentKeystrokes.Contains("LEFT_ARROW"))
                dir -= Double.DegreesToRadians(5);
            if (Game.currentKeystrokes.Contains("RIGHT_ARROW"))
                dir += Double.DegreesToRadians(5);
            dir %= (Math.PI * 2);
            if (dir < 0)
                dir += Math.PI * 2;
        }
    }
}