using CodeBase.Data;
using CodeBase.GamePlay.Prototype;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;

namespace CodeBase.Infrastructure.Services
{
    public class HealingService : IHealingService
    {
        private const float HEALINGVALUE = 0.15f;

        private IProgressProvider progressProvider;
        private IProgressSaver progressSaver;
        private IGameFactory gameFactory;

        public HealingService(
            IProgressProvider progressProvider,
            IProgressSaver progressSaver,
            IGameFactory gameFactory)
        {
            this.gameFactory = gameFactory;
            this.progressSaver = progressSaver;
            this.progressProvider = progressProvider;
        }

        public bool TryHeal(LootItemID id)
        {
            PrototypeHealth health = gameFactory.PrototypeObject.GetComponent<PrototypeHealth>();

            if (progressProvider.PlayerProgress.PrototypeInventoryData.HealingPotionAmount <= 0) return false;
            if (health.Current >= health.Max) return false;

            progressProvider.PlayerProgress.PrototypeInventoryData.ConsumeItem(id, 1);

            float healAmount = 0;

            if (id == LootItemID.HealingPotion)
                healAmount = health.Max * HEALINGVALUE;

            health.ApplyHeal(healAmount);
            progressSaver.SaveProgress();

            return true;
        }
    }
}
