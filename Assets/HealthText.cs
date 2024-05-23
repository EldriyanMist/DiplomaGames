using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public float timeToLive = 1f;

    public float floatSpeed = 300f;

    public Vector3 floatDirection = Vector3.up;
    public TextMeshProUGUI textMesh;

    RectTransform rectTransform;

    Color startingColor;

    float timeElapsed = 0f;
    void Start()
    {
        startingColor = textMesh.color;
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        rectTransform.position += floatSpeed * Time.deltaTime * floatDirection;
        
        textMesh.color = new Color(startingColor.r, startingColor.g, startingColor.b, 1 - timeElapsed / timeToLive);

        if (timeElapsed >= timeToLive)
        {
            Destroy(gameObject);
        } 
    }
}
