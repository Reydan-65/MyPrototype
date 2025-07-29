namespace CodeBase.GamePlay
{
    public interface IEnergy : IResource
    {
        void Consume(float amount);
        void Restore(float amount);
    }
}
