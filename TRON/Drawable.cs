using System;
using System.Collections.Generic;
using System.Text;
using Void;

namespace TRON
{
    internal abstract class Drawable : IComparable<Drawable>
    {
        protected double distance { get; set; }
        protected Coordinates positionToDraw;
        internal Drawable(double distance, Coordinates positionToDraw)
        {
            this.distance = distance;
            this.positionToDraw = positionToDraw;
        }
        internal abstract void Draw();
        int IComparable<Drawable>.CompareTo(Drawable? d)
        {
            if (d == null) return 1;
            return distance.CompareTo(d.distance);
        }
    }
}