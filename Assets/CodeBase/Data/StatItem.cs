namespace CodeBase.Data
{
    [System.Serializable]
    public class StatItem
    {
        public int Level = 1;
        public float Value;

        public StatItem(float value)
        {
            Value = value;
        }

        public void SetDefault(float defaultValue)
        {
            Level = 1;
            Value = defaultValue;
        }
    }
}