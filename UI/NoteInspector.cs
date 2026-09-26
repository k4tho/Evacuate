using NUnit.Framework;
using System.Linq;
using UHFPS.Tools;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoteInspector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    float maxScale = 4f;
    float minScale = 1f;
    float scaleTarget;

    float multiplier = .2f;

    RectTransform[] rectTransforms;

    private RectTransform draggedRect;
    private Vector2 dragOffset;

    private Vector2 mask;

    Canvas canvas;
    void Awake()
    {
        mask = GetComponent<RectTransform>().rect.size;
        rectTransforms = GetComponentsInChildren<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void FixedUpdate()
    {
        scaleTarget -= Input.GetAxisRaw("Mouse ScrollWheel") * multiplier;
        scaleTarget = Mathf.Clamp(scaleTarget, minScale, maxScale);

        UpdateScale();
    }

    private void UpdateScale()
    {
        foreach (RectTransform rt in rectTransforms)
        {
            if (rt.gameObject.name == gameObject.name) continue;

            rt.localScale = new Vector3(scaleTarget, scaleTarget, scaleTarget);
        }
    }

    private void OnEnable()
    {
        scaleTarget = 1f;
        rectTransforms = GetComponentsInChildren<RectTransform>();

        foreach (RectTransform rt in rectTransforms)
        {
            if (rt.gameObject.name == gameObject.name) continue;

            rt.localScale = new Vector3(1,1,1);
            rt.transform.localPosition = Vector3.zero;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        draggedRect = eventData.pointerPressRaycast.gameObject.GetComponent<RectTransform>();

        if (draggedRect == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localMousePos);

        Vector2 objectLocalPos = canvas.transform.InverseTransformPoint(draggedRect.position);
        dragOffset = objectLocalPos - localMousePos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedRect == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localMousePos);

        Vector2 targetPos = localMousePos + dragOffset;
        draggedRect.position = canvas.transform.TransformPoint(targetPos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggedRect = null;
    }
}
