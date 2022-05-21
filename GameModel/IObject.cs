using System.Drawing;

namespace GameModel
{
    public interface IObject
    {
        Bitmap GetImage();
        int GetDrawingPriority();
        bool CheckOnDeath(IObject conflictedObject);
    }
}