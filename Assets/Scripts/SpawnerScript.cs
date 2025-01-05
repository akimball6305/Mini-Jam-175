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
    public float fallThresholdY = -5f;  // The Y value below which we start spawning (edge fall threshold)
    public float despawnThresholdY = -10f;  // The Y value below which birds will despawn

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private Transform playerTransform;
    private Camera mainCamera;

    private float lastYPosition;
    private bool hasFallen = false;  // Tracks if the player has fallen or started moving

    void Start()
    {
        playerTransform = transform;
        mainCamera = Camera.main;
        lastYPosition = playerTransform.position.y;

        // Initially, don't spawn anything
    }

    void Update()
    {
        // Check if the player falls below a certain Y threshold (indicating they've fallen off the edge)
        if (!hasFallen && playerTransform.position.y <= fallThresholdY)
        {
            hasFallen = true;
            StartCoroutine(SpawnPrefab());  // Start spawning once the player falls
        }

        // Despawn birds that are too far below the player
        DespawnBirds();
    }

    private IEnumerator SpawnPrefab()
    {
        while (hasFallen && spawnedObjects.Count < maxSpawnCount)
        {
            // Find a valid position to spawn the prefab
            Vector3 randomPosition = Vector3.zero;
            bool validPosition = false;

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
            }

            // Spawn the new object at the random position
            GameObject newObject = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
            spawnedObjects.Add(newObject);

            Debug.Log("Spawned " + prefabToSpawn.name + " at position: " + randomPosition);

            // Wait for the next spawn cycle
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // Despawns birds if the player is too far below them
    private void DespawnBirds()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt(i);  // Remove null objects (destroyed birds)
                continue;
            }

            // Check if the bird is too far below the player
            float distanceBelowPlayer = playerTransform.position.y - spawnedObjects[i].transform.position.y;
            if (distanceBelowPlayer >= despawnThresholdY)
            {
                // If the bird is too far below, despawn it
                Destroy(spawnedObjects[i]);
                spawnedObjects.RemoveAt(i);
                Debug.Log("Bird despawned due to distance: " + spawnedObjects[i].name);
            }
        }
    }
}
