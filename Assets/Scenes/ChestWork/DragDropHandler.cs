using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image itemImage;
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;

        itemImage.sprite = GetComponent<Image>().sprite;
        itemImage.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvasGroup.transform.localScale.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemImage.gameObject.SetActive(false);
        transform.SetParent(originalParent);
        canvasGroup.blocksRaycasts = true;

        GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;
        if (hitObject != null && hitObject.transform.parent != originalParent && hitObject.transform.parent.GetComponent<Slot>() != null)
        {
            Slot targetSlot = hitObject.transform.parent.GetComponent<Slot>();
            InventoryChest inventory = originalParent.GetComponentInParent<InventoryChest>();
            inventory.SwapItems(originalParent.GetComponent<Slot>(), targetSlot);
        }
    }
}
