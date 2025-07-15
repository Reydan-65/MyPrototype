using CodeBase.Infrastructure.DependencyInjection;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public interface IInputService : IService
    {
        bool AbilityInput { get; }
        bool AttackInput { get; }
        bool AimInput { get; }
        bool Enable { get; set; }
        bool HealInput { get; }
        bool DashInput { get; }
        Vector2 MovementAxis { get; }
        bool RunInput { get; }
    }
}