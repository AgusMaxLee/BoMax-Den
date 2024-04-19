using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    public Slider sensitivitySlider;
    public PlayerLook playerLookScript;

    private void Start()
    {
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
        sensitivitySlider.value = savedSensitivity;
        UpdateSensitivity(savedSensitivity);
    }

    public void OnSensitivityChanged(float value)
    {
        UpdateSensitivity(value);
        PlayerPrefs.SetFloat("Sensitivity", value); // Save the sensitivity setting
    }

    private void UpdateSensitivity(float value)
    {
        playerLookScript.sensitivity = value;
    }
}
