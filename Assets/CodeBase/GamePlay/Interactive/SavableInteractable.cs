using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;

namespace CodeBase.GamePlay.Interactive
{
    public abstract class SavableInteractable : Interactable,
        IInteractiveState,
        IProgressBeforeSaveHandler,
        IProgressLoadHandler
    {
        protected IProgressSaver progressSaver;
        protected IProgressProvider progressProvider;

        [Inject]
        public void Construct(IProgressSaver progressSaver, IProgressProvider progressProvider)
        {
            this.progressSaver = progressSaver;
            this.progressProvider = progressProvider;
        }

        private void Awake() =>progressSaver.AddObject(gameObject);
        public abstract bool IsActivated { get; set; }
        public abstract string UniqueID { get; }
        public virtual void LoadProgress(PlayerProgress progress) { }
        public virtual void UpdateProgressBeforeSave(PlayerProgress progress)
            => progressProvider.PlayerProgress.HasSavedGame = true;
    }
}