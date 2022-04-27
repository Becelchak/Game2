using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameModel;

namespace Game
{
    class Wall : IObject
    {
        public ObjectCommand Act(int x, int y)
        {
            return new ObjectCommand() { DeltaX = 0, DeltaY = 0 };
        }

        public bool DeadInConflict(IObject conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetPathForImage()
        {
            return "C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources\\Beton.png";
        }
    }
    class Floor : IObject
    {
        public ObjectCommand Act(int x, int y)
        {
            return new ObjectCommand() {DeltaX = 0, DeltaY = 0};
        }

        public bool DeadInConflict(IObject conflictedObject)
        {
           return true;
        }

        public int GetDrawingPriority()
        {
           return 1;
        }

        public string GetPathForImage()
        {
            return "C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources\\Floor.png";
        }
    }
    class Player : IObject
    {
        public int ChangeX, ChangeY;
        public ObjectCommand Act(int x, int y)
        {
            switch (Game_Map.KeyPressed)
            {
                case Keys.Right:
                    ChangeX = 1;
                    ChangeY = 0;
                    break;
                case Keys.Left:
                    ChangeX = -1;
                    ChangeY = 0;
                    break;
                case Keys.Down:
                    ChangeY = 1;
                    ChangeX = 0;
                    break;
                case Keys.Up:
                    ChangeY = -1;
                    ChangeX = 0;
                    break;
                default:
                    Wait();
                    break;
            }
            if (!StayInArea(x, y, ChangeX, ChangeY))
                Wait();
            FaceTheWall(x, y);
            return new ObjectCommand() { DeltaX = ChangeX, DeltaY = ChangeY };
        }
        public void Wait()
        {
            ChangeX = 0;
            ChangeY = 0;
        }

        public void FaceTheWall(int x, int y)
        {
            if (Game_Map.Map[x + ChangeX, y + ChangeY] is Wall)
                Wait();
        }
        private bool StayInArea(int x, int y, int dX, int dY)
        {
            return x + dX >= 0 && x + dX < Game_Map.MapWidth &&
                   y + dY >= 0 && y + dY < Game_Map.MapHeight;
        }

        public bool DeadInConflict(IObject conflictedObject)
        {
            if (conflictedObject is Wall)
            {
                return true;
            }
            return false;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetPathForImage()
        {
           return "C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources\\Player.png";
        }
    }
}
