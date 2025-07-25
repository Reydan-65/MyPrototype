using CodeBase.Data;
using CodeBase.GamePlay.Hero;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;

namespace CodeBase.Infrastructure.Services
{
    public class HealingService : IHealingService
    {
        private const float HEALINGVALUE = 0.15f;

        private IProgressProvider progressProvider;
        private IGameFactory gameFactory;

        public HealingService(
            IProgressProvider progressProvider,
            IGameFactory gameFactory)
        {
            this.gameFactory = gameFactory;
            this.progressProvider = progressProvider;
        }

        public bool TryHeal(LootItemID id)
        {
            HeroHealth health = gameFactory.HeroObject.GetComponent<HeroHealth>();

            if (progressProvider.PlayerProgress.HeroInventoryData.HealingPotionAmount <= 0) return false;
            if (health.Current >= health.Max) return false;

            progressProvider.PlayerProgress.HeroInventoryData.ConsumeItem(id, 1);

            float healAmount = 0;

            if (id == LootItemID.HealingPotion)
                healAmount = health.Max * HEALINGVALUE;

            health.ApplyHeal(healAmount);

            return true;
        }
    }
}
