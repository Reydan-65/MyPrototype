using UnityEngine;

namespace CodeBase.GamePlay.UI
{
    public class HUDWindow : WindowBase
    {
        [SerializeField] private HealthBar healthBar;
        public HealthBar HealthBar => healthBar;

        [SerializeField] private HealthText healthText;
        public HealthText HealthText => healthText;

        [SerializeField] private EnergyBar energyBar;
        public EnergyBar EnergyBar => energyBar;

        [SerializeField] private EnergyText energyText;
        public EnergyText EnergyText => energyText;

        [SerializeField] private HealingPotionItem healingPotionItem;
        public HealingPotionItem HealingPotionItem => healingPotionItem;

        protected override void OnClose()
        {
            Destroy(gameObject);
        }
    }
}
