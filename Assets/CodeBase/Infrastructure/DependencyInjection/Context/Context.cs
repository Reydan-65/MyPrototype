using UnityEngine;

namespace CodeBase.Infrastructure.DependencyInjection
{
    public abstract class Context : MonoBehaviour
    {
        [SerializeField] protected MonoInstaller[] monoInstallers;
        [SerializeField] protected MonoBootStrapper contextBootStrapper;

        protected void InstallBindings()
        {
            if (monoInstallers == null) return;

            for (int i = 0; i < monoInstallers.Length; i++)
            {
                monoInstallers[i]?.InstallBindings();
            }
        }

        protected void OnBindResolved()
        {
            contextBootStrapper?.OnBindResolved();
        }
    }
}
