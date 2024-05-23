using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public string targetTag = "Player";
    public List<GameObject> detectedObjects = new List<GameObject>();

    public Collider2D collider;

    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            detectedObjects.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedObjects.Remove(collision.gameObject);
    }
}
