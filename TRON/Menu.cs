using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CustomConsole;

namespace Void
{
    internal class Menu
    {
        internal int x { get; private set; }
        internal int y { get; private set; }
        internal int height { get; private set; }
        internal int width { get; private set; }
        internal (byte red, byte green, byte blue) colour { get; private set; }
        internal string text { get; private set; }
        internal Menu(int x, int y, int width, int height, (byte red, byte green, byte blue) colour, string text)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.colour = colour;
            this.text = text;
        }
        internal void DrawBox()
        {
            ConsoleBuffer.Write(x, y, new string('█', width), colour.red, colour.green, colour.blue);
            for (int i = 0; i < height - 1; i++)
            {
                ConsoleBuffer.Write(x, y + i, new string('█', 1), colour.red, colour.green, colour.blue);
                ConsoleBuffer.Write(x + width - 1, y + i, new string('█', 1), colour.red, colour.green, colour.blue);
            }
            ConsoleBuffer.Write(x, y + height - 1, new string('█', width), colour.red, colour.green, colour.blue);
        }
    }
}