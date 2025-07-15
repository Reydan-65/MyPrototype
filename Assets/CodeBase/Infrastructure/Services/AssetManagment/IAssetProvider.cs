using CodeBase.Infrastructure.DependencyInjection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.AssetManagment
{
    public interface IAssetProvider : IService
    {
        T GetPrefab<T>(string prefabPath) where T : Object;
        T Instantiate<T>(string prefabPath) where T : Object;
        Task<TType> Load<TType>(string address) where TType : class;
        Task<TType> Load<TType>(AssetReference assetReference) where TType : class;
        void CleanUp();
    }
}