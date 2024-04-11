using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerStatePostProcessing : MonoBehaviour
{
    public PlayerController playerController;
    public PostProcessVolume postProcessVolume;

    private Vignette vignette;

    void Start()
    {
        postProcessVolume.profile.TryGetSettings(out vignette);
    }

    void Update()
    {
        switch (playerController.currentState)
        {
            case PlayerController.PlayerState.Normal:
                SetNormalStateEffects();
                break;
            case PlayerController.PlayerState.Fire:
                SetFireStateEffects();
                break;
            case PlayerController.PlayerState.Water:
                SetWaterStateEffects();
                break;
            case PlayerController.PlayerState.Earth:
                SetEarthStateEffects();
                break;
        }
    }

    void SetNormalStateEffects()

    {
        Debug.Log("changing color");
        vignette.color.value = Color.black;

    }

    void SetFireStateEffects()
    {
        Debug.Log("changing color");
        vignette.color.value = Color.red;

    }

    void SetWaterStateEffects()
    {
        Debug.Log("changing color");
        vignette.color.value = Color.blue;

    }

    void SetEarthStateEffects()
    {
        vignette.color.value = Color.green;
    }
}

