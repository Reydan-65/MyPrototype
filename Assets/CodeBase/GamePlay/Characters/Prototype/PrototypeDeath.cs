using UnityEngine;

namespace CodeBase.GamePlay.Prototype
{
    public class PrototypeDeath : MonoBehaviour
    {
        [SerializeField] private PrototypeHealth health;

        private void Start() => health.Depleted += OnDie;
        private void OnDestroy() => health.Depleted -= OnDie;
        private void OnDie() => Destroy(gameObject);
    }
}
