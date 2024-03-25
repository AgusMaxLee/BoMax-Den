using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 100f;

    void Update()
    {
        // Rotate the object around its Z-axis at a constant speed
        transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
    }
}
