﻿using System;
using System.Drawing;

namespace GameModel
{
    public class Bullet
    {
        private readonly int Speed = 20;
        private Point Start;
        private Point End;
        public Point Location;
        public Image TypeBullet;
        public bool stop;

        public Bullet(Point start, Point end)
        {
            Start = start;
            End = end;
            Location = Start;
        }

        public void Move()
        {
            if(!stop)
            {
                var distanceX = End.X - Location.X;
                var distanceY = End.Y - Location.Y;
                var vector = Math.Sqrt((End.X - Location.X) * (End.X - Location.X) +
                                       (End.Y - Location.Y) * (End.Y - Location.Y));
                var newX = distanceX / vector * Speed + Location.X;
                var newY = distanceY / vector * Speed + Location.Y;
                var newPos = new Point((int) newX, (int) newY);
                Location = newPos;
            }
            if (Math.Abs(End.X - Location.X) <= 10 && Math.Abs(End.Y - Location.Y) <= 10) 
                stop = true;
            
        }
    }
}
