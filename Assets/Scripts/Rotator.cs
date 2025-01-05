using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] Vector3 rotationAxis = Vector3.up; //defaults to y axis
    [SerializeField] float rotationSpeed = 10f; //degrees per second

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
