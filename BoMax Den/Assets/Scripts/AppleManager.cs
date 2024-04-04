using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppleManager : MonoBehaviour
{
    public static AppleManager instance;

    public GameObject appleCounterDisplay;
    private int appleCount;
    public int healingAmount = 25;
    public float healCooldown = 1.5f; 
    private float lastHealTime; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        appleCount = 0;
        UpdateAppleCounterDisplay();
        lastHealTime = -healCooldown; 
    }

    void Update()
    {
        if (InputManager.isHealInput && CanHeal() && Time.time - lastHealTime >= healCooldown)
        {
            HealPlayer();
            lastHealTime = Time.time; 
        }
    }

    public void CollectApple()
    {
        appleCount++;
        UpdateAppleCounterDisplay();
    }

    private void HealPlayer()
    {
        if (CanHeal())
        {
            appleCount--;
            UpdateAppleCounterDisplay();

            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.currentHealth += healingAmount;
                Debug.Log("Player healed by " + healingAmount + " HP.");
            }
            else
            {
                Debug.LogWarning("PlayerStats component not found.");
            }
        }
    }

    private bool CanHeal()
    {
        return appleCount > 0;
    }

    public int GetAppleCount()
    {
        return appleCount;
    }

    private void UpdateAppleCounterDisplay()
    {
        if (appleCounterDisplay != null)
        {
            Text textComponent = appleCounterDisplay.GetComponent<Text>();
            if (textComponent != null)
            {
                textComponent.text = appleCount.ToString();
            }
        }
    }
}
