using CustomConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void;

namespace TRON
{
    internal class Particle
    {
        protected IntCoordinates position;
        internal Particle(IntCoordinates position)
        {
            this.position = position;
        }
    }
    internal class LoadingParticle : Particle
    {
        private const double gravity = 0.5;
        private const double bounceStrength = 3;
        private double velocityY;
        private double y;
        private int floorY;
        private int tick;
        private int delay;
        internal LoadingParticle(IntCoordinates position, int index) : base(position)
        {
            floorY = ConsoleBuffer.Height / 2;
            y = floorY;
            velocityY = -bounceStrength;
            tick = 0;
            delay = index * 20;
        }
        internal void Tick()
        {
            velocityY += gravity;
            y += velocityY;
            if (y >= floorY)
            {
                y = floorY;
                if (tick % 100 == delay)
                    velocityY = -bounceStrength;
            }
            position.y = (int)y;
            ConsoleBuffer.Write(position.x, position.y, " _\n|_|");
            tick++;
        }
    }
}
