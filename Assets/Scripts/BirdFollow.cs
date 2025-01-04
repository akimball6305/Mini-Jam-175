using UnityEngine;

public class BirdFollow : MonoBehaviour
{
    public float speed = 5f; // Adjust speed as needed
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
        Debug.Log("Update is running");  // Log to confirm that Update is running

        if (targetPlayer != null)
        {
            // Move the bird towards the player
            Vector3 direction = targetPlayer.transform.position - transform.position;
            Vector3 moveDirection = direction.normalized * speed * Time.deltaTime;

            // Move bird
            transform.position += moveDirection;
        }
    }

}
