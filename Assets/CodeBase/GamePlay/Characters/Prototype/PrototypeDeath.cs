using UnityEngine;

namespace CodeBase.GamePlay.Prototype
{
    public class PrototypeDeath : BaseDeath
    {
        private CharacterController controller;
        private PrototypeInput input;

        protected override void Awake()
        {
            base.Awake();
            controller = GetComponent<CharacterController>();
            input = GetComponent<PrototypeInput>();
        }

        protected override void OnDie()
        {
            gameFactory.CreateImpactEffectObjectFromPrefab(destroySFX, visualModel.transform.position, Quaternion.identity);
            base.OnDie();
        }

        protected override void DisableComponents()
        {
            controller.enabled = false;
            input.enabled = false;
        }
    }
}
