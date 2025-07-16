namespace CodeBase.GamePlay.UI
{
    public class UpgradeStatsWindow : WindowBase
    {
        protected override void OnClose()
        {
            Destroy(gameObject);
        }
    }
}
