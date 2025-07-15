using UnityEngine;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyInventory : MonoBehaviour
    {
        [SerializeField] private GameObject keyInfoObject;

        private bool hasKey = false;
        private int coinsAmount;
        private int healingPotions;

        private void Start() => UpdateKeyInfo();

        public int GetCoinsAmount() => coinsAmount;
        public int GetHealingPotionsAmount() => healingPotions;
        public bool HasKey() => hasKey;
        public void SetCoinsAmount(int amount) => coinsAmount = Mathf.Max(0, amount);
        public void SetHealingPotionsAmount(int amount) => healingPotions = Mathf.Max(0, amount);

        public void SetHasKey(bool value)
        {
            hasKey = value;
            UpdateKeyInfo();
        }

        private void UpdateKeyInfo()
        {
            if (keyInfoObject != null)
                keyInfoObject.SetActive(hasKey);
        }
    }
}
