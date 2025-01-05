using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int maxSpawnCount = 5;
    public float spawnRange = 10f;
    public float spawnHeightRange = 5f;
    public float spawnDelay = 2f;
    public float spawnExclusionRadius = 5f;
    public float despawnThresholdY = 10f;  // The Y value above which birds will despawn

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private List<float> spawnTimes = new List<float>();  // Track spawn times for each bird
    private Transform playerTransform;
    private Camera mainCamera;

    void Start()
    {
        playerTransform = transform;
        mainCamera = Camera.main;

        // Start spawning birds immediately
        StartCoroutine(SpawnPrefab());
    }

    private IEnumerator SpawnPrefab()
    {
        // Start spawning immediately
        while (spawnedObjects.Count < maxSpawnCount)
        {
            // Find a valid position to spawn the prefab
            Vector3 randomPosition = Vector3.zero;
            bool validPosition = false;

            // Ensure a valid spawn position
            while (!validPosition)
            {
                Vector3 offset = new Vector3(
                    Random.Range(-spawnRange, spawnRange),
                    Random.Range(0f, spawnHeightRange),
                    Random.Range(-spawnRange, spawnRange)
                );

                randomPosition = playerTransform.position + offset;
                Vector3 viewportPosition = mainCamera.WorldToViewportPoint(randomPosition);

                float distanceToPlayer = Vector3.Distance(randomPosition, playerTransform.position);
                if (viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
                    viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
                    viewportPosition.z > 0 &&
                    distanceToPlayer >= spawnExclusionRadius)
                {
                    validPosition = true;
                }

                // Prevent infinite loop, yield if no valid position found after a few attempts
                if (!validPosition)
                {
                    yield return null; // Yield until the next frame
                }
            }

            // Spawn the new object at the random position
            GameObject newObject = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
            spawnedObjects.Add(newObject);
            spawnTimes.Add(Time.time);  // Track the time of the spawn

            Debug.Log("Spawned " + prefabToSpawn.name + " at position: " + randomPosition);

            // Wait for the next spawn cycle before spawning again
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // Despawns birds if they are too far above the player
    private void DespawnBirds()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt(i);  // Remove null objects (destroyed birds)
                continue;
            }

            // Check if the bird is too far above the player
            float distanceAbovePlayer = spawnedObjects[i].transform.position.y - playerTransform.position.y;

            // Only despawn birds that have been alive long enough (not immediately after spawn)
            if (distanceAbovePlayer >= despawnThresholdY && Time.time - spawnTimes[i] >= spawnDelay)
            {
                // If the bird is too far above the player, despawn it
                Destroy(spawnedObjects[i]);
                spawnedObjects.RemoveAt(i);
                spawnTimes.RemoveAt(i);  // Remove the associated spawn time
                Debug.Log("Bird despawned due to distance above player: " + spawnedObjects[i].name);
            }
        }
    }

    void Update()
    {
        // Only call DespawnBirds periodically, not every frame
        if (Time.frameCount % 5 == 0)  // Call every 5 frames (or adjust as needed)
        {
            DespawnBirds();
        }
    }
}
