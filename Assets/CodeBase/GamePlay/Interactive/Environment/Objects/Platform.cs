using CodeBase.Data;
using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public class Platform : SavableInteractable
    {
        [SerializeField] private Animator platformAnimator;
        [SerializeField] private string uniqueID = "platform_";

        private bool isActivated;

        public override bool IsActivated
        {
            get => isActivated;
            set
            {
                if (isActivated != value)
                {
                    isActivated = value;
                    if (isActivated) ActivatedVisual();
                }
            }
        }

        private void ActivatedVisual()
        {
            if (platformAnimator != null)
                platformAnimator.enabled = true;
        }

        public override void Interact()
        {
            base.Interact();
            if (isActivated) return;
            isActivated = true;
            ActivatedVisual();
            progressSaver.SaveProgress();
        }

        public override string UniqueID => uniqueID;

        public override void LoadProgress(PlayerProgress progress)
        {
            if (progress == null) return;

            if (progress.TryGetInteractiveState(UniqueID, out bool state))
            {
                isActivated = state;
                if (isActivated) ActivatedVisual();
            }
        }

        public override void UpdateProgressBeforeSave(PlayerProgress progress)
        {
            progress.SetInteractiveState(UniqueID, IsActivated);
        }
    }
}
