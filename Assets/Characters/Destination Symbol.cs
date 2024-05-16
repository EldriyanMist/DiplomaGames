using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI operations


public class Destination : MonoBehaviour
{
    public enum DestinationType
    {
        None,
        Eating,
        Resting,
        Working
    }

    public DestinationType destinationType;
    public Sprite actionSymbol; // The sprite to show when NPC arrives

    // You might want to add a method that will be called by the NPC when it arrive
}

