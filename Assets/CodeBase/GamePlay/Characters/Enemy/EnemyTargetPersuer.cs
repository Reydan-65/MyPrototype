using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyTargetPersuer : MonoBehaviour
    {
        public event UnityAction PersuitTarget;
        public event UnityAction LostTarget;

        [SerializeField] private float viewDistance = 10f;

        private EnemyFollowToTarget followToTarget;
        private EnemyCallAssist callAssist;
        private EnemyCombatMovement combatMovement;
        private EnemyDasher dasher;

        private bool hasTarget = false;

        public bool IsPursuing => hasTarget;

        private IGameFactory gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory) => this.gameFactory = gameFactory;

        private void Awake()
        {
            followToTarget = GetComponent<EnemyFollowToTarget>();
            callAssist = GetComponent<EnemyCallAssist>();
            combatMovement = GetComponent<EnemyCombatMovement>();
            dasher = GetComponent<EnemyDasher>();

            followToTarget.enabled = false;
            dasher.enabled = false;
        }

        private void Update()
        {
            if (gameFactory == null || gameFactory.PrototypeObject == null) return;

            bool isInRange = Vector3.Distance(transform.position,
                gameFactory.PrototypeObject.transform.position) <= viewDistance;

            if (isInRange && !hasTarget)
                StartPersuit();
            else if (!isInRange && hasTarget)
                StopPersuit();
        }

        private void StartPersuit()
        {
            followToTarget.enabled = true;
            callAssist.CallAssistToPersuitTarget(viewDistance);
            hasTarget = true;
            combatMovement.SetActive(true);
            dasher.enabled = true;

            PersuitTarget?.Invoke();
        }

        private void StopPersuit()
        {
            followToTarget.enabled = false;
            hasTarget = false;
            combatMovement.SetActive(false);
            dasher.enabled = false;

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
