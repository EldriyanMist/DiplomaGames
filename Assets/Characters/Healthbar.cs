using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
   public Slider slider;
   public Gradient gradient;

   public Image fill;

   private int currentHealth; 
   public int maxHealth = 100;  
  private void Start()
    {
        SetMaxHealth(maxHealth);
        currentHealth = maxHealth; // Initialize currentHealth
    }
  public void SetMaxHealth(int health)
{
    if (slider == null)
    {
        Debug.LogError("Slider is not set on " + gameObject.name);
        return;
    }
    if (fill == null)
    {
        Debug.LogError("Fill Image is not set on " + gameObject.name);
        return;
    }
    if (gradient == null)
    {
        Debug.LogError("Gradient is not set on " + gameObject.name);
        return;
    }

    slider.maxValue = health;
    slider.value = health;

    fill.color = gradient.Evaluate(1f);
}

public void SetHealth(int health)
{
    if (slider == null)
    {
        Debug.LogError("Slider is not set on " + gameObject.name);
        return;
    }
    if (fill == null)
    {
        Debug.LogError("Fill Image is not set on " + gameObject.name);
        return;
    }
    if (gradient == null)
    {
        Debug.LogError("Gradient is not set on " + gameObject.name);
        return;
    }

    slider.value = health;
    fill.color = gradient.Evaluate(slider.normalizedValue);
}


}