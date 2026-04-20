using System;
using System.Collections.Generic;
using System.Text;
using Void;

namespace TRON
{
    internal class Disc : Entity
    {
        private double forwardX, forwardY;
        internal bool isActive { get; private set; }
        private int timeToLive;
        internal Disc()
        {
            isActive = false;
            timeToLive = 0;
            speed = 0.15;
        }
        internal void Throw(Coordinates thrower, double dir)
        {
            position = new Coordinates { x = thrower.x, y = thrower.y };
            forwardX = Math.Cos(dir);
            forwardY = Math.Sin(dir);
            isActive = true;
            this.dir = dir;
            timeToLive = 120;
        }
        internal override void Move()
        {
            if (!isActive) return;
            if (timeToLive < 1) { isActive = false; return; }
            double nextX = position.x + forwardX * speed;
            double nextY = position.y + forwardY * speed;

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
        internal double DistanceFromPlayer()
        {
            double dx = position.x - Game.player.position.x;
            double dy = position.y - Game.player.position.y;

            return Math.Sqrt(dx * dx + dy * dy);
        }

    }
}
