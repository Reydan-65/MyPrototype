namespace CodeBase.GamePlay.UI
{
    public class ShopWindow : WindowBase
    {
        protected override void OnClose()
        {
            Destroy(gameObject);
        }
    }
}
