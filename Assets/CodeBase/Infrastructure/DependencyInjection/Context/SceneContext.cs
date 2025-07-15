using UnityEngine;

namespace CodeBase.Infrastructure.DependencyInjection
{
    [DefaultExecutionOrder(-10000)]
    public class SceneContext : Context
    {
        private void Awake()
        {
            ProjectContextFactory.TryCreate();

            // Раздаем зависимости на контейнер (для инсталлеров)
            ProjectContext.InjectToInstallers(monoInstallers);

            InstallBindings();

            // Раздаем новые зависимости (для загрузчиков)
            ProjectContext.InjectToAllMonoBehaviour();

            OnBindResolved();
        }
    }
}
