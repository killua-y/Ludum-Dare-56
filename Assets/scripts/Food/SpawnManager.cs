using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject nutrientPrefab; // The nutrient prefab
    private float spawnInterval = 2f;  // Time between spawns
    private float nutrientSpeed = 2f;  // Speed of nutrient movement
    private float seaSurface = 4f;     // Sea surface boundary (y-axis)
    private float seaFloor = -2f;      // Sea floor boundary (y-axis)
    private float minSpawnDistance = 1f; // Minimum distance between nutrients

    private List<GameObject> existingNutrients = new List<GameObject>(); // Track existing nutrients
    private Queue<GameObject> nutrientPool = new Queue<GameObject>();    // Object pool for nutrients
    private Dictionary<GameObject, Coroutine> nutrientCoroutines = new Dictionary<GameObject, Coroutine>(); // Track coroutines

    private Camera cam;
    private float screenLeft, screenRight, screenTop, screenBottom;

    void Start()
    {
        cam = Camera.main;

        // Get camera boundaries (in world units)
        Vector3 screenBottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 screenTopRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.nearClipPlane));

        screenLeft = screenBottomLeft.x;
        screenRight = screenTopRight.x;
        screenBottom = screenBottomLeft.y;
        screenTop = screenTopRight.y;

        StartCoroutine(SpawnNutrients());
    }

    // Coroutine to spawn nutrients at a regular interval
    IEnumerator SpawnNutrients()
    {
        while (true)
        {
            SpawnNutrient();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Returns the nutrient to the pool instead of destroying it
    public void ReturnToPool(GameObject food)
    {
        // Stop the movement coroutine
        if (nutrientCoroutines.ContainsKey(food))
        {
            StopCoroutine(nutrientCoroutines[food]);
            nutrientCoroutines.Remove(food);
        }

        existingNutrients.Remove(food);
        food.SetActive(false);
        nutrientPool.Enqueue(food);
    }

    // Spawns a nutrient by reusing from the pool or instantiating if none are available
    void SpawnNutrient()
    {
        Vector3 spawnPosition = GetValidSpawnPosition();

        GameObject nutrient;
        if (nutrientPool.Count > 0)
        {
            nutrient = nutrientPool.Dequeue();
            nutrient.transform.position = spawnPosition;
            nutrient.SetActive(true);
        }
        else
        {
            nutrient = Instantiate(nutrientPrefab, spawnPosition, Quaternion.identity);
        }

        existingNutrients.Add(nutrient);

        // Start moving the nutrient randomly and track the coroutine
        Coroutine movementCoroutine = StartCoroutine(MoveNutrientRandomly(nutrient));
        nutrientCoroutines[nutrient] = movementCoroutine;
    }

    // Gets a valid spawn position that's not too close to other nutrients
    Vector3 GetValidSpawnPosition()
    {
        Vector3 spawnPosition;
        bool validPosition = false;

        // Keep trying until we find a valid spawn position
        do
        {
            spawnPosition = GetRandomOffScreenPositionWithinSea();
            validPosition = IsFarEnoughFromOtherNutrients(spawnPosition);
        }
        while (!validPosition);

        return spawnPosition;
    }

    // Check if the spawn position is far enough from all existing nutrients
    bool IsFarEnoughFromOtherNutrients(Vector3 newPosition)
    {
        foreach (GameObject nutrient in existingNutrients)
        {
            if (Vector3.Distance(newPosition, nutrient.transform.position) < minSpawnDistance)
            {
                return false; // Not far enough, invalid position
            }
        }
        return true; // Valid position
    }

    // Gets a random position outside the screen but within the sea bounds (+5 and -5 on the Y-axis)
    Vector3 GetRandomOffScreenPositionWithinSea()
    {
        // Ensure y-axis is between the sea surface and sea floor limits
        float minY = Mathf.Max(screenBottom - 5, seaFloor);
        float maxY = Mathf.Min(screenTop + 5, seaSurface);

        // Randomly choose to spawn left, right, top, or bottom of the screen
        int side = Random.Range(0, 4); // 0 = left, 1 = right, 2 = top, 3 = bottom
        Vector3 spawnPosition = Vector3.zero;

        switch (side)
        {
            case 0: // Left of the screen
                spawnPosition = new Vector3(screenLeft - 1, Random.Range(minY, maxY), 0);
                break;
            case 1: // Right of the screen
                spawnPosition = new Vector3(screenRight + 1, Random.Range(minY, maxY), 0);
                break;
            case 2: // Top of the screen (but respecting sea surface)
                spawnPosition = new Vector3(Random.Range(screenLeft, screenRight), Mathf.Min(seaSurface + 1, screenTop + 1), 0);
                break;
            case 3: // Bottom of the screen (but respecting sea floor)
                spawnPosition = new Vector3(Random.Range(screenLeft, screenRight), Mathf.Max(seaFloor - 1, screenBottom - 1), 0);
                break;
        }

        return spawnPosition;
    }

    // Coroutine to move the nutrient in random directions
    IEnumerator MoveNutrientRandomly(GameObject nutrient)
    {
        while (true)
        {
            // Choose a random direction for the nutrient to move
            Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Vector3 movement = randomDirection * nutrientSpeed * Time.deltaTime;

            // Apply movement to the nutrient
            nutrient.transform.position += movement;

            // Check if nutrient is out of bounds
            if (IsOutOfBounds(nutrient))
            {
                ReturnToPool(nutrient);
                yield break;
            }

            yield return null;
        }
    }

    // Checks if the nutrient is outside the allowed bounds
    bool IsOutOfBounds(GameObject nutrient)
    {
        Vector3 pos = nutrient.transform.position;
        if (pos.x < screenLeft - 1 || pos.x > screenRight + 1 || pos.y < seaFloor || pos.y > seaSurface)
        {
            return true;
        }
        return false;
    }
}
