using UnityEngine;

public class TestInventory : MonoBehaviour
{
    public InventoryChest inventory;
    public Item testItem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.AddItem(testItem);
        }
    }
}