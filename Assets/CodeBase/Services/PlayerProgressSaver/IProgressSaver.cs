using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.PlayerProgressSaver
{
    public interface IProgressSaver : IService
    {
        void AddObject(GameObject gameObject);
        void ClearObjects();
        void LoadProgress();
        void SaveProgress();
        PlayerProgress GetProgress();
    }
}