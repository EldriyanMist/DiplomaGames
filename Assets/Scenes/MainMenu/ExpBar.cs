using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private int currentExp = 0;  // Start from 0 experience
    public int maxExp = 100;  // Set the maximum experience to 100

    private void Start()
    {
        SetMaxExp(maxExp);
        currentExp = 0;  // Start with 0 experience
        StartCoroutine(IncreaseExpOverTime());
    }

    public void SetMaxExp(int exp)
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

        slider.maxValue = exp;
        slider.value = 0;  // Start with slider at 0

        fill.color = gradient.Evaluate(0f);  // Start with the initial color of the gradient
    }

    public void SetExp(int exp)
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

        slider.value = exp;
        fill.color = gradient.Evaluate(slider.normalizedValue);  // Update the fill color based on the current exp
    }

    private IEnumerator IncreaseExpOverTime()
    {
        // This loop will keep running as long as the experience is less than the maximum
        while (currentExp < maxExp)
        {
            yield return new WaitForSeconds(1);  // Wait for one second
            currentExp += 10;  // Increase experience by 10
            SetExp(currentExp);  // Update the experience bar
        }
    }
}
