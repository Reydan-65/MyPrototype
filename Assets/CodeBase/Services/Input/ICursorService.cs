using CodeBase.Infrastructure.DependencyInjection;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public interface ICursorService : IService
    {
        bool IsVisible { get; set; }
        Vector2 ScreenPosition { get; }
        Vector3 GetWorldPositionOnPlane(Vector3 planeOrigin, Vector3 planeNormal);
        void SetCamera(Camera camera);
        void Lock(bool locked);
    }
}