using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpwanManager : MonoBehaviour
{
    public float spawnInterval = 2f;    // Time between spawns
    public float nutrientSpeed = 2f;    // Speed of nutrient movement
    public float seaSurface = 5f;       // Sea surface boundary (y-axis)
    public float seaFloor = -5f;        // Sea floor boundary (y-axis)
    public float minSpawnDistance = 3f; // Minimum distance between nutrients
    public int maxNutrientsOnMap = 20;  // Max number of nutrients on the map

    private List<GameObject> activeNutrients = new List<GameObject>(); // Track active nutrients
    private Camera cam;

    public GameObject prefab; // The prefab that this pool manages
    public int initialSize = 50; // Initial pool size

    private List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        // Pre-fill the pool with inactive GameObjects
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
        cam = Camera.main;
        StartCoroutine(SpawnNutrients());
    }

    // Get an object from the pool
    public GameObject GetFromPool()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true); // Activate the object and return it
                return obj;
            }
        }

        // If no inactive objects are available, instantiate a new one and add it to the pool
        GameObject newObj = Instantiate(prefab);
        pool.Add(newObj);
        return newObj;
    }

    // Return an object to the pool
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        activeNutrients.Remove(obj);
    }

    IEnumerator SpawnNutrients()
    {
        yield return new WaitForSeconds(spawnInterval);

        // Check if we have reached the max nutrients on the map
        if (activeNutrients.Count < maxNutrientsOnMap)
        {
            SpawnNutrient();
        }
    }

    void SpawnNutrient()
    {
        // Get a nutrient from the pool
        GameObject nutrient = GetFromPool();

        if (nutrient != null)
        {
            // Determine a spawn position outside the screen
            Vector2 spawnPosition = GetSpawnPosition();

            // Ensure the new nutrient is not too close to existing ones
            if (IsPositionValid(spawnPosition))
            {
                nutrient.transform.position = spawnPosition;
                nutrient.SetActive(true);

                activeNutrients.Add(nutrient);

                // Set the nutrient's movement towards the screen center
                Rigidbody2D rb = nutrient.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 direction = (Vector2.zero - spawnPosition).normalized;
                    rb.velocity = direction * nutrientSpeed;
                }
            }
            else
            {
                // If position is not valid, try again
                SpawnNutrient();
            }
        }
    }

    Vector2 GetSpawnPosition()
    {
        // Get screen bounds
        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        // Spawn at a position outside the screen on the x-axis
        float spawnX = 0f;

        if (Random.value > 0.5f)
        {
            // Spawn to the right of the screen
            spawnX = (screenWidth / 2) + 1f;
        }
        else
        {
            // Spawn to the left of the screen
            spawnX = -(screenWidth / 2) - 1f;
        }

        // Spawn within y boundaries
        float spawnY = Random.Range(seaFloor, seaSurface);

        return new Vector2(spawnX, spawnY);
    }

    bool IsPositionValid(Vector2 position)
    {
        foreach (GameObject food in activeNutrients)
        {
            if (food != null && food.activeInHierarchy)
            {
                float distance = Vector2.Distance(position, food.transform.position);
                if (distance < minSpawnDistance)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
