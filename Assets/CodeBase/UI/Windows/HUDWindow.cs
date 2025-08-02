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

        [SerializeField] private GameObject solution;
        public GameObject Solution { get => solution; set => solution = value; }

        private void Start()
        {
            SetSolutionActive(false);
        }

        public void SetSolutionActive(bool active)
        {
            if (solution == null) return;

            if (solution.activeSelf != active)
                solution.SetActive(active);
        }

        protected override void OnClose()
        {
            Destroy(gameObject);
        }
    }
}
