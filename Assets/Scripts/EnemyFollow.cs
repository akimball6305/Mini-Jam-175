using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    
    public GameObject Player;
    public GameObject Target;
    public float speed;

    void Update()
    {
        Target.transform.position = Vector3.MoveTowards(Target.transform.position, Player.transform.position, speed);
    }
}

