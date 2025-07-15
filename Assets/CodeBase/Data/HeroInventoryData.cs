using System;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    public class HeroInventoryData
    {
        public event Action<int> CoinValueChanged;
        public event Action KeyPickuped;

        [SerializeField] private int coinAmount;

        public int CoinAmount
        {
            get => coinAmount;
            set
            {
                coinAmount = value;
                CoinValueChanged?.Invoke(this.coinAmount);
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
            hasKey = false;
        }

        public void CopyFrom(HeroInventoryData data)
        {
            CoinAmount = data.CoinAmount;
            HasKey = data.HasKey;
        }

        public void AddCoin(int coinAmount)
        {
            this.coinAmount += coinAmount;
            CoinValueChanged?.Invoke(this.coinAmount);

            //Debug.Log($"Coins changed: {this.coinAmount}");
        }

        public bool SpendCoins(int coinAmount)
        {
            if (this.coinAmount < 0 || this.coinAmount < coinAmount)
                return false;

            this.coinAmount -= coinAmount;
            return true;
        }
    }
}
