using CodeBase.Data;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.PlayerProgressSaver
{
    public class ProgressSaver : IProgressSaver
    {
        private const string ProgressKey = "Progress";

        private IProgressProvider progressProvider;

        private List<IProgressBeforeSaveHandler> progressBeforeSaveHandlers;
        private List<IProgressLoadHandler> progressLoadHandlers;

        public ProgressSaver(IProgressProvider progressProvider)
        {
            this.progressProvider = progressProvider;

            progressBeforeSaveHandlers = new List<IProgressBeforeSaveHandler>();
            progressLoadHandlers = new List<IProgressLoadHandler>();
        }

        public void AddObject(GameObject gameObject)
        {
            foreach (IProgressLoadHandler loadHandler in gameObject.GetComponentsInChildren<IProgressLoadHandler>())
            {
                if (loadHandler != null)
                {
                    if (progressLoadHandlers.Contains(loadHandler) == false)
                        progressLoadHandlers.Add(loadHandler);
                }
            }

            foreach (IProgressBeforeSaveHandler saveHandler in gameObject.GetComponentsInChildren<IProgressBeforeSaveHandler>())
            {
                if (saveHandler != null)
                {
                    if (progressBeforeSaveHandlers.Contains(saveHandler) == false)
                        progressBeforeSaveHandlers.Add(saveHandler);
                }
            }
        }

        public void ClearObjects()
        {
            progressBeforeSaveHandlers.Clear();
            progressLoadHandlers.Clear();
        }

        public void LoadProgress()
        {
            if (PlayerPrefs.HasKey(ProgressKey) == false)
                progressProvider.PlayerProgress = PlayerProgress.GetDefaultProgress();
            else
            {
                progressProvider.PlayerProgress = JsonUtility.FromJson<PlayerProgress>(PlayerPrefs.GetString(ProgressKey));
                Debug.Log($"PROGRESS LOADED: FROM SAVE!");
            }

            foreach (IProgressLoadHandler loadHandler in progressLoadHandlers)
            {
                loadHandler?.LoadProgress(progressProvider.PlayerProgress);
            }
        }

        public void SaveProgress()
        {
            foreach (IProgressBeforeSaveHandler saveHandler in progressBeforeSaveHandlers)
            {
                saveHandler?.UpdateProgressBeforeSave(progressProvider.PlayerProgress);
            }

            PlayerPrefs.SetString(ProgressKey, JsonUtility.ToJson(progressProvider.PlayerProgress));

            Debug.Log("PROGRESS SAVED!");
        }

        public PlayerProgress GetProgress()
        {
            return progressProvider.PlayerProgress;
        }
    }
}
