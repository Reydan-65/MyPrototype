using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Infrastructure.AssetManagment
{
    public class AssetProvider : IAssetProvider
    {
        private Dictionary<string, AsyncOperationHandle> cacheHandle =
            new Dictionary<string, AsyncOperationHandle>();

        private Dictionary<string, List<AsyncOperationHandle>> allHandles =
            new Dictionary<string, List<AsyncOperationHandle>>();

        public T GetPrefab<T>(string prefabPath) where T : Object
        {
            return Resources.Load<T>(prefabPath);
        }

        public T Instantiate<T>(string prefabPath) where T : Object
        {
            var prefab = GetPrefab<T>(prefabPath);
            return Object.Instantiate(prefab);
        }

        public async Task<TType> Load<TType>(string address) where TType : class =>
            await LoadAsset<TType>(address, address);

        public async Task<TType> Load<TType>(AssetReference assetReference) where TType : class =>
            await LoadAsset<TType>(assetReference.AssetGUID, assetReference);

        private async Task<TType> LoadAsset<TType>(string cacheKey, object loadSource) where TType : class
        {
            if (cacheHandle.TryGetValue(cacheKey, out var cachedHandle))
                return cachedHandle.Result as TType;

            var loadHandle = CreateLoadHandle<TType>(loadSource);
            RegisterHandle(cacheKey, loadHandle);

            return await loadHandle.Task;
        }

        private AsyncOperationHandle<TType> CreateLoadHandle<TType>(object loadSource) where TType : class =>
            loadSource switch
            {
                string address => Addressables.LoadAssetAsync<TType>(address),
                AssetReference reference => Addressables.LoadAssetAsync<TType>(reference),
                _ => throw new System.ArgumentException("Unsupported load source type")
            };

        private void RegisterHandle<TType>(string key, AsyncOperationHandle<TType> handle) where TType : class
        {
            handle.Completed += h => cacheHandle[key] = h;

            if (!allHandles.TryGetValue(key, out var handles))
            {
                handles = new List<AsyncOperationHandle>();
                allHandles[key] = handles;
            }

            handles.Add(handle);
        }

        public void CleanUp()
        {
            foreach (var handles in allHandles.Values)
            {
                foreach (var handle in handles)
                {
                    Addressables.Release(handle);
                }
            }

            allHandles.Clear();
            cacheHandle.Clear();
        }
    }
}