using System.Drawing;
using GameModel;

namespace Game
{
    class Wall : IObject
    {
        public bool CheckOnDeath(IObject conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public Bitmap GetImage() => Resource.Beton;
    }
    class Shards : IObject
    {
        public bool CheckOnDeath(IObject conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public Bitmap GetImage() => Resource.Shards;
    }
    class Blood : IObject
    {
        public bool CheckOnDeath(IObject conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public Bitmap GetImage() => Resource.Blood;
    }
    class WallUp : IObject
    {
        public bool CheckOnDeath(IObject conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public Bitmap GetImage() => Resource.BetonUp;
    }
    class WallDown : IObject
    {
        public bool CheckOnDeath(IObject conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public Bitmap GetImage() => Resource.BetonDown;
    }
    class WallRight : IObject
    {
        public bool CheckOnDeath(IObject conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public Bitmap GetImage() => Resource.BetonRight;
    }
    class WallLeft : IObject
    {
        public bool CheckOnDeath(IObject conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public Bitmap GetImage() => Resource.BetonLeft;
    }
    class Glass : IObject
    {
        public bool CheckOnDeath(IObject conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public Bitmap GetImage() => Resource.Glass;
    }
    class Door : IObject
    {
        public bool CheckOnDeath(IObject conflictedObject)
        {
            return conflictedObject == this;
        }

        public int GetDrawingPriority()
        {
            return 4;
        }

        public Bitmap GetImage()
        {
            return  Resource.Door;
        }
    }
    class Exit : IObject
    {
        public bool CheckOnDeath(IObject conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 4;
        }

        public Bitmap GetImage()
        {
           return Resource.Exit;
        }
    }
    class ZoneEnemy : IObject
    {
        public bool CheckOnDeath(IObject conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public Bitmap GetImage()
        {
            return Resource.Floor1;
        }
    }
    class Medkit : IObject
    {
        public bool CheckOnDeath(IObject conflictedObject)
        {
            return conflictedObject is Medkit;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public Bitmap GetImage()
        {
            return Resource.Floor1; ;
        }
    }
}
