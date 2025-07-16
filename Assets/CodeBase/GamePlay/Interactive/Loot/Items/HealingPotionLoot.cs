using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public class HealingPotionLoot : LootItem
    {
        [SerializeField] private float healingPercentValue;

        protected override void OnPickup()
        {
            if (gameFactory?.HeroHealth != null)
                gameFactory.HeroHealth.ApplyDamage(-1 * gameFactory.HeroHealth.Max * healingPercentValue);
        }
    }
}
