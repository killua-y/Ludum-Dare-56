using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject nutrientPrefab; // The nutrient prefab
    public GameObject seaFloorNutrientPrefab; // The nutrient prefab
    private float spawnInterval = 2f;  // Time between spawns
    private float seaSurface = 4f;     // Sea surface boundary (y-axis)
    private float seaFloor = -2f;      // Sea floor boundary (y-axis)
    private float minSpawnDistance = 2f; // Minimum distance between nutrients
    private int maxSpwanObject = 30;

    private List<GameObject> existingNutrients = new List<GameObject>(); // Track existing nutrients
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        StartCoroutine(SpawnNutrients());
    }

    // Coroutine to spawn nutrients at a regular interval
    IEnumerator SpawnNutrients()
    {
        while (true)
        {
            if (existingNutrients.Count < maxSpwanObject)
            {
                SpawnNutrient();
            }

            if (existingNutrients.Count <= 5)
            {
                yield return new WaitForSeconds(spawnInterval / 4);
            }
            else if (existingNutrients.Count <= 10)
            {
                yield return new WaitForSeconds(spawnInterval / 2);
            }
            else
            {
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    public void ReturnToPool(GameObject food)
    {
        existingNutrients.Remove(food);
        Destroy(food);
    }

    // Spawns a nutrient randomly outside the screen but within sea surface and sea floor bounds
    void SpawnNutrient()
    {
        Vector3 spawnPosition = GetValidSpawnPosition();

        // Instantiate the nutrient and add it to the list of existing nutrients
        GameObject nutrient = Instantiate(nutrientPrefab, spawnPosition, Quaternion.identity);
        existingNutrients.Add(nutrient);
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
        // Get camera boundaries (in world units)
        Vector3 screenBottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.transform.position.z));
        Vector3 screenTopRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));

        // Ensure y-axis is between the sea surface and sea floor limits
        float minY = Mathf.Max(screenBottomLeft.y, seaFloor);
        float maxY = Mathf.Min(screenTopRight.y, seaSurface);

        // Randomly choose to spawn left, right, top, or bottom of the screen
        int side = Random.Range(0, 4); // 0 = left, 1 = right, 2 = top, 3 = bottom
        Vector3 spawnPosition = Vector3.zero;

        switch (side)
        {
            case 0: // Left of the screen
                spawnPosition = new Vector3(screenBottomLeft.x - 1, Random.Range(minY, maxY), 0);
                break;
            case 1: // Right of the screen
                spawnPosition = new Vector3(screenTopRight.x + 1, Random.Range(minY, maxY), 0);
                break;
            case 2: // Top of the screen (but respecting sea surface)
                spawnPosition = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), maxY + 1, 0);
                break;
            case 3: // Bottom of the screen (but respecting sea floor)
                spawnPosition = new Vector3(Random.Range(screenBottomLeft.x, screenTopRight.x), minY - 1, 0);
                break;
        }

        return spawnPosition;
    }
}