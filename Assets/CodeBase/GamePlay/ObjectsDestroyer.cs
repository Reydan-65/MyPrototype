using UnityEngine;

namespace CodeBase.GamePlay
{
    public class ObjectsDestroyer
    {
        public static void DestroyObjectsByTag(string tag)
        {
            if (string.IsNullOrEmpty(tag)) return;

            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

            if (objects == null ||  objects.Length == 0) return;

            foreach (GameObject obj in objects)
            {
                if (obj != null)
                    Object.Destroy(obj);
            }
        }
    }
}
