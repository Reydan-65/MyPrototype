using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using UnityEngine;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyCallAssist : MonoBehaviour
    {
        private IGameFactory gameFactory;

        [Inject]
        public void Construct (IGameFactory gameFactory) => this.gameFactory = gameFactory;

        public void CallAssistToPersuitTarget(float radius)
        {
            foreach (GameObject enemy in gameFactory.EnemiesObject)
            {
                if (enemy == null) continue;

                EnemyFollowToTarget folower = enemy.transform.root.GetComponent<EnemyFollowToTarget>();
                float distance = Vector3.Distance(transform.position, folower.transform.position);

                if (distance < radius)
                {
                    if (folower != null)
                        folower.enabled = true;
                }
            }
        }
    }
}
