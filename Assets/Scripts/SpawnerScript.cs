using UnityEngine;
using System.Collections;
public class RandomSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;     // The prefab to spawn
    public int maxSpawnCount = 5;        // Maximum number of spawns
    public float spawnRange = 10f;       // Range within which to spawn the prefab (X and Z)
    public float spawnHeightRange = 5f;  // Range for the Y-axis (height)
    public float spawnDelay = 2f;        // Delay between each spawn

    private int spawnCount = 0;          // Track how many objects have been spawned

    void Start()
    {
        // Start the spawning process
        StartCoroutine(SpawnPrefab());
    }

    private IEnumerator SpawnPrefab()
    {
        while (spawnCount < maxSpawnCount)
        {
            // Generate a random position within the specified range (including Y)
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnRange, spawnRange),  // Random X
                Random.Range(0f, spawnHeightRange),     // Random Y (from 0 to spawnHeightRange)
                Random.Range(-spawnRange, spawnRange)   // Random Z
            );

            // Instantiate the prefab at the random position
            Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);

            // Increment the spawn count
            spawnCount++;

            // Log the spawn count and position (optional for debugging)
            Debug.Log("Spawned " + prefabToSpawn.name + " at position: " + randomPosition);

            // Wait for the specified delay before spawning the next prefab
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
