using CodeBase.Sounds;
using UnityEngine;

namespace CodeBase.GamePlay
{
    public class HitImpactEffect : ImpactEffect
    {
        public SFXEvent PlaySFX;

        [SerializeField] private AudioClip hitEnvironmentClip;
        [SerializeField] private AudioClip hitClip;

        // Временно

        private void Start()
        {
            if (hitClip == null || hitEnvironmentClip == null) return;

            if (transform.root.GetComponent<IHealth>() != null)
                PlaySFX.Invoke(hitClip, 1f, 0.95f, 1.05f);
            else
                PlaySFX.Invoke(hitEnvironmentClip, 1f, 0.95f, 1.05f);
        }
    }
}
