using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Transform CameraPos;
    void Update()
    {
        transform.position = CameraPos.position;
    }
}
