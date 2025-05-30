using UnityEngine;
using UnityEngine.EventSystems;

public class RingRotator : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Vector2 screenCenter;
    private float lastAngle;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        screenCenter = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, rectTransform.position);
        lastAngle = GetAngle(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        float currentAngle = GetAngle(eventData.position);
        float delta = Mathf.DeltaAngle(lastAngle, currentAngle);

        rectTransform.Rotate(0f, 0f, delta);
        lastAngle = currentAngle;
    }

    private float GetAngle(Vector2 screenPos)
    {
        Vector2 dir = screenPos - screenCenter;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
}
