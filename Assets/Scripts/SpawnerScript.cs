using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;     // The prefab to spawn
    public int maxSpawnCount = 5;        // Maximum number of spawns
    public float spawnRange = 10f;       // Range within which to spawn the prefab (X and Z)
    public float spawnHeightRange = 5f;  // Range for the Y-axis (height)
    public float spawnDelay = 2f;        // Delay between each spawn
    public float spawnExclusionRadius = 5f;  // Radius around the player where spawns are not allowed

    private List<GameObject> spawnedObjects = new List<GameObject>();  // Track spawned objects
    private Transform playerTransform;   // Reference to the player's transform

    void Start()
    {
        // Get the player's transform (script is attached to player body)
        playerTransform = transform;

        // Start the spawning process
        StartCoroutine(SpawnPrefab());
    }

    private IEnumerator SpawnPrefab()
    {
        while (true) // Keep spawning indefinitely
        {
            // Check if the current spawn count is less than the maximum spawn count
            if (spawnedObjects.Count < maxSpawnCount)
            {
                Vector3 randomPosition = Vector3.zero;
                bool validPosition = false;

                // Try to generate a valid position outside the exclusion radius
                while (!validPosition)
                {
                    // Generate a random offset relative to the player's current position
                    Vector3 offset = new Vector3(
                        Random.Range(-spawnRange, spawnRange),  // Random X
                        Random.Range(0f, spawnHeightRange),     // Random Y
                        Random.Range(-spawnRange, spawnRange)   // Random Z
                    );

                    // Calculate the spawn position based on the player's current position
                    randomPosition = playerTransform.position + offset;

                    // Check if the position is outside the exclusion radius
                    float distanceToPlayer = Vector3.Distance(randomPosition, playerTransform.position);
                    if (distanceToPlayer >= spawnExclusionRadius)
                    {
                        validPosition = true;
                    }
                }

                // Instantiate the prefab at the calculated position
                GameObject newObject = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);

                // Add the spawned object to the list
                spawnedObjects.Add(newObject);

                Debug.Log("Spawned " + prefabToSpawn.name + " at position: " + randomPosition);

                // Wait for the specified delay before spawning the next prefab
                yield return new WaitForSeconds(spawnDelay);
            }

            // Check if any spawned object is destroyed, and replace it
            for (int i = spawnedObjects.Count - 1; i >= 0; i--)
            {
                if (spawnedObjects[i] == null)
                {
                    spawnedObjects.RemoveAt(i);
                    Debug.Log("Object destroyed. Replacing it...");
                    StartCoroutine(SpawnPrefab());
                    break;
                }
            }

            yield return null;
        }
    }
}
