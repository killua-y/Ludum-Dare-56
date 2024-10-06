using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    private int totalPoints;
    private int currentPoints;
    public TextMeshProUGUI foodText;
    private EvolutionaryStages currentState = EvolutionaryStages.Meatworms;

    public GameObject optionButtonPrefab;
    public Transform headPosition;
    public Transform tailPosition;

    private Button HeadButton;
    private Button tailButton;
    [SerializeField] private Button lastButton;

    public TextMeshProUGUI explainText;

    private PlayerMovements player;
    private GameManager gameManager;

    private void Start()
    {
        explainText.gameObject.SetActive(false);
        player = FindAnyObjectByType<PlayerMovements>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public void eat()
    {
        totalPoints += 1;
        currentPoints += 1;
        foodText.text = "point: " + currentPoints;

        CheckProgress();
    }

    private void CheckProgress()
    {
        if (totalPoints == 15)
        {
            NewEvolvetraits();
            AddLevelUpOption("Upgrade month", MouthLeveloneUp, headPosition);
        }

        // 如果升级口部就注定会是结局1
        if (currentState == EvolutionaryStages.MouthL1)
        {
            if (totalPoints == 30)
            {
                NewEvolvetraits();
                AddLevelUpOption("Upgrade a larger month", MouthLeveltwoUp, headPosition);
            }
            else if (totalPoints == 100)
            {
                // 强制进入结局1
                gameManager.EndingOne();
            }
            return;
        }

        if (totalPoints == 30)
        {
            if (lastButton != null)
            {
                Debug.Log("DEstory button");
                Destroy(lastButton.gameObject);
            }
            NewEvolvetraits();
            AddLevelUpOption("Upgrade Tail Notochord", TailNotochord, tailPosition);
        }

    }

    private void NewEvolvetraits()
    {
        GenerateExplainText("Find new Evolve traits", 1f);
    }

    // 添加解释文本
    public void GenerateExplainText(string text, float time)
    {
        explainText.text = text;  // Set the text
        explainText.gameObject.SetActive(true);  // Activate the text
        StartCoroutine(HideTextAfterTime(time));  // Start coroutine to hide the text after a certain time
    }

    private IEnumerator HideTextAfterTime(float time)
    {
        yield return new WaitForSeconds(time);  // Wait for the specified time
        explainText.gameObject.SetActive(false);  // Deactivate the text
    }

    private void AddLevelUpOption(string text, Action action, Transform buttonPosition)
    {
        GameObject buttonObject = Instantiate(optionButtonPrefab, this.transform);
        buttonObject.transform.position = buttonPosition.position;
        Button optionButton = buttonObject.GetComponent<Button>();
        TextMeshProUGUI buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = text;

        optionButton.onClick.AddListener(() => action.Invoke());

        // 如果是头部按钮添加到当前按钮
        lastButton = optionButton;
}

    // 升级滤食口1
    private void MouthLeveloneUp()
    {
        Debug.Log("Level up month");
        player.ResizeSpriteAndCollider(new Vector2(1.5f, 1.5f));
        currentState = EvolutionaryStages.MouthL1;
    }

    // 升级滤食口2，引导向结局1
    private void MouthLeveltwoUp()
    {
        Debug.Log("Level up month");
        player.ResizeSpriteAndCollider(new Vector2(2f, 2f));
        currentState = EvolutionaryStages.MouthL2End;
        gameManager.EndingOne();
    }

    // 升级脊索1
    private void TailNotochord()
    {
        Debug.Log("Level up Tail Notochord");
        player.ChangeSpeed(7);
    }

    // 升级脊索2
    private void WholeBodyNotochord()
    {
        Debug.Log("Level up month");
        player.ChangeSpeed(9);
    }
}


public enum EvolutionaryStages
{
    Meatworms,
    MouthL1,
    MouthL2End,
    NotochordL1,
    NotochordL2End,
    BrainAndEye,
    Extinction
}