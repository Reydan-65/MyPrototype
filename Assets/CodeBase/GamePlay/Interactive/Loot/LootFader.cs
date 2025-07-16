using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public class LootFader : MonoBehaviour
    {
        private const float LIFETIME = 15.0f;
        private const float STARTFADINGTIME = 5.0f;
        private const float MINSCALE = 0.1f;

        private Transform fadingTransform;
        private float currentTimer;
        private bool isFading;
        private Vector3 initialScale;

        private void Awake()
        {
            currentTimer = LIFETIME;
            fadingTransform = transform.GetChild(0);
            initialScale = fadingTransform.localScale;
        }

        private void Update()
        {
            currentTimer -= Time.deltaTime;

            if (currentTimer <= STARTFADINGTIME && !isFading)
                isFading = true;

            Fading();

            if (currentTimer <= 0)
                Destroy(gameObject);
        }

        private void Fading()
        {
            isFading = true;

            float progress = currentTimer / STARTFADINGTIME;
            float currentScale = Mathf.Lerp(MINSCALE, 1f, progress);

            if (fadingTransform != null)
                fadingTransform.localScale = initialScale * currentScale;
        }
    }
}
