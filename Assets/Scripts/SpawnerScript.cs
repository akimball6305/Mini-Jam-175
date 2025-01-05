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
    private Camera mainCamera;           // Reference to the main camera
    private float previousYPosition;     // Track the previous Y position of the player

    void Start()
    {
        playerTransform = transform;  // Get the player's transform
        mainCamera = Camera.main;     // Get the main camera
        previousYPosition = playerTransform.position.y;

        StartCoroutine(SpawnPrefab());
    }

    private IEnumerator SpawnPrefab()
    {
        while (true) // Keep spawning indefinitely
        {
            if (playerTransform.position.y != previousYPosition)  // Check if player's Y position is changing
            {
                if (spawnedObjects.Count < maxSpawnCount)
                {
                    Vector3 spawnPosition = Vector3.zero;
                    bool validPosition = false;

                    while (!validPosition)
                    {
                        Vector3 offset = new Vector3(
                            Random.Range(-spawnRange, spawnRange),
                            Random.Range(0f, spawnHeightRange),
                            Random.Range(-spawnRange, spawnRange)
                        );

                        spawnPosition = playerTransform.position + offset;

                        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(spawnPosition);

                        float distanceToPlayer = Vector3.Distance(spawnPosition, playerTransform.position);
                        if (viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
                            viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
                            viewportPosition.z > 0 &&
                            distanceToPlayer >= spawnExclusionRadius)
                        {
                            validPosition = true;
                        }
                    }

                    GameObject newObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                    spawnedObjects.Add(newObject);

                    Debug.Log("Spawned " + prefabToSpawn.name + " at position: " + spawnPosition);

                    yield return new WaitForSeconds(spawnDelay);
                }

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
            }

            previousYPosition = playerTransform.position.y;  // Update the previous Y position
            yield return null;
        }
    }
}
