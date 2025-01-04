using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private GameObject targetPlayer;

    void Start()
    {
        // Find the GameObject tagged as "Player"
        targetPlayer = GameObject.FindGameObjectWithTag("Player");

        if (targetPlayer == null)
        {
            Debug.LogError("No GameObject with the 'Player' tag found!");
        }
    }

    void Update()
    {
        if (targetPlayer != null)
        {
            // Make the object look at the player
            transform.LookAt(targetPlayer.transform);
        }
    }
}
