using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CodeBase.Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Level")]
    public class LevelConfig : ScriptableObject
    {
        public string SceneName;
        public Vector3 PrototypeSpawnPoint;
        public Vector3 FinishPoint;

#if UNITY_EDITOR
        [Space(10)]
        [SerializeField] private SceneAsset linkedScene;

        public SceneAsset LinkedScene => linkedScene;

        public void SetLinkedScene(SceneAsset scene)
        {
            linkedScene = scene;
            EditorUtility.SetDirty(this);
        }
#endif

        public void SetPrototypeSpawnPoint(Vector3 position) => PrototypeSpawnPoint = position;
        public void SetFinishPoint(Vector3 position) => FinishPoint = position;
    }
}