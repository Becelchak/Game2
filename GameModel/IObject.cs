namespace GameModel
{
    public interface IObject
    {
        string GetImageFileName();
        int GetDrawingPriority();
        ObjectCommand Act(int x, int y);
        bool DeadInConflict(IObject conflictedObject);
    }
}