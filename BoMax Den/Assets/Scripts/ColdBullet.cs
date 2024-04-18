using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdBullet : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.FreezePlayer(1.0f);
            }
            Destroy(gameObject);
        }
    }
}