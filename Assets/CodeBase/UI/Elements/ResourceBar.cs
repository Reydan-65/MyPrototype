using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GamePlay.UI
{
    public class ResourceBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private Slider slider;

        private IResource resource;

        private void Start()
        {
            if (resource != null)
                resource.Changed += OnResourceChanged;

            UpdateView();
        }

        private void OnDestroy()
        {
            if (resource != null)
                resource.Changed -= OnResourceChanged;
        }

        public void SetResource(IResource newResource)
        {
            if (resource != null)
                resource.Changed -= OnResourceChanged;

            resource = newResource;

            if (resource != null)
            {
                resource.Changed += OnResourceChanged;
                UpdateView();
            }
        }

        private void OnResourceChanged()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            if (resource == null || resource.Max <= 0) return;

            float fillAmount = Mathf.Clamp01(resource.Current / resource.Max);

            if (fillImage != null)
                fillImage.fillAmount = fillAmount;

            if (slider != null)
                slider.value = fillAmount;
        }
    }
}