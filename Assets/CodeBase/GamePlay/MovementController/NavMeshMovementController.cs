using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.GamePlay
{
    public class NavMeshMovementController : IMovementController
    {
        private NavMeshAgent agent;
        public NavMeshMovementController(NavMeshAgent agent) => this.agent = agent;
        public void Move(Vector3 vector3) => agent.Warp(agent.transform.position + vector3);
        public bool IsStoped { get => agent.isStopped; set => agent.isStopped = value; }
        public void Warp(Vector3 position) => agent.Warp(position);
        public void ResetPath() => agent.ResetPath();
        public Vector3 Velocity => agent.velocity;
        public Vector3 Position => agent.transform.position;
    }
}
