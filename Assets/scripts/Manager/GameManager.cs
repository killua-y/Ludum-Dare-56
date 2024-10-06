using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    PlayerMovements player;

    public GameObject EndScenePanel;
    public Image endSceneImage;
    public TextMeshProUGUI endSceneText;
    public Button EndSceneButton;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerMovements>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowEndScreenView(string imageLocation, string textInformation)
    {
        endSceneImage.sprite = Resources.Load<Sprite>(imageLocation);
        endSceneText.text = textInformation;
    }

    void ShowEndScreen()
    {
        StartCoroutine(SmoothMoveCoroutine(1080, 0));
    }

    private IEnumerator SmoothMoveCoroutine(float startY, float endY)
    {
        float elapsedTime = 0;
        float duration = 0.2f;
        Vector3 startPosition = EndScenePanel.transform.localPosition;
        Vector3 targetPosition = new Vector3(startPosition.x, endY, startPosition.z);

        while (elapsedTime < duration)
        {
            // Calculate the current position using Lerp
            float newY = Mathf.Lerp(startY, endY, elapsedTime / duration);
            EndScenePanel.transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final position is set
        EndScenePanel.transform.localPosition = targetPosition;
    }

    public void SetUpleaveButton()
    {
        TextMeshProUGUI buttonText = EndSceneButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Try Again";
        EndSceneButton.onClick.AddListener(BackToMainMenu);
    }

    void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }

    // 结局1 
    public void EndingOne()
    {
        StartCoroutine(EndingOneHelper(5f));
    }

    public void EndingOneHelper()
    {
        ShowEndScreenView("EndSceneImage/end1", "Perhaps avoiding the enlargement " +
            "of the head is another possible evolutionary path.");
        SetUpleaveButton();
    }

    private IEnumerator EndingOneHelper(float time)
    {
        player.ChangeSpeed(0.5f);

        yield return new WaitForSeconds(2f);

        player.StopMoving();
        Rigidbody2D rb = player.gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;

        yield return new WaitForSeconds(time);  // Wait for the specified time

        ShowEndScreen();
        ShowEndScreenView("EndSceneImage/end1", "Due to the excessively large " +
            "filter-feeding holes in your head, you gradually realize that your " +
            "species is unable to swim in the ocean. Eventually, you settle on " +
            "the seafloor to filter-feed, becoming what would later evolve into " +
            "echinoderms (such as starfish).");
        EndSceneButton.GetComponentInChildren<TextMeshProUGUI>().text = "next";
        EndSceneButton.onClick.AddListener(EndingOneHelper);
    }

    // 结局2

    // 结局3

    // 结局4

    // 结局5（真）
}
