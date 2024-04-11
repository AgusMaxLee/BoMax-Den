using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractUI : MonoBehaviour
{
    public GameObject canvasImage;
    private Transform mainCameraTransform;

    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
        HideUI();
    }

    private void LateUpdate()
    {
        transform.LookAt(mainCameraTransform);
        transform.Rotate(0, 180, 0);
    }
    public void ShowUI()
    {
        canvasImage.SetActive(true);
    }
    public void HideUI()
    {
        canvasImage.SetActive(false);
    }
}
