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

    private List<GameObject> spawnedObjects = new List<GameObject>();  // Track spawned objects

    void Start()
    {
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
                // Generate a random position within the specified range (including Y)
                Vector3 randomPosition = new Vector3(
                    Random.Range(-spawnRange, spawnRange),  // Random X
                    Random.Range(0f, spawnHeightRange),     // Random Y
                    Random.Range(-spawnRange, spawnRange)   // Random Z
                );

                // Instantiate the prefab at the random position
                GameObject newObject = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);

                // Add the spawned object to the list
                spawnedObjects.Add(newObject);

                // Log the spawn count and position (optional for debugging)
                Debug.Log("Spawned " + prefabToSpawn.name + " at position: " + randomPosition);

                // Wait for the specified delay before spawning the next prefab
                yield return new WaitForSeconds(spawnDelay);
            }

            // Check if any spawned object is destroyed, and replace it
            for (int i = spawnedObjects.Count - 1; i >= 0; i--)
            {
                if (spawnedObjects[i] == null) // If the object is destroyed
                {
                    // Remove it from the list
                    spawnedObjects.RemoveAt(i);

                    // Log that an object was destroyed and will be replaced
                    Debug.Log("Object destroyed. Replacing it...");

                    // Spawn a new object to replace the destroyed one
                    StartCoroutine(SpawnPrefab());

                    break; // Exit the loop after handling destruction
                }
            }

            // Wait for a short amount of time before checking again
            yield return null;
        }
    }
}
