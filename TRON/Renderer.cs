using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Text;
using CustomConsole;
using Void;

namespace TRON
{
    internal static class Renderer
    {
        internal static void Render()
        {
            foreach (Drawable d in Game.r.buffer)
                d.Draw();
        }
    }
}
