using CodeBase.GamePlay.Enemies;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Factory;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Services.EntityActivityController
{
    public class EntityActivityController : IEntityActivityController
    {
        private Dictionary<EnemyFollowToHero, bool> originalFollowerStates = new();

        private IGameFactory gameFactory;
        private IInputService inputService;
        private ILootService lootService;

        public EntityActivityController(
            IGameFactory gameFactory,
            IInputService inputService,
            ILootService lootService)
        {
            this.gameFactory = gameFactory;
            this.inputService = inputService;
            this.lootService = lootService;
        }

        public void SetEntitiesActive(bool isActive)
        {
            if (inputService != null)
                inputService.Enable = isActive;

            SetEnemyActiveState(isActive);
            SetLootItemsActiveState(isActive);
        }

        private void SetLootItemsActiveState(bool isActive)
        {
            lootService?.SetPauseVisualEffects(isActive);
        }

        private void SetEnemyActiveState(bool isActive)
        {
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

                var persuer = enemy.GetComponent<EnemyHeroPersuer>();
                if (persuer != null)
                    persuer.enabled = isActive;

                var agent = enemy.GetComponent<NavMeshAgent>();
                if (agent != null)
                    agent.enabled = isActive;
            }
        }

        public void MoveCameraToTarget(Transform target)
        {
            if (gameFactory == null || gameFactory.FollowCamera == null) return;

            gameFactory.FollowCamera.SetTarget(target);
        }
    }
}
