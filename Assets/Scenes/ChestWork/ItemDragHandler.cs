using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged;
    private GameObject dragIcon;
    private RectTransform dragIconTransform;
    private Canvas canvas;
    private Image itemSlotImage;
    private Image swapItemImage;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        itemSlotImage = GetComponent<Image>();
        swapItemImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        dragIcon = new GameObject("Drag Icon");
        dragIcon.transform.SetParent(canvas.transform, false);
        dragIconTransform = dragIcon.AddComponent<RectTransform>();
        dragIconTransform.sizeDelta = itemSlotImage.rectTransform.sizeDelta;

        Image icon = dragIcon.AddComponent<Image>();
        icon.sprite = itemSlotImage.sprite;
        icon.SetNativeSize();
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0.6f); // Semi-transparent

        Image swapIcon = new GameObject("Swap Icon").AddComponent<Image>();
        swapIcon.transform.SetParent(dragIcon.transform, false);
        swapIcon.sprite = swapItemImage.sprite;
        swapIcon.SetNativeSize();
        swapIcon.color = new Color(swapIcon.color.r, swapIcon.color.g, swapIcon.color.b, 0.6f); // Semi-transparent
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragIconTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(dragIcon);
        itemBeingDragged = null;
    }
}
