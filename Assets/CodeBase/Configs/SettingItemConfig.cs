using CodeBase.Data;
using UnityEngine;

namespace CodeBase.Configs
{
    [CreateAssetMenu(fileName = "SettingItemConfig", menuName = "Configs/SettingItem")]
    public class SettingItemConfig : ScriptableObject
    {
        public SettingItemID ID;
        public string Title;
        public string Value;
        public int MinValue;
        public int MaxValue;
    }
}