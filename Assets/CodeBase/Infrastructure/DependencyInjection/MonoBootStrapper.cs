using UnityEngine;

namespace CodeBase.Infrastructure.DependencyInjection
{
    public abstract class MonoBootStrapper : MonoBehaviour
    {
        public abstract void OnBindResolved();
    }
}
