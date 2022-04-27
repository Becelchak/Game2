namespace GameModel
{
    public interface IObject
    {
        string GetPathForImage();
        int GetDrawingPriority();
        ObjectCommand Action(int x, int y);
        bool CheckOnDeath(IObject conflictedObject);
    }
}