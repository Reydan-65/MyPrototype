using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    public class PrototypeInventoryData
    {
        public event Action<int> CoinValueChanged;
        public event Action<int> HealingPotionValueChanged;
        public event Action<string> KeyAdded;
        public event Action<string> KeyRemoved;

        [SerializeField] private int coinAmount;
        [SerializeField] private int healingPotionAmount;
        [SerializeField] private List<string> keys = new List<string>();

        public int CoinAmount
        {
            get => coinAmount;
            set
            {
                coinAmount = value;
                CoinValueChanged?.Invoke(coinAmount);
            }
        }

        public int HealingPotionAmount
        {
            get => healingPotionAmount;
            set
            {
                healingPotionAmount = value;
                HealingPotionValueChanged?.Invoke(healingPotionAmount);
            }
        }

        public List<string> Keys => keys;
        public bool HasKey(string keyId) => keys.Contains(keyId);

        public void SetDefaultInventoryData()
        {
            CoinAmount = 0;
            healingPotionAmount = 0;
            keys.Clear();
        }

        public void CopyFrom(PrototypeInventoryData data)
        {
            CoinAmount = data.CoinAmount;
            HealingPotionAmount = data.HealingPotionAmount;
            keys = new List<string>(data.Keys);
        }

        public void AddItem(LootItemID id, int amount)
        {
            if (id == LootItemID.None) return;
            if (id == LootItemID.Key) return;
            if (id == LootItemID.HealingPotion) return;

            if (id == LootItemID.Coin)
            {
                coinAmount += amount;
                CoinValueChanged?.Invoke(coinAmount);
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
            }
        }

        public void AddKey(string keyId)
        {
            if (!keys.Contains(keyId))
            {
                keys.Add(keyId);
                KeyAdded?.Invoke(keyId);
            }
        }

        public void RemoveKey(string keyId)
        {
            if (keys.Remove(keyId))
                KeyRemoved?.Invoke(keyId);
        }

        public void ConsumeItem(LootItemID id, int amount)
        {
            if (id == LootItemID.None) return;
            if (id == LootItemID.Key) return;

            if (id == LootItemID.Coin)
            {
                coinAmount -= amount;
                CoinValueChanged?.Invoke(coinAmount);
            }

            if (id == LootItemID.HealingPotion)
            {
                healingPotionAmount -= amount;
                HealingPotionValueChanged?.Invoke(healingPotionAmount);
            }
        }
    }
}
