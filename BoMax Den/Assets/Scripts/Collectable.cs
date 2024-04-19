using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;


public class Collectable : MonoBehaviour, Interactor.IInteractable
{
    public PlayerController playerController;
    public PlayerState newState;
    [SerializeField] private AudioClip pickupSound;


    public void Interact()
    {
        Debug.Log("is interacting");
        // Request to switch player state when the collectible is grabbed
        playerController.RequestStateSwitch(newState);
        AudioManager.Instance.PlaySound(pickupSound);

    }
}