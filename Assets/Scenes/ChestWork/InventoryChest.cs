using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;
    public int numberOfSlots = 15;
    private GameObject[] slots;

    void Start()
    {
        slots = new GameObject[numberOfSlots];
        for (int i = 0; i < numberOfSlots; i++)
        {
            slots[i] = Instantiate(slotPrefab, transform);
        }
    }

    public void AddItem(Sprite itemIcon)
    {
        for (int i = 0; i < numberOfSlots; i++)
        {
            Image slotImage = slots[i].GetComponent<Image>();
            if (slotImage.sprite == null)
            {
                slotImage.sprite = itemIcon;
                break;
            }
        }
    }
}
