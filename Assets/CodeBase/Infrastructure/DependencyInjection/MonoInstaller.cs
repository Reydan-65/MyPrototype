using UnityEngine;

namespace CodeBase.Infrastructure.DependencyInjection
{
    public abstract class MonoInstaller : MonoBehaviour
    {
        protected DIContainer container;

        [Inject]
        public void Construct(DIContainer container)
        {
            this.container = container;
        }

        public virtual void InstallBindings() { }
    }
}
