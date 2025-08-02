using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public abstract class SavableInteractable : Interactable,
        IInteractiveState,
        IProgressBeforeSaveHandler,
        IProgressLoadHandler
    {
        protected IProgressSaver progressSaver;

        [Inject]
        public void Construct(IProgressSaver progressSaver)
        {
            this.progressSaver = progressSaver;
        }

        private void Awake()
        {
            progressSaver.AddObject(gameObject);
        }

        public abstract bool IsActivated { get; set; }
        public abstract string UniqueID { get; }

        public virtual void LoadProgress(PlayerProgress progress) { }
        public virtual void UpdateProgressBeforeSave(PlayerProgress progress) { }
    }
}