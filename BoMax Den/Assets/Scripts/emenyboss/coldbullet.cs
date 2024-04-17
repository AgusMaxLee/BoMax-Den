using UnityEngine;

public class coldbullet : MonoBehaviour
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
            Destroy(gameObject);  // ���к����������ӵ�
        }
    }
}
