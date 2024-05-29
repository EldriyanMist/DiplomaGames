using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject statusWindowPrefab;
    private Dictionary<Character, GameObject> statusWindows = new Dictionary<Character, GameObject>();

    void Start()
    {
        // You can add initialization code here if needed
    }

    void Update()
    {
        // Any global update logic can go here
    }

    public void ShowStatusWindow(Character character, Transform characterTransform)
    {
        if (!statusWindows.ContainsKey(character))
        {
            GameObject newStatusWindow = Instantiate(statusWindowPrefab, transform);
            StatusWindowManager manager = newStatusWindow.GetComponent<StatusWindowManager>();
            manager.UpdateStatusWindow(character);
            UIFollowCharacter follow = newStatusWindow.GetComponent<UIFollowCharacter>();
            follow.characterTransform = characterTransform;
            statusWindows.Add(character, newStatusWindow);
        }

        statusWindows[character].SetActive(true);
    }
}


