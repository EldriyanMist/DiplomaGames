using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemInteractable : MonoBehaviour
{
    public List<Action> actions; // List of actions available for this item

    private InteractionSystem interactionSystem;

    void Start()
    {
        interactionSystem = FindObjectOfType<InteractionSystem>();
    }

    void OnMouseDown()
    {
        interactionSystem.ShowInteractionMenu(gameObject, actions);
    }
}

