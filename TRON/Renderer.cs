using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Text;
using CustomConsole;
using Void;
using Figgle;
using Figgle.Fonts;

namespace TRON
{
    internal static class Renderer
    {
        internal static void Render()
        {
            foreach (Drawable d in Game.r.buffer)
                d.Draw();
        }
        internal static void LoadingBar(int time)
        {
            LoadingParticle[] particles = new LoadingParticle[5];
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = new LoadingParticle(new IntCoordinates() { x = (ConsoleBuffer.Width / 2 - 25) + i * 10 , y = i}, i);
            }
            while (true)
            {
                if (time == 0) return;
                string message = "LOADING...";
                //ConsoleBuffer.WriteFiggle(ConsoleBuffer.Width / 2 - message.Split('\n')[0].Length, ConsoleBuffer.Height / 2 + 5, message);
                foreach (LoadingParticle l in particles)
                    l.Tick();
                ConsoleBuffer.Draw();
                ConsoleBuffer.ResizeBuffer();
                ConsoleBuffer.Clear();
                time--;
                Thread.Sleep(20);
            }
        }
    }
}
