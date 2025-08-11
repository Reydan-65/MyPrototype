using CodeBase.Data;
using CodeBase.GamePlay.Interactive;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public class LootService : ILootService
    {
        private const float DropRadius = 3.0f;
        private const float DropHeight = 2.0f;
        private const float DropAnimationDuration = 0.5f;

        private IGameFactory gameFactory;
        private IProgressProvider progressProvider;

        private readonly List<LootItem> activeLootItems = new List<LootItem>();

        public LootService(IGameFactory gameFactory, IProgressProvider progressProvider)
        {
            this.gameFactory = gameFactory;
            this.progressProvider = progressProvider;
        }

        public async Task DropLoot(Vector3 position, LootItemID lootType, int count = 1, string keyID = null)
        {
            var tasks = new List<Task>();

            for (int i = 0; i < count; i++)
            {
                tasks.Add(DropSingleLoot(position, lootType, keyID));
            }

            await Task.WhenAll(tasks);
        }

        private async Task DropSingleLoot(Vector3 position, LootItemID lootType, string keyID = null)
        {
            Vector2 randomOffset = Random.insideUnitCircle * DropRadius;
            Vector3 spawnPosition = position + new Vector3(randomOffset.x, DropHeight, randomOffset.y);

            GameObject lootObject = await gameFactory.CreateLootItemFromPrefab(lootType);
            if (lootObject.TryGetComponent<LootItem>(out var lootItem))
            {
                lootItem.SetColliderEnabled(false);
                lootItem.transform.position = spawnPosition;
                lootItem.GenerateID();
                if (lootItem.TryGetComponent(out KeyLoot key))
                    key.KeyID = keyID;

                await AnimateLootDrop(lootItem);

                lock (activeLootItems)
                    activeLootItems.Add(lootItem);
            }
        }

        private async Task AnimateLootDrop(LootItem lootItem)
        {
            if (lootItem == null) return;

            Transform lootTransform = lootItem.transform;
            Vector3 startPos = lootTransform.position;
            Vector3 endPos = startPos - new Vector3(0, DropHeight, 0);
            float elapsed = 0f;

            while (elapsed < DropAnimationDuration && lootTransform != null)
            {
                float progress = elapsed / DropAnimationDuration;
                lootTransform.position = Vector3.Lerp(startPos, endPos, progress);
                elapsed += Time.deltaTime;
                await Task.Yield();
            }

            if (lootItem != null)
                lootItem.SetColliderEnabled(true);
        }

        public void CleanUp()
        {
            foreach (var loot in activeLootItems)
            {
                if (loot != null)
                    Object.Destroy(loot);
            }

            activeLootItems.Clear();
        }

        public void CleanUpPickedLoot()
        {
            var lootItems = GameObject.FindObjectsOfType<LootItem>(true);
            var progress = progressProvider.PlayerProgress;

            foreach (var lootItem in lootItems)
            {
                // Проверяем, был ли предмет уже подобран
                if (progress.WasLootPicked(lootItem.UniqueID))
                {
                    Object.Destroy(lootItem.gameObject);
                }
            }
        }

        public void SetPauseVisualEffects(bool isActive)
        {
            var lootItems = GameObject.FindObjectsOfType<LootItem>(true);

            foreach (var lootItem in lootItems)
            {
                lootItem.Rotator.enabled = isActive;
            }
        }
    }
}
