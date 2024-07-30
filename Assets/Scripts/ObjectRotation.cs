using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    public float rotationSpeed = 100f; // adjust to your liking
    public Transform targetObject; // assign your object in the Inspector

    void Update()
    {
        targetObject.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
