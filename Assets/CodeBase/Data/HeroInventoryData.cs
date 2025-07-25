using System;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    public class HeroInventoryData
    {
        public event Action<int> CoinValueChanged;
        public event Action<int> HealingPotionValueChanged;
        public event Action KeyPickuped;

        [SerializeField] private int coinAmount;

        public int CoinAmount
        {
            get => coinAmount;
            set
            {
                coinAmount = value;
                CoinValueChanged?.Invoke(coinAmount);
            }
        }

        [SerializeField] private int healingPotionAmount;
        public int HealingPotionAmount
        {
            get => healingPotionAmount;
            set
            {
                healingPotionAmount = value;
                HealingPotionValueChanged?.Invoke(healingPotionAmount);
            }
        }

        [SerializeField] private bool hasKey;

        public bool HasKey
        {
            get => hasKey;
            set
            {
                if (hasKey != value)
                {
                    hasKey = value;
                    KeyPickuped?.Invoke();
                }
            }
        }

        public void SetDefaultInventoryData()
        {
            CoinAmount = 0;
            healingPotionAmount = 0;
            hasKey = false;
        }

        public void CopyFrom(HeroInventoryData data)
        {
            CoinAmount = data.CoinAmount;
            HealingPotionAmount = data.HealingPotionAmount;
            HasKey = data.HasKey;
        }

        public void AddItem(LootItemID id, int amount)
        {
            if (id == LootItemID.None) return;
            if (id == LootItemID.HealingPotion) return;

            if (id == LootItemID.Coin)
            {
                coinAmount += amount;
                CoinValueChanged?.Invoke(coinAmount);

                //Debug.Log($"Coins changed: {this.coinAmount}");
            }


            if (id == LootItemID.Key)
            {

            }
        }

        public void AddHealingItem(LootItemID id, int amount, float healingValue)
        {
            if (id == LootItemID.None) return;
            if (id == LootItemID.Coin) return;
            if (id == LootItemID.Key) return;

            if (id == LootItemID.HealingPotion)
            {
                healingPotionAmount += amount;
                HealingPotionValueChanged?.Invoke(healingPotionAmount);

                //Debug.Log($"Healing Potion changed: {this.healingPotionAmount}");
            }
        }

        public void ConsumeItem(LootItemID id, int amount)
        {
            if (id == LootItemID.None) return;

            if (id == LootItemID.Coin)
            {
                coinAmount -= amount;
                CoinValueChanged?.Invoke(coinAmount);

                //Debug.Log($"Coins changed: {this.coinAmount}");
            }

            if (id == LootItemID.HealingPotion)
            {
                healingPotionAmount -= amount;
                HealingPotionValueChanged?.Invoke(healingPotionAmount);

                //Debug.Log($"Healing Potion changed: {this.healingPotionAmount}");
            }

            if (id == LootItemID.Key)
            {

            }
        }
    }
}
