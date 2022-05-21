using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameModel
{
    public class Player
    {
        public readonly int HealPoint = 100;

        private int Ammo = 15;

        public Point Location;

        public Player(Point Location)
        {
            this.Location = Location;
        }

        public int ShowAmmo() => Ammo;
        public void SpendAmmo() => --Ammo; 

        public void Reload()
        {
            Ammo = 15;
        }
    }
}
