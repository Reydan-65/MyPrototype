using System.Threading.Tasks;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    public Transform Target => target;
    public Vector3 Offset {get => offset; set => offset = value; }

    private void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.transform.position;
    }

    public async Task MoveToTarget(Transform newTarget, float duration, Vector3? customOffset = null)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = newTarget.position + (customOffset?? Vector3.zero);

        float elapsed = 0;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition,endPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            await Task.Yield();
        }

        target = newTarget;
        if (customOffset.HasValue) Offset = customOffset.Value;
    }

    public void SetTarget(Transform target) => this.target = target;
}
