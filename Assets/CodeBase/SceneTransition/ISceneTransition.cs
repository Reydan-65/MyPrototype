using CodeBase.Infrastructure.DependencyInjection;
using System;

namespace CodeBase
{
    public interface ISceneTransition : IService
    {
        float GetDuration(string animationName);
        void LoadSceneWithTransition(string sceneName, Action onLoaded = null);
    }
}