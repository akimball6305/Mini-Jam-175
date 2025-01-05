using UnityEngine;
using System.Collections;

public class PowerupSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;  // The powerup prefab to spawn
    public float spawnDelay = 2f;     // Cooldown before the next spawn
    public float spawnHeightOffset = -1f; // Offset to spawn below the player

    private GameObject currentPowerup;  // Track the currently spawned powerup
    private Transform playerTransform;  // Reference to the player's transform
    private bool canSpawn = true;       // Control spawning cooldown

    void Start()
    {
        playerTransform = transform;  // Assume this script is on the player
        StartCoroutine(SpawnPrefab());
    }

    private IEnumerator SpawnPrefab()
    {
        while (true)
        {
            // Check if there is no current powerup and spawning is allowed
            if (currentPowerup == null && canSpawn)
            {
                // Calculate spawn position directly below the player
                Vector3 spawnPosition = playerTransform.position + new Vector3(0f, spawnHeightOffset, 0f);

                // Instantiate the powerup
                currentPowerup = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

                Debug.Log("Spawned " + prefabToSpawn.name + " below player at: " + spawnPosition);
            }

            // Despawn powerup if player gets below it
            if (currentPowerup != null && playerTransform.position.y < currentPowerup.transform.position.y)
            {
                Destroy(currentPowerup);
                currentPowerup = null;
                Debug.Log("Player got below powerup. Despawning...");
                canSpawn = false;
                StartCoroutine(SpawnCooldown());
            }

            yield return null;
        }
    }

    // Cooldown coroutine to enable spawning again
    private IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(spawnDelay);
        canSpawn = true;
    }
}
