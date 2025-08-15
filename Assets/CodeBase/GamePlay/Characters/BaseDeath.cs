using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Sounds;
using UnityEngine;

namespace CodeBase.GamePlay
{
    public class BaseDeath : MonoBehaviour
    {
        public SFXEvent PlaySFX;

        [Header("Destroy Effect Settings")]
        [SerializeField] protected GameObject destroySFX;
        [SerializeField] protected Transform visualModel;
        [SerializeField] protected float scatterForce = 3f;
        [SerializeField] protected float scatterTorque = 2f;
        [SerializeField] protected float scatterUpwardModifier = 0.5f;
        [SerializeField] protected float destroyDelay = 3f;

        protected IHealth health;
        protected Transform[] parts;

        protected IGameFactory gameFactory;
        protected IAssetProvider assetProvider;

        [Inject]
        public void Construct(
            IGameFactory gameFactory,
            IAssetProvider assetProvider)
        {
            this.gameFactory = gameFactory;
            this.assetProvider = assetProvider;
        }

        protected virtual void Awake()
        {
            health = gameObject.GetComponent<IHealth>();
            parts = visualModel.GetComponentsInChildren<Transform>();
        }

        protected void Start() => health.Depleted += OnDie;
        protected void OnDestroy() => health.Depleted -= OnDie;

        protected virtual async void OnDie()
        {
            DisableComponents();
            ScatterParts();

            gameFactory.CreateImpactEffectObjectFromPrefab(destroySFX, visualModel.transform.position, Quaternion.identity);
            PlaySFX?.Invoke(await assetProvider.Load<AudioClip>(AssetAddress.ExplosionSound), 1.25f, 1, 1);

            Destroy(gameObject, destroyDelay);
        }

        protected virtual void DisableComponents() { }

        protected void ScatterParts()
        {
            if (parts == null || parts.Length == 0) return;

            for (int i = 0; i < parts.Length - 1; i++) // without FirePoint
            {
                if (parts[i] == null || parts[i] == visualModel) continue;

                var collider = parts[i].GetComponent<Collider>();
                if (collider != null)
                    collider.enabled = true;

                var rb = parts[i].gameObject.AddComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    Vector3 randomDirection = new Vector3(
                        Random.Range(-1f, 1f),
                        Random.Range(-0.5f, 0.5f),
                        Random.Range(-1, 1)
                    ).normalized;

                    rb.AddForce(randomDirection * scatterForce + Vector3.up * scatterUpwardModifier, ForceMode.Impulse);
                    rb.AddTorque(Random.insideUnitSphere * scatterTorque, ForceMode.Impulse);
                }

                parts[i].SetParent(null);
                Destroy(parts[i].gameObject, Random.Range(destroyDelay * 0.5f, destroyDelay * 1.5f));
            }
        }
    }
}
