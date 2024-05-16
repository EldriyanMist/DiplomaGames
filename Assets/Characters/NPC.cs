using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour
{
    public Character characterData;

    void OnMouseDown()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.UpdateStatusWindowForCharacter(characterData, transform);
    }
}

