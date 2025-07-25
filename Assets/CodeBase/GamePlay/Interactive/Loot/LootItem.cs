using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public abstract class LootItem : MonoBehaviour
    {
        private Collider lootCollider;
        protected IGameFactory gameFactory;
        protected IProgressProvider progressProvider;
        protected IProgressSaver progressSaver;

        [Inject]
        public void Construct(
            IGameFactory gameFactory,
            IProgressProvider progressProvider,
            IProgressSaver progressSaver)
        {
            this.gameFactory = gameFactory;
            this.progressProvider = progressProvider;
            this.progressSaver = progressSaver;
        }

        private void Start()
        {
            transform.SetParent(null);
            lootCollider = GetComponent<Collider>();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (gameFactory == null) return;

            if (other.gameObject == gameFactory.HeroObject)
            {
                OnPickup();
                progressSaver.SaveProgress();
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
