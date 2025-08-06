using UnityEngine;

namespace CodeBase.GamePlay
{
    public interface IMovementController
    {
        void Move(Vector3 vector3);
        bool IsStoped { get; set; }
        void Warp(Vector3 position);
        void ResetPath();
        Vector3 Velocity { get; }
        Vector3 Position { get; }
    }
}
