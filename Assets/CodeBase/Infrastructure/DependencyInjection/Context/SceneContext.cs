using UnityEngine;

namespace CodeBase.Infrastructure.DependencyInjection
{
    [DefaultExecutionOrder(-10000)]
    public class SceneContext : Context
    {
        private void Awake()
        {
            ProjectContextFactory.TryCreate();

            // ������� ����������� �� ��������� (��� �����������)
            ProjectContext.InjectToInstallers(monoInstallers);

            InstallBindings();

            // ������� ����� ����������� (��� �����������)
            ProjectContext.InjectToAllMonoBehaviour();

            OnBindResolved();
        }
    }
}
