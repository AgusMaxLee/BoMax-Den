using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private Transform mainCameraTransform;

    private void Start()
    {
        // Get the main camera's transform
        mainCameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // Face the health bar towards the camera
        if (mainCameraTransform != null)
        {
            transform.LookAt(mainCameraTransform);
            transform.Rotate(0, 180, 0);
        }
    }

    public void UpdateHealthBar(int currentValue, int maxValue)
    {
        slider.value = currentValue;
    }
}
