using System;
using System.Collections.Generic;
using System.Text;
using TRON;

namespace Void
{
    internal struct Coordinates
    {
        internal double x, y;
        internal Coordinates(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
    internal struct IntCoordinates
    {
        internal int x, y;
        internal IntCoordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    internal abstract class Entity : Drawable
    {
        internal Coordinates position;
        internal double dir;
        protected double speed;

        internal Entity(double distance, Coordinates positionToDraw) : base(distance, positionToDraw)
        {
        }

        internal abstract void Move();
        internal abstract void Rotate();
        protected bool IsInWall() => World.grid[(int)position.x, (int)position.y] == '█';
    }
}