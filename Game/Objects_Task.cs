using System;
using System.Drawing;
using GameModel;

namespace Game
{
    class Wall : IObject
    {
        public ObjectCommand Action(int x, int y)
        {
            return new ObjectCommand() { DeltaX = 0, DeltaY = 0 };
        }

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
        public ObjectCommand Action(int x, int y)
        {
            return new ObjectCommand() { DeltaX = 0, DeltaY = 0 };
        }

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
        public ObjectCommand Action(int x, int y)
        {
            return new ObjectCommand() { DeltaX = 0, DeltaY = 0 };
        }

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
        public ObjectCommand Action(int x, int y)
        {
            return new ObjectCommand() { DeltaX = 0, DeltaY = 0 };
        }

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
        public ObjectCommand Action(int x, int y)
        {
            return new ObjectCommand() { DeltaX = 0, DeltaY = 0 };
        }

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
        public ObjectCommand Action(int x, int y)
        {
            return new ObjectCommand() { DeltaX = 0, DeltaY = 0 };
        }

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
        public ObjectCommand Action(int x, int y)
        {
            return new ObjectCommand() { DeltaX = 0, DeltaY = 0 };
        }

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
        public ObjectCommand Action(int x, int y)
        {
            return new ObjectCommand() { DeltaX = 0, DeltaY = 0 };
        }

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
        public ObjectCommand Action(int x, int y)
        {
            return new ObjectCommand() { DeltaX = 0, DeltaY = 0 };
        }

        public bool CheckOnDeath(IObject conflictedObject)
        {
            return conflictedObject != this;
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
        public ObjectCommand Action(int x, int y)
        {
            return new ObjectCommand() { DeltaX = 0, DeltaY = 0 };
        }

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
}
