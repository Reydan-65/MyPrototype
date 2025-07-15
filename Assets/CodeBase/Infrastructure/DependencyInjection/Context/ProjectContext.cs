using UnityEngine;

namespace CodeBase.Infrastructure.DependencyInjection
{
    public class ProjectContext : Context
    {
        private static ProjectContext Instance;

        public static bool Initialized => Instance != null;

        private DIContainer container;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                CreateDIContainer();

                // Раздаем зависимости на контейнер (для инсталлеров)
                InjectToInstallers(monoInstallers);

                InstallBindings();

                // Раздаем новые зависимости (для загрузчиков)
                container.InjectToGameObject(gameObject);

                OnBindResolved();

                DontDestroyOnLoad(gameObject);

                return;
            }

            Destroy(gameObject);
        }

        private void CreateDIContainer()
        {
            container = new DIContainer();
            container.RegisterSingle(container);
        }

        public static void InjectToGameObject(GameObject gameObject)
        {
            Instance.container.InjectToGameObject(gameObject);
        }

        public static void InjectToMonoBehaviour(MonoBehaviour monoBehaviour)
        {
            Instance.container.InjectToMonoBehaviour(monoBehaviour);
        }

        public static void InjectToAllMonoBehaviour()
        {
            Instance.container.InjectToAllMonoBehaviour();
        }

        public static void InjectToInstallers(MonoInstaller[] monoInstallers)
        {
            for (int i = 0; i < monoInstallers.Length; i++)
            {
                Instance.container.InjectToMonoBehaviour(monoInstallers[i]);
            }
        }
    }
}
