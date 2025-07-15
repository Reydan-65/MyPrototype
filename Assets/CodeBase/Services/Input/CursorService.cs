using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public class CursorService : ICursorService
    {
        private Camera camera;

        public Vector2 ScreenPosition => Input.mousePosition;
        //public Vector2 WorldPosition
        //{
        //    get
        //    {
        //        if (camera == null)
        //        {
        //            Debug.LogError("Camera is not assigned in CursorService!");
        //            return Vector2.zero;
        //        }

        //        Ray ray = camera.ScreenPointToRay(ScreenPosition);

        //        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        //        if (groundPlane.Raycast(ray, out float distance))
        //            return ray.GetPoint(distance);

        //        return Vector3.zero;
        //    }
        //}

        public bool IsVisible
        {
            get => Cursor.visible;
            set => Cursor.visible = value;
        }

        public void SetCamera(Camera camera)
        {
            this.camera = camera;
        }

        public Vector3 GetWorldPositionOnPlane(Vector3 planeOrigin, Vector3 planeNormal)
        {
            if (camera == null)
            {
                Debug.LogError("Camera is not assigned in CursorService!");
                return Vector3.zero;
            }

            Ray ray = camera.ScreenPointToRay(ScreenPosition);
            Plane plane = new Plane(planeNormal, planeOrigin);

            if (plane.Raycast(ray, out float distance))
                return ray.GetPoint(distance);

            return Vector3.zero;
        }

        public void Lock(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}
