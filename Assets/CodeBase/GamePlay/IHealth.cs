namespace CodeBase.GamePlay
{
    public interface IHealth : IResource
    {
        void ApplyDamage(float damage);
        void ApplyHeal(float amount);
        public void SetInvulnerability(bool enable);
    }
}
