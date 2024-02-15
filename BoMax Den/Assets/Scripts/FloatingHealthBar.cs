using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealthBar(int currentValue, int maxValue)
    {
        slider.value = currentValue;
    }
    void Update()
    {
        
    }
}
