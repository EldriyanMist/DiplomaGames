using UnityEngine;

public class TestChestInteraction : MonoBehaviour
{
    private InventoryPlayer inventoryPlayer;
    public InventoryChest chestInventory;

    void Start()
    {
        // Find the InventoryPlayer component in the children of this GameObject
        inventoryPlayer = GetComponentInChildren<InventoryPlayer>();
        if (inventoryPlayer == null)
        {
            Debug.LogError("InventoryPlayer component not found on the child GameObjects.");
        }

        if (chestInventory == null)
        {
            Debug.LogError("ChestInventory reference is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        if (inventoryPlayer != null && chestInventory != null)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                bool success = inventoryPlayer.PutItemInChest(chestInventory);
                if (success)
                {
                    Debug.Log("Item successfully put in the chest.");
                }
                else
                {
                    Debug.Log("Failed to put item in the chest.");
                }
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                bool success = inventoryPlayer.TakeItemFromChest(chestInventory, 0); // Assume we are taking the item from the first slot for testing
                if (success)
                {
                    Debug.Log("Item successfully taken from the chest.");
                }
                else
                {
                    Debug.Log("Failed to take item from the chest.");
                }
            }
        }
    }
}
