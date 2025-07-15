using UnityEngine;
using UnityEditor;

namespace CodeBase.Editor
{
    public class PlayerPrefsCleaner
    {
        [MenuItem("Tools/ClearPrefs")]
        public static void ClearPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            Debug.Log("PROGRESS DELETED!");
        }
    }
}
