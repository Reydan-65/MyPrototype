using UnityEngine;

namespace CodeBase.GamePlay
{
    public class CharacterMovementController : IMovementController
    {
        private CharacterController controller;
        public CharacterMovementController(CharacterController controller) => this.controller = controller;
        public void Move(Vector3 vector3) => controller.Move(vector3);
        public bool IsStoped { get; set; }
        public void Warp(Vector3 position) => controller.transform.position = position;
        public void ResetPath() { }
        public Vector3 Velocity => controller.velocity;
        public Vector3 Position => controller.transform.position;
    }
}
