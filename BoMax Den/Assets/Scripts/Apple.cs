using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Apple : MonoBehaviour, Interactor.IInteractable
{
    public GameObject appleCounterDisplay;

    public void Interact()
    {
        Debug.Log("APPLE");
        AppleManager.instance.CollectApple();
        Destroy(gameObject);
    }

    private void UpdateAppleCounterDisplay()
    {
        if (appleCounterDisplay != null)
        {
            Text textComponent = appleCounterDisplay.GetComponent<Text>();
            if (textComponent != null)
            {
                textComponent.text = AppleManager.instance.GetAppleCount().ToString();
            }
        }
    }
}

