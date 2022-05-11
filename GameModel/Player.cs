using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameModel
{
    public class Player
    {
        public readonly int HealPoint = 100;

        public int Ammo = 15;

        public Point Location;

        public Player(Point Location)
        {
            this.Location = Location;
        }

        public void Reload()
        {
            Ammo = 15;
        }
    }
}
