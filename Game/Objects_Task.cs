using GameModel;

namespace Game
{
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

        public string GetPathForImage() => "C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources\\BetonUp.png";
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

        public string GetPathForImage() => "C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources\\BetonDown.png";
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

        public string GetPathForImage() => "C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources\\BetonRight.png";
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

        public string GetPathForImage() => "C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources\\BetonLeft.png";
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

        public string GetPathForImage() => "C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources\\Glass.png";
    }
}
