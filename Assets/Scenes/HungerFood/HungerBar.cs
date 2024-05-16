using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hungerbar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private int currentHunger;
    public int maxHunger = 100;

    private void Start()
    {
        SetMaxHunger(maxHunger);
        currentHunger = maxHunger;
        StartCoroutine(DecreaseHungerOverTime());
    }

    public void SetMaxHunger(int hunger)
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

        slider.maxValue = hunger;
        slider.value = hunger;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHunger(int hunger)
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

        slider.value = hunger;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    private IEnumerator DecreaseHungerOverTime()
    {
        while (currentHunger > 0)
        {
            yield return new WaitForSeconds(1);
            currentHunger--;
            SetHunger(currentHunger);
        }
    }
}