using CodeBase.Data;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.PlayerProgressSaver
{
    public class ProgressSaver : IProgressSaver
    {
        public const string ProgressKey = "Progress";
        private string SavePath => Path.Combine(Application.persistentDataPath, "progress_save.json");

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
            if (File.Exists(SavePath))
            {
                string json = File.ReadAllText(SavePath);
                var tempProgress = JsonUtility.FromJson<PlayerProgress>(json);

                progressProvider.PlayerProgress = new PlayerProgress();
                progressProvider.PlayerProgress.CopyFrom(tempProgress);

                Debug.Log($"PROGRESS LOADED FROM SAVE!");
                //Debug.LogWarning(json);
            }
            else
                progressProvider.PlayerProgress = PlayerProgress.GetDefaultProgress();

            progressProvider.PlayerProgress.PrototypeStats.IsChanged();
            progressProvider.PlayerProgress.ProjectileStats.IsChanged();

            foreach (var handler in progressLoadHandlers)
                handler?.LoadProgress(progressProvider.PlayerProgress);
        }

        public void SaveProgress()
        {
            foreach (IProgressBeforeSaveHandler saveHandler in progressBeforeSaveHandlers)
                saveHandler?.UpdateProgressBeforeSave(progressProvider.PlayerProgress);

            string json = JsonUtility.ToJson(progressProvider.PlayerProgress);
            File.WriteAllText(SavePath, json);

            Debug.Log($"PROGRESS SAVED!");
            //string json = PlayerPrefs.GetString(ProgressKey);
            //Debug.LogWarning(json);
        }

        public PlayerProgress GetProgress() => progressProvider.PlayerProgress;

        public void ResetProgress()
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);

            progressProvider.PlayerProgress = PlayerProgress.GetDefaultProgress();
            SaveProgress();
        }
    }
}
