using UnityEngine;

namespace CodeBase.GamePlay.Hero
{
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField] private HeroHealth health;

        private void Start() => health.Die += OnDie;
        private void OnDestroy() => health.Die -= OnDie;
        private void OnDie() => Destroy(gameObject);
    }
}
