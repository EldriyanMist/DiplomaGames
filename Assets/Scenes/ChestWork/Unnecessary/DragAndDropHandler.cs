using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static DragAndDropHandler instance;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Image itemIcon;
    private Image itemBackground;

    private void Awake()
    {
        instance = this;
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        itemIcon = transform.Find("SwapItem").GetComponent<Image>();
        itemBackground = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        CreateProjection();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        DestroyProjection();
        // Add logic to handle item drop
    }

    private void CreateProjection()
    {
        GameObject projection = Instantiate(gameObject, canvas.transform);
        projection.name = "Projection";
        Destroy(projection.GetComponent<DragAndDropHandler>());
        projection.GetComponent<CanvasGroup>().blocksRaycasts = false;
        projection.GetComponent<Image>().raycastTarget = false;
        projection.transform.Find("SwapItem").GetComponent<Image>().raycastTarget = false;
        projection.transform.SetAsLastSibling();
    }

    private void DestroyProjection()
    {
        GameObject projection = GameObject.Find("Projection");
        if (projection != null)
        {
            Destroy(projection);
        }
    }
}
