using CodeBase.GamePlay.Hero;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Configs;
using UnityEngine.AI;
using UnityEngine;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyMeleeAttack : MonoBehaviour/*, IEnemyConfigInstaller*/
    {
        //[SerializeField] private NavMeshAgent agent;
        //[SerializeField] private EnemyAnimator animator;
        //[SerializeField] private float cooldown;
        //[SerializeField] private float radius;
        //[SerializeField] private float damage;

        //private IGameFactory gameFactory;
        //private HeroHealth target;

        //private float timer;
        //private float findTargetTimer;
        //private bool isInitialized = false;

        //[Inject]
        //public void Construct(IGameFactory gameFactory)
        //{
        //    this.gameFactory = gameFactory;
        //    InitializeTarget();
        //}

        //public void InstallEnemyConfig(EnemyConfig config)
        //{
        //    cooldown = config.AttackCooldown;
        //    radius = config.ShootingRange;
        //    damage = config.Damage;
        //}

        //private void InitializeTarget()
        //{
        //    if (gameFactory == null) return;

        //    if (gameFactory.HeroHealth == null)
        //        gameFactory.HeroCreated += OnHeroCreated;
        //    else
        //        SetTarget(gameFactory.HeroHealth);
        //}
        //private void OnHeroCreated()
        //{
        //    if (gameFactory != null && gameFactory.HeroHealth != null)
        //    {
        //        SetTarget(gameFactory.HeroHealth);
        //        gameFactory.HeroCreated -= OnHeroCreated;
        //    }
        //}

        //private void Update()
        //{
        //    if (!isInitialized || target == null)
        //    {
        //        findTargetTimer += Time.deltaTime;

        //        if (findTargetTimer == 1 && gameFactory != null)
        //        {
        //            SetTarget(gameFactory.HeroHealth);
        //            findTargetTimer = 0;
        //        }

        //        return;
        //    }

        //    if (target == null) return;

        //    timer += Time.deltaTime;

        //    if (CanAttack() == true && target.Current > 0)
        //        StartAttack();
        //}

        //private void SetTarget(HeroHealth heroHealth)
        //{
        //    if (heroHealth == null) return;

        //    target = heroHealth;
        //    isInitialized = true;
        //}

        //private void AnimationEventOnHit()
        //{
        //    if (target != null)
        //    {
        //        target.ApplyDamage(damage);
        //    }
        //}

        //private void StartAttack()
        //{
        //    timer = 0;
        //    animator.PlayAttack();
        //}

        //private bool CanAttack()
        //{
        //    return timer >= cooldown && agent.velocity.magnitude <= 0.1f &&
        //        Vector3.Distance(transform.position, target.transform.position) <= radius;
        //}
    }
}
