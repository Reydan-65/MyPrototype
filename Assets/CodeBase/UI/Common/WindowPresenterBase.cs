using CodeBase.UI;
using UnityEngine.Events;

namespace CodeBase.GamePlay.UI
{
    public abstract class WindowPresenterBase<TWindow> where TWindow : WindowBase
    {
        public event UnityAction WindowAssigned;
        public abstract void SetWindow(TWindow window);
        public virtual void AssignWindowToClickSounds(TWindow window)
        {
            UIClickSound[] clickSounds = window.GetComponentsInChildren<UIClickSound>();
            if (clickSounds != null && clickSounds.Length > 0)
                foreach (UIClickSound clickSound in clickSounds)
                    clickSound.SetWindow(window);

            WindowAssigned?.Invoke();
        }
    }
}
