using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // List to store audio clips
    public List<AudioClip> audioClipsList = new List<AudioClip>();

    // AudioSources for required sounds and background music
    public AudioSource audioSource;

    public GameObject BGM;
    public GameObject FinalBGM;
    public GameObject FailBGM;
    public GameObject TrueEndingBGM;

    private void Start()
    {
        // Initialize the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Ensure the AudioSource is assigned correctly
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on this GameObject. Please add one.");
        }
    }

    // Public method to play a sound for a specific duration using an index from the list
    public void PlaySound(int index, float duration)
    {
        // Check if the index is within the range of the list
        if (index >= 0 && index < audioClipsList.Count)
        {
            AudioClip clip = audioClipsList[index];

            // Play the audio clip
            audioSource.clip = clip;
            audioSource.Play();

            // If duration is 0 or less, play the entire clip length
            if (duration <= 0)
            {
                duration = clip.length;
            }
        }
        else
        {
            Debug.LogWarning($"Audio clip at index {index} not found. Make sure the index is within the range of the list.");
        }
    }

    public void PlayFinalBGM()
    {
        BGM.SetActive(false);
        FinalBGM.SetActive(true);
        FailBGM.SetActive(false);
        TrueEndingBGM.SetActive(false);
    }

    public void PlayFailBGM()
    {
        BGM.SetActive(false);
        FinalBGM.SetActive(false);
        FailBGM.SetActive(true);
        TrueEndingBGM.SetActive(false);
    }

    public void PlayTrueEndingBGM()
    {
        BGM.SetActive(false);
        FinalBGM.SetActive(false);
        FailBGM.SetActive(false);
        TrueEndingBGM.SetActive(true);
    }
}
