using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
        volumeSlider.value = savedVolume;
        UpdateVolume(savedVolume);
    }

    public void OnVolumeChanged(float value)
    {
        UpdateVolume(value);
        PlayerPrefs.SetFloat("Volume", value); // Save the volume setting
    }

    private void UpdateVolume(float value)
    {
        sfxAudioSource.volume = value;
        musicAudioSource.volume = value;
    }
}
