using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Scene;
using CodeBase.Infrastructure.Services;
using System;
using System.Collections;
using UnityEngine;

namespace CodeBase
{
    public class SceneTransition : MonoBehaviour, ISceneTransition
    {
        [SerializeField] private Animator animator;

        private ISceneLoader sceneLoader;
        private ICoroutineRunner coroutineRunner;

        [Inject]
        public void Construct(
            ISceneLoader sceneLoader,
            ICoroutineRunner coroutineRunner)
        {
            if (sceneLoader == null || coroutineRunner == null) return;

            this.sceneLoader = sceneLoader;
            this.coroutineRunner = coroutineRunner;
        }

        private void Start()
        {
            ResetAllTriggers();
            PlayStartAnimation();
        }

        public void LoadSceneWithTransition(string sceneName, Action onLoaded = null)
        {
            coroutineRunner.StartCoroutine(LoadSceneRoutine(sceneName, onLoaded));
        }

        private void PlayStartAnimation()
        {
            animator.SetBool("Start", true);
            animator.SetBool("End", false);
            Debug.Log("Start animation triggered");
        }

        private IEnumerator LoadSceneRoutine(string sceneName, Action onLoaded)
        {
            animator.SetBool("End", true);
            animator.SetBool("Start", false);

            yield return new WaitForSeconds(GetDuration("sceneTransition_End"));

            bool loadingCompleted = false;
            sceneLoader.Load(sceneName, () => loadingCompleted = true);

            while (!loadingCompleted)
                yield return null;
            animator.SetBool("End", false);
            animator.SetBool("Start", false);
            onLoaded?.Invoke();
            animator.SetBool("Start", true);
        }

        public float GetDuration(string animationName)
        {
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == animationName)
                    return clip.length;
            }

            return 0f;
        }

        private void ResetAllTriggers()
        {
            foreach (var param in animator.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Trigger)
                    animator.ResetTrigger(param.name);
            }
        }
    }
}