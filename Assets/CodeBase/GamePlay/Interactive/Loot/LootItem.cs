using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public abstract class LootItem : MonoBehaviour
    {
        [SerializeField] private string uniqueID;
        public string UniqueID => uniqueID;

        private Collider lootCollider;
        private Rotator rotator;
        
        public Rotator Rotator => rotator;

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

            lootCollider = GetComponent<Collider>();
            transform.SetParent(null);
            rotator = GetComponent<Rotator>();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (gameFactory == null) return;

            if (other.gameObject == gameFactory.PrototypeObject)
            {
                OnPickup();
                progressProvider.PlayerProgress.AddPickedLoot(uniqueID);
                progressProvider.PlayerProgress.HasSavedGame = true;
                progressSaver.SaveProgress();
                Destroy(gameObject);
            }
        }

        protected virtual void OnPickup() { }

        public void SetColliderEnabled(bool value)
        {
            if (lootCollider != null)
                lootCollider.enabled = value;
        }

        public void GenerateID()
        {
            if (string.IsNullOrEmpty(uniqueID))
            {
                var pos = transform.position;
                uniqueID = $"loot_{gameObject.name}_{pos.x:F1}_{pos.z:F1}";
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }
    }
}
