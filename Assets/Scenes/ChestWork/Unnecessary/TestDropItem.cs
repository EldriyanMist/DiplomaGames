using UnityEngine;

public class TestDropItem : MonoBehaviour
{
    private InventoryPlayer inventoryPlayer;

    void Start()
    {
        inventoryPlayer = GetComponent<InventoryPlayer>();
        if (inventoryPlayer == null)
        {
            Debug.LogError("InventoryPlayer component not found on this GameObject.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            inventoryPlayer.DropItem();
        }
    }
}
