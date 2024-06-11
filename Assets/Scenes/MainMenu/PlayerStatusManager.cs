using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class StatusManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject statusWindow;
    public Slider healthSlider;
    public Gradient healthGradient;
    public Image healthFill;
    public Slider hungerSlider;
    public Gradient hungerGradient;
    public Image hungerFill;
    public Slider expSlider;
    public Gradient expGradient;
    public Image expFill;
    public TextMeshProUGUI textLvlName;
    public TextMeshProUGUI agilityText;
    public TextMeshProUGUI enduranceText;
    public TextMeshProUGUI strengthText;
    public GameObject npcIcon;

    private int currentHealth;
    private int maxHealth = 100;
    private int currentHunger;
    private int maxHunger = 100;
    private int currentExp;
    private int maxExp = 100;
    private int currentLevel = 1;
    private int agility = 10;
    private int endurance = 10;
    private int strength = 10;

    private void Start()
    {
        SetMaxHealth(strength * 10);
        SetMaxHunger(endurance * 10);
        SetMaxExp(maxExp);

        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentExp = 0;

        StartCoroutine(DecreaseHungerOverTime());
        StartCoroutine(IncreaseExpOverTime());

        UpdateLevelText();
        UpdateStatsText();

        statusWindow.SetActive(false);
        AddEventTriggerToNPCIcon();
    }

    #region Health Management
    public void SetMaxHealth(int health)
    {
        maxHealth = health;
        healthSlider.maxValue = health;
        healthSlider.value = Mathf.Min(healthSlider.value, health);
        healthFill.color = healthGradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        healthSlider.value = health;
        healthFill.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }

    public void DecreaseHealth(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        SetHealth(currentHealth);
    }
    #endregion

    #region Hunger Management
    public void SetMaxHunger(int hunger)
    {
        maxHunger = hunger;
        hungerSlider.maxValue = hunger;
        hungerSlider.value = Mathf.Min(hungerSlider.value, hunger);
        hungerFill.color = hungerGradient.Evaluate(1f);
    }

    public void SetHunger(int hunger)
    {
        hungerSlider.value = hunger;
        hungerFill.color = hungerGradient.Evaluate(hungerSlider.normalizedValue);
    }

    private IEnumerator DecreaseHungerOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            currentHunger = Mathf.Max(currentHunger - 5, 0);
            SetHunger(currentHunger);

            if (currentHunger == 0)
            {
                DecreaseHealth(10);
            }
        }
    }
    #endregion

    #region Experience Management
    public void SetMaxExp(int exp)
    {
        expSlider.maxValue = exp;
        expSlider.value = 0;
        expFill.color = expGradient.Evaluate(0f);
    }

    public void SetExp(int exp)
    {
        expSlider.value = exp;
        expFill.color = expGradient.Evaluate(expSlider.normalizedValue);

        if (exp >= maxExp)
        {
            LevelUp();
        }
    }

    public void IncreaseExp(int amount)
    {
        currentExp = Mathf.Min(currentExp + amount, maxExp);
        SetExp(currentExp);
    }

    private void LevelUp()
    {
        currentExp = 0;
        maxExp += 100;
        currentLevel += 1;

        SetMaxExp(maxExp);
        SetExp(currentExp);
        UpdateLevelText();
        IncreaseRandomStat();
    }

    private IEnumerator IncreaseExpOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            IncreaseExp(10);
        }
    }
    #endregion

    #region Level Management
    private void UpdateLevelText()
    {
        textLvlName.text = $"NPC-1 - LVL. {currentLevel}";
    }
    #endregion

    #region Stats Management
    private void UpdateStatsText()
    {
        agilityText.text = $"Agility: {agility}";
        enduranceText.text = $"Endurance: {endurance}";
        strengthText.text = $"Strength: {strength}";
    }

    private void IncreaseRandomStat()
    {
        int randomStat = Random.Range(0, 3);

        switch (randomStat)
        {
            case 0:
                agility += 1;
                break;
            case 1:
                endurance += 1;
                SetMaxHunger(endurance * 10);
                break;
            case 2:
                strength += 1;
                SetMaxHealth(strength * 10);
                break;
        }

        UpdateStatsText();
    }
    #endregion

    #region UI Toggle
    public void ToggleStatusWindow()
    {
        statusWindow.SetActive(!statusWindow.activeSelf);
    }

    private void AddEventTriggerToNPCIcon()
    {
        EventTrigger trigger = npcIcon.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = npcIcon.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((eventData) => { ToggleStatusWindow(); });
        trigger.triggers.Add(entry);
    }
    #endregion
}
