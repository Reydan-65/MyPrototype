namespace CodeBase.GamePlay.UI
{
    public class DeathPresenter : WindowPresenterBase<DeathWindow>
    {
        private DeathWindow window;

        public override void SetWindow(DeathWindow window)
        {
            this.window = window;
            this.window.CleanUped += OnCleanUped;
        }

        private void OnCleanUped() => window.CleanUped -= OnCleanUped;
    }
}
