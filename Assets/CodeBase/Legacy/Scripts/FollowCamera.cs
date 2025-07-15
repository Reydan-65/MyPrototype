using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.transform.position;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
