namespace CodeBase.Data
{
    [System.Serializable]
    public class BoughtIAP
    {
        public string ProductID;
        public int Amount;

        public BoughtIAP(string productID, int amount)
        {
            ProductID = productID;
            Amount = amount;
        }
    }
}
