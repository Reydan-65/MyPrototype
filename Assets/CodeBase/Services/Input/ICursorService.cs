using CodeBase.Infrastructure.DependencyInjection;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public interface ICursorService : IService
    {
        bool IsVisible { get; set; }
        Vector2 ScreenPosition { get; }
        //Vector2 WorldPosition { get; }
        void SetCamera(Camera camera);
        Vector3 GetWorldPositionOnPlane(Vector3 planeOrigin, Vector3 planeNormal);
        void Lock(bool locked);
    }
}