using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientSpawner : MonoBehaviour
{
    public GameObject nutrientPrefab; // Assign the nutrient prefab in the inspector
    public Transform player; // Assign the player's transform in the inspector
    private List<GameObject> nutrients = new List<GameObject>(); // Track existing nutrients
    private int maxSpwanObject = 50;
//    private int count = 0;
    public bool SeaFloorFood = false;

    void Start()
    {
        StartCoroutine(SpawnSeaFloorNutrients());
    }

    IEnumerator SpawnSeaFloorNutrients()
    {
        while (SeaFloorFood)
        {
            if (nutrients.Count < maxSpwanObject)
            {
                // Randomize the x position based on the player's position
                float xPosition = player.position.x + Random.Range(-30f, 30f);
                // Randomize the y position between -4.5 and -5.5
                float yPosition = Random.Range(-5.5f, -4.5f);

                // Define the spawn position
                Vector2 spawnPosition = new Vector2(xPosition, yPosition);

                // Instantiate the nutrient prefab at the spawn position with no rotation
                GameObject newNutrients = Instantiate(nutrientPrefab, spawnPosition, Quaternion.identity);
                nutrients.Add(newNutrients);

            }

            // Wait for 0.1 seconds before spawning the next nutrient
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void extinction()
    {

    }
}
