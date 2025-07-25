using UnityEngine;

namespace CodeBase.GamePlay.Hero
{
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField] private HeroHealth health;

        private void Start() => health.Depleted += OnDie;
        private void OnDestroy() => health.Depleted -= OnDie;
        private void OnDie() => Destroy(gameObject);
    }
}
