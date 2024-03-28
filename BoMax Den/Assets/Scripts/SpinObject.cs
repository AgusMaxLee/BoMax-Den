using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 100f;
    [SerializeField] private RotationAxis rotationAxis = RotationAxis.Z;

    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    void Update()
    {
        Vector3 rotationAxisVector = Vector3.zero;

        // Set the appropriate rotation axis vector based on the selected axis
        switch (rotationAxis)
        {
            case RotationAxis.X:
                rotationAxisVector = Vector3.right;
                break;
            case RotationAxis.Y:
                rotationAxisVector = Vector3.up;
                break;
            case RotationAxis.Z:
                rotationAxisVector = Vector3.forward;
                break;
        }

        // Rotate the object around the specified axis at a constant speed
        transform.Rotate(rotationAxisVector, spinSpeed * Time.deltaTime);
    }
}
