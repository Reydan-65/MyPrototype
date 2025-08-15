using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public class CursorService : ICursorService
    {
        private Camera camera;
        public Vector2 ScreenPosition => Input.mousePosition;
        public bool IsVisible
        {
            get => Cursor.visible;
            set => Cursor.visible = value;
        }

        public void SetCamera(Camera camera) => this.camera = camera;

        public Vector3 GetWorldPositionOnPlane(Vector3 planeOrigin, Vector3 planeNormal)
        {
            if (camera == null) return Vector3.zero;

            Ray ray = camera.ScreenPointToRay(ScreenPosition);
            Plane plane = new Plane(planeNormal, planeOrigin);

            if (plane.Raycast(ray, out float distance))
                return ray.GetPoint(distance);

            return Vector3.zero;
        }

        public void Lock(bool locked) 
            => Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
