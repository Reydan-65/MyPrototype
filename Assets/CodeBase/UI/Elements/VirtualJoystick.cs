using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform stickArea;
    [SerializeField] private RectTransform stick;

    public static Vector2 Value { get; private set; }

    private void Start()
    {
        Value = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Move(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Move(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        stick.anchoredPosition = Vector2.zero;
        Value = Vector2.zero;
    }

    private void Move(Vector2 newPosition)
    {
        stick.position = newPosition;

        if (stick.anchoredPosition.magnitude > stickArea.sizeDelta.x / 2)
        {
            stick.anchoredPosition = stick.anchoredPosition.normalized * (stickArea.sizeDelta / 2);
        }

        Value = new Vector2(stick.anchoredPosition.x, stick.anchoredPosition.y).normalized;
    }
}
