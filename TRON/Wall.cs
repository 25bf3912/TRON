using CustomConsole;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Void;

namespace TRON
{
    internal class Wall : Drawable
    {
        private (byte red, byte green, byte blue) colour;
        private int startY, endY;
        internal Wall((byte red, byte green, byte blue) colour, int startY, int endY, double distance, Coordinates positionToDraw) : base(distance, positionToDraw)
        {
            this.colour = colour;
            this.startY = startY;
            this.endY = endY;
        }
        internal override void Draw()
        {
            ConsoleBuffer.Write((int)positionToDraw.x, startY, "█", 125, 253, 254);
            for (int i = startY + 1; i < endY; i++)
                ConsoleBuffer.Write((int)positionToDraw.x, i, "█", colour.red, colour.green, colour.blue);
            ConsoleBuffer.Write((int)positionToDraw.x, endY, "█", 125, 253, 254);
        }
    }
}
