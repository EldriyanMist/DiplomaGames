using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnclickInfo : MonoBehaviour
{
    public GameObject statusWindow;

    void Start()
    {
        // Ensure the status window is inactive when the game starts
        statusWindow.SetActive(false);
    }

    void OnMouseDown()
    {
        // Toggle the visibility of the status window
        statusWindow.SetActive(!statusWindow.activeSelf);
    }
}
