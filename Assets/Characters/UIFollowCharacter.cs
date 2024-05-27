using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowCharacter : MonoBehaviour
{
    public Transform characterTransform; // The transform of the character to follow
    public Vector3 offset; // Offset to adjust the position of the UI element
    private Canvas canvas;
    private RectTransform rectTransform;
    private Camera mainCamera;

    void Start()
    {
        canvas = GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;

        if (characterTransform == null)
        {
            Debug.LogError("Character Transform is not assigned.");
        }
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform is not found.");
        }
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera is not found.");
        }
    }

    void Update()
    {
        if (characterTransform != null && rectTransform != null && mainCamera != null)
        {
            // Convert the character's world position to a screen position
            Vector3 screenPos = mainCamera.WorldToScreenPoint(characterTransform.position + offset);

            // Set the position of the RectTransform to follow the character
            rectTransform.position = screenPos;
        }
    }
}



// This script can be attached to a UI element to make it follow a character in the game world.

