using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour
{
    public GameObject fishPrefab;         // The prefab for the fish
    public GameObject goalPrefab;         // Optional visual goal (e.g., a 2D sprite) to see the goal position
    public float tankSize = 5f;    // Size of the tank in which the fish will swim (2D)
    public static Vector2 goalPos = Vector2.zero; // Global position the fish are trying to reach (2D)

    private float distance = 50f;

    private int numFish = 5;              // Number of fish to spawn
    public static GameObject[] allFish;   // Array to hold references to all the fish

    PlayerMovements playerMovements;

    void Start()
    {
        StartFishFlock();
        Destroy(this.gameObject, 8f);
    }

    public void StartFishFlock()
    {
        allFish = new GameObject[numFish]; // Initialize the array with the size of numFish

        // Create all fish and place them randomly within the tank
        for (int i = 0; i < numFish; i++)
        {
            Vector2 pos = new Vector2(
                Random.Range(-tankSize, tankSize),
                Random.Range(-5, 5)
            ) + (Vector2)this.transform.position;
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().globalFlock = this;

            Destroy(allFish[i], 8f);
        }

        playerMovements = FindAnyObjectByType<PlayerMovements>();

        // Set the initial goal position
        goalPos = this.transform.position;
    }

    void EndFishEvent()
    {
        foreach (GameObject fish in allFish)
        {
            Destroy(fish);
        }
    }

    void Update()
    {
        goalPos = playerMovements.transform.position;

        if (Vector2.Distance(playerMovements.transform.position, this.transform.position) >= distance)
        {
            EndFishEvent();
        }
        //// Occasionally (random chance) update the goal position within the bounds of the tank
        //if (Random.Range(0, 10000) < 50)
        //{
        //    goalPos = new Vector2(
        //        Random.Range(-tankSize, tankSize),
        //        Random.Range(-tankSize, tankSize)
        //    );
        //}

        //// Optional: Move the goal prefab (e.g., a sprite) to visualize the goal position
        //if (goalPrefab != null)
        //{
        //    goalPrefab.transform.position = goalPos;
        //}
    }
}
