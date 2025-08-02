using System;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public class InputService : IInputService
    {
        private const string HorizontalAxisName = "Horizontal";
        private const string VerticalAxisName = "Vertical";

        private bool enable = true;

        private Vector2 GetMovementAxis()
        {
            if (enable == false) return Vector2.zero;

            return new Vector2(Input.GetAxis(HorizontalAxisName), Input.GetAxis(VerticalAxisName));
        }

        public Vector2 MovementAxis => GetMovementAxis();

        public bool AttackInput => enable && Input.GetMouseButton(0);
        public bool AimInput => enable && Input.GetMouseButton(1);
        public bool RunInput => enable && Input.GetKey(KeyCode.LeftShift);
        public bool DashInput => enable && Input.GetKeyDown(KeyCode.Space);
        public bool AbilityInput => enable && Input.GetKeyDown(KeyCode.F);
        public bool HealInput => enable && Input.GetKeyDown(KeyCode.Q);
        public bool UseInput => enable && Input.GetKeyDown(KeyCode.E);
        public bool EscapeInput => Input.GetKeyDown(KeyCode.Escape);

        public bool Enable { get => enable; set => enable = value; }
    }
}
