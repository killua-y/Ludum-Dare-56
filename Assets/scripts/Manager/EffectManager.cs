using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // List to store effects (prefabs)
    public List<GameObject> effectPrefabs = new List<GameObject>();

    // Method to play an effect at a specific position and destroy it after a duration
    public void PlayEffect(int index, Vector3 position, float duration)
    {
        // Check if the index is within the range of the list
        if (index >= 0 && index < effectPrefabs.Count)
        {
            // Get the effect prefab from the list
            GameObject effectPrefab = effectPrefabs[index];

            // Instantiate the effect at the specified position
            GameObject effectInstance = Instantiate(effectPrefab, position, Quaternion.identity);

            // Destroy the effect after the specified duration
            Destroy(effectInstance, duration);
        }
        else
        {
            Debug.LogWarning($"Effect at index {index} not found. Make sure the index is within the range of the list.");
        }
    }
}
