using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GamePlay.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private Image image;

        private void Start()
        {
            health.Changed += OnHitPointsChanged;

            OnHitPointsChanged();
        }

        private void OnDestroy()
        {
            health.Changed -= OnHitPointsChanged;
        }

        private void OnHitPointsChanged()
        {
            if (health.Max > 0)
                image.fillAmount = health.Current / health.Max;
        }
    }
}
