using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectibleType
    {
        Earth,
        Water,
        Fire
    }

    public CollectibleType collectibleType;
    public Color collectibleColor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);

            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                switch (collectibleType)
                {
                    case CollectibleType.Earth:
                        playerController.ChangePlayerColor(Color.green);
                        break;
                    case CollectibleType.Water:
                        playerController.ChangePlayerColor(Color.blue);
                        break;
                    case CollectibleType.Fire:
                        playerController.ChangePlayerColor(Color.red);
                        break;
                }
            }
        }
    }
}
