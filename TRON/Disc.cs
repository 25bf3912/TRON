using System;
using System.Collections.Generic;
using System.Text;
using Void;

namespace TRON
{
    internal class Disc : Entity
    {
        private double forwardX, forwardY;
        private bool isActive;
        private int timeToLive;
        internal Disc()
        {
            isActive = false;
            timeToLive = 0;
            speed = 0.25;
        }
        internal void Throw(Coordinates thrower, double dir)
        {
            position = new Coordinates { x = thrower.x, y = thrower.y };
            forwardX = Math.Cos(dir);
            forwardY = Math.Sin(dir);
            isActive = true;
            this.dir = dir;
            timeToLive = 300;
        }
        internal override void Move()
        {

            if (!isActive) return;
            if (timeToLive < 1) { isActive = false; return; }
            double nextX = position.x + forwardX * speed;
            double nextY = position.y + forwardY * speed;

            // Check for wall collision on the X axis
            if (World.grid[(int)nextX, (int)position.y] == '█')
                forwardX = -forwardX;
            else
                position.x = nextX;

            if (World.grid[(int)position.x, (int)nextY] == '█')
                forwardY = -forwardY;
            else
                position.y = nextY;
            timeToLive--;
        }
        internal override void Rotate()
        {
            throw new NotImplementedException();
        }
    }
}
