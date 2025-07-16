using CodeBase.GamePlay.UI;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.DependencyInjection;
using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public class Shrine : Interactable
    {
        private IWindowsProvider windowsProvider;

        [Inject]
        public void Construct(IWindowsProvider windowsProvider)
        {
            this.windowsProvider = windowsProvider;
        }

        public override void Interact(GameObject user)
        {
            base.Interact(user);
            Debug.Log($"Shrine used by {user.name}!");
            windowsProvider.Open(WindowID.ShrineWindow);
        }
    }
}