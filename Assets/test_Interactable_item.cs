using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_Interactable_item : MonoBehaviour
{
    // Start is called before the first frame update
    public string itemName = "Statue";

    void Start()
    {
        // Find the specific item by name
        GameObject itemObject = GameObject.Find(itemName);
        if (itemObject != null)
        {
            ItemInteractable item = itemObject.GetComponent<ItemInteractable>();
            if (item != null)
            {
                List<string> availableActions = item.GetAvailableActions();
                Debug.Log($"Available actions for {itemName}:");
                foreach (string action in availableActions)
                {
                    Debug.Log(action);
                }
            }
            else
            {
                Debug.LogWarning($"The object {itemName} does not have an ItemInteractable component.");
            }
        }
        else
        {
            Debug.LogWarning($"No object named {itemName} found in the scene.");
        }
    }
}
