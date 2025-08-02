using CodeBase.GamePlay.Enemies;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Factory;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.EntityActivityController
{
    public class EntityActivityController : IEntityActivityController
    {
        private Dictionary<EnemyFollowToHero, bool> originalFollowerStates = new();

        private IGameFactory gameFactory;
        private IInputService inputService;

        public EntityActivityController(
            IGameFactory gameFactory,
            IInputService inputService)
        {
            this.gameFactory = gameFactory;
            this.inputService = inputService;
        }

        public void SetEntitiesActive(bool isActive)
        {
            if (inputService != null)
            {
                inputService.Enable = isActive;
            }

            if (gameFactory?.EnemiesObject == null) return;

            foreach (var enemy in gameFactory.EnemiesObject)
            {
                if (enemy == null) continue;

                var shooter = enemy.GetComponent<EnemyShooter>();
                if (shooter != null)
                    shooter.enabled = isActive;

                var follower = enemy.GetComponent<EnemyFollowToHero>();
                if (follower != null)
                {
                    if (isActive)
                    {
                        if (originalFollowerStates.TryGetValue(follower, out var originalState))
                            follower.enabled = originalState;
                    }
                    else
                    {
                        originalFollowerStates[follower] = follower.enabled;
                        follower.enabled = false;
                    }
                }
            }
        }

        public async void MoveCameraToTarget(Transform target, float duration, System.Action onComplete = null)
        {
            if (gameFactory == null) return;

            var originalTarget = gameFactory.FollowCamera.Target;

            await gameFactory.FollowCamera.MoveToTarget(target, duration, new Vector3(5,0,0));

            await Task.Delay((int)(duration * 1000));

            await gameFactory.FollowCamera.MoveToTarget(originalTarget, duration);

            onComplete?.Invoke();
        }
    }
}
