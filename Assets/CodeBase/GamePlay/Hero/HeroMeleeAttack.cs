using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.GamePlay.Hero
{
    public class HeroMeleeAttack : MonoBehaviour, IProgressLoadHandler
    {
        [SerializeField] private HeroMovement heroMovement;
        [SerializeField] private HeroAnimator heroAnimator;
        [SerializeField] private float cooldown;
        [SerializeField] private float radius;
        [SerializeField] private int damage;

        private IInputService inputService;

        [Inject]
        public void Construct(IInputService inputService)
        {
            this.inputService = inputService;
        }

        private Health[] targets;

        private float timer;

        private void Update()
        {
            if (timer >= cooldown)
                timer = cooldown;
            else
                timer += Time.deltaTime;

            if (CanAttack() == true)
            {
                targets = FindTargets();
                StartAttack();
            }
        }

        private void AnimationEventOnHit()
        {
            if (targets == null) return;

            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i] != null)
                    targets[i].ApplyDamage(damage);
            }
        }

        private bool CanAttack()
        {
            return timer == cooldown && inputService.AttackInput == true;
        }

        private Health[] FindTargets()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

            List<Health> result = new List<Health>();

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].transform.root == transform.root) continue;

                Health health = colliders[i].transform.root.GetComponent<Health>();

                if (health != null)
                {
                    result.Add(health);
                }
            }

            return result.ToArray();
        }

        private void StartAttack()
        {
            timer = 0;
            heroAnimator.Attack();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            damage = progress.HeroStats.Damage;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            float offset = GetComponent<CharacterController>().height / 2;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + offset, transform.position.z), radius);
        }
#endif
    }
}