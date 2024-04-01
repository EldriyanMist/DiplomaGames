using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColorChanger : MonoBehaviour
{
    public Color[] possibleColors; // Assign this in the Inspector with the colors you want

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeColor();
    }

    private void ChangeColor()
    {
        // Choose a random color from the possible colors
        Color newColor = possibleColors[Random.Range(0, possibleColors.Length)];
        spriteRenderer.color = newColor; // Apply the color
    }
}