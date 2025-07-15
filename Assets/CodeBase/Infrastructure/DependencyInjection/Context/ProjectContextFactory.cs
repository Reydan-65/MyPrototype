using UnityEngine;

namespace CodeBase.Infrastructure.DependencyInjection
{
    public class ProjectContextFactory : MonoBehaviour
    {
        public static string ProjectContextResourcePath = "ProjectContext";

        public static void TryCreate()
        {
            if (ProjectContext.Initialized == true) return;

            ProjectContext prefab = TryGetPrefab();

            if (prefab != null)
            {
                GameObject.Instantiate(prefab);
            }
        }

        private static ProjectContext TryGetPrefab()
        {
            var prefabs = Resources.LoadAll<ProjectContext>(ProjectContextResourcePath);

            if (prefabs.Length > 0)
            {
                return prefabs[0];
            }

            return null;
        }
    }
}
