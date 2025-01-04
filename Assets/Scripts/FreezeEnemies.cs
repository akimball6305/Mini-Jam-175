using UnityEngine;
using System.Collections;

public class FreezeEnemies : MonoBehaviour
{
    private Collider triggerCollider;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        // Cache the collider and mesh renderer
        triggerCollider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger!");

            // Find all BirdFollow instances and start the freeze coroutine
            BirdFollow[] birds = FindObjectsOfType<BirdFollow>();

            foreach (BirdFollow bird in birds)
            {
                StartCoroutine(FreezeBirdTemporarily(bird));
            }

            // Disable the collider to prevent re-entry
            triggerCollider.enabled = false;
            // Hide the object immediately
            meshRenderer.enabled = false;

            // Destroy the GameObject after coroutine completes
            Destroy(gameObject, 3.5f);  // Slight buffer to ensure coroutine finishes
        }
    }

    // Coroutine to freeze and unfreeze birds
    private IEnumerator FreezeBirdTemporarily(BirdFollow bird)
    {
        float originalSpeed = bird.speed;  // Store original speed
        bird.speed = 0;  // Freeze the bird
        Debug.Log($"Freezing bird: {bird.name}");

        yield return new WaitForSeconds(3f);  // Wait for 3 seconds

        bird.speed = originalSpeed;  // Restore original speed
        Debug.Log($"Bird unfrozen: {bird.name}");
    }
}
