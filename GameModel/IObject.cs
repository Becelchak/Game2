using System.Drawing;

namespace GameModel
{
    public interface IObject
    {
        Bitmap GetImage();
        int GetDrawingPriority();
        ObjectCommand Action(int x, int y);
        bool CheckOnDeath(IObject conflictedObject);
    }
}