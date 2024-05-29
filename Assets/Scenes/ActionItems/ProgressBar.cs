using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public float duration = 15f; // Duration in seconds for the slider to fill

public void StartProgressBar(float duration)
{
    gameObject.SetActive(true);
    Debug.Log("Use logic executed.");
    StartCoroutine(FillSliderWhenActive(duration));
}

private IEnumerator FillSliderWhenActive(float duration)
    {
        // Wait until the game object is active
        while (!gameObject.activeInHierarchy)
        {
            yield return null;
        }

        StartCoroutine(FillSlider(duration));
    }

private IEnumerator FillSlider(float duration)
{
    float elapsedTime = 0f;
    slider.value = 0f;
    fill.color = gradient.Evaluate(0f);

    while (elapsedTime < duration)
    {
        elapsedTime += Time.deltaTime;
        float normalizedValue = Mathf.Clamp01(elapsedTime / duration);
        slider.value = normalizedValue;
        fill.color = gradient.Evaluate(normalizedValue);
        yield return null;
    }

    slider.value = 1f;
    fill.color = gradient.Evaluate(1f);
    gameObject.SetActive(false);
}

}
