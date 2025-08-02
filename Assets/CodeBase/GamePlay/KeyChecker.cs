using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GamePlay
{
    public class KeyChecker : MonoBehaviour
    {
        [SerializeField] private string requiredKeyID;
        [SerializeField] private Image infoImage;
        [SerializeField] private Color[] infoColors = new Color[2];
        [SerializeField] private bool updateOnStart = true;

        private IProgressProvider progressProvider;
        private IGameFactory gameFactory;
        private HeroInventoryData inventoryData;

        [Inject]
        public void Construct(IProgressProvider progressProvider, IGameFactory gameFactory)
        {
            this.progressProvider = progressProvider;
            this.gameFactory = gameFactory;

            if (this.progressProvider?.PlayerProgress?.HeroInventoryData == null) return;

            inventoryData = this.progressProvider.PlayerProgress.HeroInventoryData;
            SubscribeToEvents();
        }

        private void Start()
        {
            if (updateOnStart)
                UpdateKeyVisual();
        }

        private void SubscribeToEvents()
        {
            if (inventoryData == null) return;

            inventoryData.KeyAdded -= OnKeyChanged;
            inventoryData.KeyAdded += OnKeyChanged;

            gameFactory.HeroCreated -= OnHeroCreated;
            gameFactory.HeroCreated += OnHeroCreated;

            inventoryData.KeyRemoved -= OnKeyChanged;
            inventoryData.KeyRemoved += OnKeyChanged;
        }

        public void UnsubscribeFromEvents()
        {
            if (inventoryData == null) return;

            inventoryData.KeyAdded -= OnKeyChanged;
            inventoryData.KeyRemoved -= OnKeyChanged;
        }

        private void OnHeroCreated()
        {
            UpdateKeyVisual();
        }

        private void OnKeyChanged(string keyId)
        {
            if (string.IsNullOrEmpty(keyId) || keyId == requiredKeyID)
                UpdateKeyVisual();
        }

        private void UpdateKeyVisual()
        {
            if (infoImage == null) return;
            if (inventoryData == null) return;

            bool hasKey = inventoryData.HasKey(requiredKeyID);
            Debug.Log($"{hasKey}");
            infoImage.color = hasKey ? infoColors[1] : infoColors[0];
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        public void RefreshKeyCheck(string newKeyID = null)
        {
            if (!string.IsNullOrEmpty(newKeyID))
                requiredKeyID = newKeyID;

            UpdateKeyVisual();
        }
    }
}