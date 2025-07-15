using CodeBase.Infrastructure.Services;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.Scene
{
    public class SceneLoader : ISceneLoader
    {
        private ICoroutineRunner coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            this.coroutineRunner = coroutineRunner;
        }

        public void Load(string name, Action onLoaded = null)
        {
            coroutineRunner.StartCoroutine(LoadAsinc(name, onLoaded));
        }

        private IEnumerator LoadAsinc(string name, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == name)
            {
                yield return null;
                onLoaded?.Invoke();
                yield break;
            }

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);

            while (asyncOperation.isDone == false)
            {
                yield return null;
            }

            onLoaded?.Invoke();
        }
    }
}
