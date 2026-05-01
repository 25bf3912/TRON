using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
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
        internal bool fat { get; private set; }
        internal Menu(int x, int y, int width, int height, (byte red, byte green, byte blue) colour, string text, bool type)
        {
            if (width < 2 || height < 2)
                throw new ArgumentException("Width and height must be at least 2.");
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.colour = colour;
            this.text = text;
            this.fat = fat;
        }
        internal void DrawBox()
        {
            if (fat)
            {
                ConsoleBuffer.Write(x, y, new string('█', width), colour.red, colour.green, colour.blue);
                for (int i = 0; i < height - 1; i++)
                {
                    ConsoleBuffer.Write(x, y + i, new string('█', 1), colour.red, colour.green, colour.blue);
                    ConsoleBuffer.Write(x + width - 1, y + i, "█", colour.red, colour.green, colour.blue);
                }
                    ConsoleBuffer.Write(x, y + height - 1, new string('█', width), colour.red, colour.green, colour.blue);
            }
            else
            {
                ConsoleBuffer.Write(x, y, "┏" + new string('━', width - 2) + "┓", colour.red, colour.green, colour.blue);
                for (int i = 1; i < height - 1; i++)
                {
                    ConsoleBuffer.Write(x, y + i, "┃" + new string(' ', width - 2) + "┃", colour.red, colour.green, colour.blue);
                }
                ConsoleBuffer.Write(x, y + height - 1, "┗" + new string('━', width - 2) + "┛", colour.red, colour.green, colour.blue);
            }
        }
        internal void DrawBox(bool b)
        {
            
        }
        internal void Display()
        {
            DrawBox();

            if (string.IsNullOrEmpty(text))
                return;

            string[] lines = text.Split('\n');

            int startY = y + 1;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (line.Length > width - 2)
                    line = line.Substring(0, width - 2);

                int textX = x + (width - line.Length) / 2;
                int textY = startY + i;

                if (textY >= y + height - 1)
                    break;

                ConsoleBuffer.Write(textX, textY, line, colour.red, colour.green, colour.blue);
            }
        }
    }
}