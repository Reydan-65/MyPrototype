using TMPro;
using UnityEngine;

namespace CodeBase.GamePlay.UI
{
    public class ResourceText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        private IResource resource;

        public void SetResource(IResource resource)
        {
            if (this.resource != null)
                this.resource.Changed -= UpdateText;

            this.resource = resource;

            if (this.resource != null)
            {
                this.resource.Changed += UpdateText;
                UpdateText();
            }
        }

        private void UpdateText()
        {
            if (text != null && resource != null)
                text.text = ((int)resource.Current).ToString();
        }

        private void OnDestroy()
        {
            if (resource != null)
                resource.Changed -= UpdateText;
        }
    }
}