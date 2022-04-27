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

        public string GetPathForImage() => "C:\\Users\\kost4\\source\\repos\\Rep\\Game\\Resources\\Beton.png";
    }
}
