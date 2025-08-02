using CodeBase.Infrastructure.DependencyInjection;
using System;
using UnityEngine;

namespace CodeBase.Services.EntityActivityController
{
    public interface IEntityActivityController : IService
    {
        void MoveCameraToTarget(Transform target, float duration, Action onComplete = null);
        void SetEntitiesActive(bool isActive);
    }
}