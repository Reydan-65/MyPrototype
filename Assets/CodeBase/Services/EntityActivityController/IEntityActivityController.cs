using CodeBase.Infrastructure.DependencyInjection;
using UnityEngine;

namespace CodeBase.Services.EntityActivityController
{
    public interface IEntityActivityController : IService
    {
        void MoveCameraToTarget(Transform target);
        void SetEntitiesActive(bool isActive);
    }
}