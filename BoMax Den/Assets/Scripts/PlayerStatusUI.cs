using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    public Slider hpSlider;
    public Slider manaSlider;
    public PlayerStats playerStats;
    public PlayerController playerController;

    void Update()
    {
        UpdateSliders();
    }

    void UpdateSliders()
    {
        hpSlider.value = playerStats.currentHealth;
        manaSlider.value = playerStats.currentMana;
        ScaleBars(playerController.currentState);
    }

    void ScaleBars(PlayerController.PlayerState state)
    {
        switch (state)
        {
            case PlayerController.PlayerState.Normal:
                // Set scale for normal state
                hpSlider.transform.localScale = new Vector3(5f, 0.3f, 0f);
                manaSlider.transform.localScale = new Vector3(5f, 0.2f, 0f);
                break;
            case PlayerController.PlayerState.Fire:
                // Set scale for fire state
                hpSlider.transform.localScale = new Vector3(3.5f, 0.3f, 0f);
                manaSlider.transform.localScale = new Vector3(5f, 0.2f, 0f);
                break;
            case PlayerController.PlayerState.Water:
                // Set scale for water state
                hpSlider.transform.localScale = new Vector3(5f, 0.3f, 0f);
                manaSlider.transform.localScale = new Vector3(6f, 0.2f, 0f);
                break;
            case PlayerController.PlayerState.Earth:
                // Set scale for earth state
                hpSlider.transform.localScale = new Vector3(6f, 0.3f, 0f);
                manaSlider.transform.localScale = new Vector3(5f, 0.2f, 0f);
                break;
        }
    }
    public void UpdateMaxValues(float maxHealth, float maxMana)
    {
        hpSlider.maxValue = maxHealth;
        manaSlider.maxValue = maxMana;
    }
}