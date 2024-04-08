using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPositionAndRotation : MonoBehaviour
{
    public Quaternion lockedRotation;

    void Start()
    {
        // Initialize the locked rotation to the object's current rotation,
        // or set this in the Inspector to a specific rotation.
        lockedRotation = transform.rotation;
    }

    void Update()
    {
        // Lock the Z position at 0
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        
        // Lock the rotation
        transform.rotation = lockedRotation;
    }
}

