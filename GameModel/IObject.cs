namespace GameModel
{
    public interface IObject
    {
        string GetPathForImage();
        int GetDrawingPriority();
        ObjectCommand Act(int x, int y);
        bool DeadInConflict(IObject conflictedObject);
    }
}