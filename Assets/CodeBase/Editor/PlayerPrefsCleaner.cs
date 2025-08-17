using System.IO;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    public class PlayerPrefsCleaner
    {
        [MenuItem("Tools/ClearPrefs")]
        public static void ClearPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            string progressSavePath = Path.Combine(Application.persistentDataPath, "progress_save.json");
            string settingsSavePath = Path.Combine(Application.persistentDataPath, "settings_save.json");

            if (File.Exists(progressSavePath))
                File.Delete(progressSavePath);
            if (File.Exists(settingsSavePath))
                File.Delete(settingsSavePath);

            Debug.Log("PROGRESS DELETED!");
            Debug.Log("SETTINGS RESET!");
        }
    }
}
