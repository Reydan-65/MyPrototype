using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public abstract class LootItem : MonoBehaviour
    {
        private Collider lootCollider;
        protected IGameFactory gameFactory;
        protected IProgressProvider progressProvider;

        [Inject]
        public void Construct(IGameFactory gameFactory, IProgressProvider progressProvider)
        {
            this.gameFactory = gameFactory;
            this.progressProvider = progressProvider;
        }

        private void Start()
        {
            transform.SetParent(null);
            lootCollider = GetComponent<Collider>();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == gameFactory.HeroObject)
            {
                OnPickup();
                Destroy(gameObject);
            }
        }

        protected virtual void OnPickup() { }

        public void SetColliderEnabled(bool value)
        {
            lootCollider.enabled = value;
        }
    }
}
