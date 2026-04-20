using System;
using System.Collections.Generic;
using System.Text;

namespace Void
{
    internal struct Coordinates
    {
        public double x, y;
    }
    internal abstract class Entity
    {
        internal Coordinates position;
        internal double dir;
        protected double speed;
        internal abstract void Move();
        internal abstract void Rotate();
        protected bool IsInWall() => World.grid[(int)position.x, (int)position.y] == '█';
    }
}