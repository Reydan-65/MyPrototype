using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyHeroPersuer : MonoBehaviour
    {
        public event UnityAction PersuitTarget;
        public event UnityAction LostTarget;

        [SerializeField] private EnemyFollowToHero followToHero;
        [SerializeField] private float viewDistance;

        private IGameFactory gameFactory;
        private bool hasTarget = false;
        public bool IsPursuing => hasTarget;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            this.gameFactory = gameFactory;
        }

        private void Start()
        {
            followToHero.enabled = false;
        }

        private void Update()
        {
            if (gameFactory.HeroObject == null) return;

            bool isInRange = Vector3.Distance(followToHero.transform.position,
                                    gameFactory.HeroObject.transform.position) <= viewDistance;

            if (isInRange && !hasTarget)
                StartPersuit();
            else if (!isInRange && hasTarget)
                StopPersuit();
        }

        private void StartPersuit()
        {
            followToHero.enabled = true;
            hasTarget = true;

            PersuitTarget?.Invoke();
        }

        private void StopPersuit()
        {
            followToHero.enabled = false;
            hasTarget = false;

            EnemyHealth enemyHealth = GetComponent<EnemyHealth>();

            if (enemyHealth.Current > 0)
                LostTarget?.Invoke();
        }

        private void OnDestroy()
        {
            if (hasTarget)
                LostTarget?.Invoke();
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, viewDistance);
        }
#endif
    }
}
