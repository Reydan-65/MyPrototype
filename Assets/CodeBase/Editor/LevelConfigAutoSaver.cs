#if UNITY_EDITOR
using CodeBase.Configs;
using CodeBase.GamePlay.Hero;
using CodeBase.GamePlay;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class LevelConfigAutoSaver
{
    private static bool isInitialized;

    static LevelConfigAutoSaver()
    {
        if (isInitialized) return;

        EditorSceneManager.sceneSaving += OnSceneSaving;
        isInitialized = true;
    }

    private static void OnSceneSaving(UnityEngine.SceneManagement.Scene scene, string path)
    {
        LevelConfig config = FindConfigForScene(scene);

        if (config == null) return;

        UpdateConfigFromScene(config);
    }

    private static LevelConfig FindConfigForScene(UnityEngine.SceneManagement.Scene scene)
    {
        string[] guids = AssetDatabase.FindAssets("t:LevelConfig");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            LevelConfig config = AssetDatabase.LoadAssetAtPath<LevelConfig>(path);

            if (config == null) continue;

            if (config.LinkedScene != null &&
                AssetDatabase.GetAssetPath(config.LinkedScene) == scene.path)
                return config;

            if (config.SceneName == scene.name) return config;
        }
        return null;
    }

    private static void UpdateConfigFromScene(LevelConfig config)
    {
        bool changed = false;
        SerializedObject serializedConfig = new SerializedObject(config);

        var spawnMarker = Object.FindFirstObjectByType<HeroSpawnPoint>();
        if (spawnMarker != null)
        {
            SerializedProperty spawnProp = serializedConfig.FindProperty("HeroSpawnPoint");
            if (spawnProp != null && spawnProp.propertyType == SerializedPropertyType.Vector3)
            {
                Vector3 newPosition = spawnMarker.transform.position;
                if (spawnProp.vector3Value != newPosition)
                {
                    spawnProp.vector3Value = newPosition;
                    changed = true;
                    Debug.Log($"Updated spawn point in Config {config.name}  to {newPosition}");
                }
            }
        }

        var finishMarker = Object.FindFirstObjectByType<FinishPoint>();
        if (finishMarker != null)
        {
            SerializedProperty finishProp = serializedConfig.FindProperty("FinishPoint");
            if (finishProp != null && finishProp.propertyType == SerializedPropertyType.Vector3)
            {
                Vector3 newPosition = finishMarker.transform.position;
                if (finishProp.vector3Value != newPosition)
                {
                    finishProp.vector3Value = newPosition;
                    changed = true;
                    Debug.Log($"Updated finish point in Config {config.name} to {newPosition}");
                }
            }
        }

        if (changed)
        {
            serializedConfig.ApplyModifiedProperties();
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
        }
    }
}
#endif