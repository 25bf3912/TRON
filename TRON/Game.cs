using CustomConsole;
using LaglessKeyboardInput;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Text;
using NAudio.Wave;
using TRON;

namespace Void
{
    internal static class Game
    {
        internal static int tick { get; private set; }
        private static int desiredFps;
        private static int pause;
        internal static bool isRunning { get; private set; }
        internal static List<string> currentKeystrokes;
        internal static Player player;
        internal static Raycaster r { get; private set; }
        private static Stopwatch timer;
        internal static List<Entity> allEntities { get; private set; }
        internal static bool loading;
        static Game()
        {
            isRunning = true;
            tick = 0;
            desiredFps = 60;
            pause = 1000 / desiredFps;
            currentKeystrokes = new List<string>();
            player = new Player();
            r = new Raycaster();
            timer = new Stopwatch();
            allEntities = new List<Entity>();
            allEntities.Add(new Recognizer((20, 20, 20), new Coordinates(5, 5), 0, new Coordinates(0, 0)));
            loading = true;
        }
        internal static void Tick()
        {
            timer.Restart();

            Keyboard.CurrentKey(out currentKeystrokes);
            CheckForActionKeys();

            if (!isRunning)
                return;

            player.Move();
            player.Rotate();
            //player.TakeInputs();
            foreach (Entity e in allEntities)
            {
                e.Rotate();
                e.Move();
            }
            //player.disc.Move();
            r.Raycast(player.position, player.dir);
            Renderer.Render();
            World.DrawHud();
            ConsoleBuffer.Draw();
            tick++;

            timer.Stop();

            int elapsedTime = (int)timer.ElapsedMilliseconds;
            int sleepTime = pause - elapsedTime;

            if (sleepTime > 0)
                Thread.Sleep(sleepTime);
            ConsoleBuffer.ResizeBuffer();
            ConsoleBuffer.Clear();
        }
        private static void CheckForActionKeys()
        {
            if (currentKeystrokes.Contains("ESCAPE"))
                isRunning = !isRunning;
            if (currentKeystrokes.Contains("R"))
                World.GenerateGrid();
        }
    }
}
