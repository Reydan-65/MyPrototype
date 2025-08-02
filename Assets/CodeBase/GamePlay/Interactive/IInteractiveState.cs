namespace CodeBase.GamePlay.Interactive
{
    public interface IInteractiveState
    {
        bool IsActivated { get; set; }
        string UniqueID { get; }
    }
}
