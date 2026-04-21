using CustomConsole;
using System;
using System.Collections.Generic;
using System.Text;
using Void;

namespace TRON
{
    //internal class Disc : Entity
    //{
    //    private double forwardX, forwardY;
    //    internal bool isActive { get; private set; }
    //    private int timeToLive;
    //    internal Disc()
    //    {
    //        isActive = false;
    //        timeToLive = 0;
    //        speed = 0.15;
    //    }
    //    internal void Throw(Coordinates thrower, double dir)
    //    {
    //        position = new Coordinates { x = thrower.x, y = thrower.y };
    //        forwardX = Math.Cos(dir);
    //        forwardY = Math.Sin(dir);
    //        isActive = true;
    //        this.dir = dir;
    //        timeToLive = 120;
    //    }
    //    internal override void Move()
    //    {
    //        if (!isActive) return;
    //        if (timeToLive < 1) { isActive = false; return; }
    //        double nextX = position.x + forwardX * speed;
    //        double nextY = position.y + forwardY * speed;

    //        if (World.grid[(int)nextX, (int)position.y] == '█')
    //            forwardX = -forwardX;
    //        else
    //            position.x = nextX;

    //        if (World.grid[(int)position.x, (int)nextY] == '█')
    //            forwardY = -forwardY;
    //        else
    //            position.y = nextY;
    //        timeToLive--;
    //    }
    //    internal override void Rotate()
    //    {
    //        throw new NotImplementedException();
    //    }
    //    internal double DistanceFromPlayer()
    //    {
    //        double dx = position.x - Game.player.position.x;
    //        double dy = position.y - Game.player.position.y;

    //        return Math.Sqrt(dx * dx + dy * dy);
    //    }
    //    internal override void Draw()
    //    {
    //        throw new NotImplementedException();
    //    }
    //    //internal void DrawDisc(Disc disc)
    //    //{
    //    //    if (!disc.isActive) return;

    //    //    double dx = disc.position.x - Game.player.position.x;
    //    //    double dy = disc.position.y - Game.player.position.y;
    //    //    double distToDisc = Math.Sqrt(dx * dx + dy * dy);

    //    //    double angleToDisc = Math.Atan2(dy, dx);
    //    //    double relativeAngle = angleToDisc - Game.player.dir;

    //    //    while (relativeAngle < -Math.PI) relativeAngle += 2 * Math.PI;
    //    //    while (relativeAngle > Math.PI) relativeAngle -= 2 * Math.PI;

    //    //    double fov = Math.PI / 3;
    //    //    int screenX = (int)((relativeAngle / (fov / 2)) * (resolution / 2) + (resolution / 2));

    //    //    if (screenX >= 0 && screenX < resolution)
    //    //    {
    //    //        if (distToDisc < zBuffer[screenX])
    //    //        {

    //    //            int baseHeight = (int)(ConsoleBuffer.Height / distToDisc);
    //    //            int screenMid = ConsoleBuffer.Height / 2;

    //    //            int startY = screenMid - (baseHeight / 20);
    //    //            int endY = screenMid + (baseHeight / 20);


    //    //            int halfWidth = (int)(15 / distToDisc);

    //    //            for (int xOffset = -halfWidth; xOffset <= halfWidth; xOffset++)
    //    //            {
    //    //                int targetX = screenX + xOffset;
    //    //                if (targetX >= 0 && targetX < resolution)
    //    //                {
    //    //                    if (distToDisc < zBuffer[targetX])
    //    //                    {
    //    //                        for (int y = startY; y <= endY; y++)
    //    //                        {
    //    //                            if (y >= 0 && y < ConsoleBuffer.Height)
    //    //                                ConsoleBuffer.Write(targetX, y, "█", 0, 255, 255);
    //    //                        }
    //    //                    }
    //    //                }
    //    //            }
    //    //        }
    //    //    }
    //    //}
    //}
}
