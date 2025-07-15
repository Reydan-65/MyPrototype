using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace CodeBase.Data
{
    [Serializable]
    public class PurchaseData
    {
        public event UnityAction Changed;
        
        public List<BoughtIAP> BoughtIAPs = new List<BoughtIAP>();
        public bool isFemaleSkinUnlocked = false;

        public bool IsFemaleSkinUnlocked
        {
            get => isFemaleSkinUnlocked;
            set
            {
                if (isFemaleSkinUnlocked != value)
                {
                    isFemaleSkinUnlocked = value;
                    Changed?.Invoke();
                }
            }
        }

        public void AddPurchase(string id)
        {
            BoughtIAP boughtIAP = BoughtIAPs.Find(x=> x.ProductID == id);

            if (boughtIAP != null)
                boughtIAP.Amount++;
            else
                BoughtIAPs.Add(new BoughtIAP(id, 1));

            Changed?.Invoke();
        }

        public void CopyFrom(PurchaseData data)
        {
            BoughtIAPs = data.BoughtIAPs;
            isFemaleSkinUnlocked = data.IsFemaleSkinUnlocked;
        }
    }
}
