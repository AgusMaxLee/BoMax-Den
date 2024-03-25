using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class Collectable : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerState newState;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Switch player state when the collectible is grabbed
            playerController.SwitchPlayerState(newState);

            gameObject.SetActive(true); // Deactivate the collectible object
        }
    }
}