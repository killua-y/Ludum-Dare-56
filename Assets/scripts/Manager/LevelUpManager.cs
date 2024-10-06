using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    private int totalPoints;
    public TextMeshProUGUI foodText;
    private EvolutionaryStages currentState = EvolutionaryStages.Meatworms;

    public GameObject optionButtonPrefab;

    public Button HeadButton;
    public Button tailButton;

    public TextMeshProUGUI explainText;

    private PlayerMovements player;
    private GameManager gameManager;
    private TextPageManager textPageManager;


    private void Start()
    {
        explainText.gameObject.SetActive(false);
        player = FindAnyObjectByType<PlayerMovements>();
        gameManager = FindAnyObjectByType<GameManager>();
        textPageManager = FindAnyObjectByType<TextPageManager>();
    }

    public void eat()
    {
        totalPoints += 1;
        foodText.text = "nutrients: " + totalPoints;

        CheckProgress();
    }

    private void CheckProgress()
    {

        if (totalPoints == 3)
        {
            NewEvolvetraits();
            AddLevelUpOption("Enlarge the filter-feeding mouth (increase size)", MouthLeveloneUp, true);
        }

        // 如果升级口部就注定会是结局1
        if (currentState == EvolutionaryStages.MouthPath)
        {
            if (totalPoints == 6)
            {
                NewEvolvetraits();
                AddLevelUpOption("Enlarge the filter-feeding mouth again (increase more size)", MouthLeveltwoUp, true);
            }
            else if (totalPoints == 100)
            {
                // 强制进入结局1
                gameManager.EndingOne();
            }
            return;
        }

        if (totalPoints == 6)
        {
            HeadButton.gameObject.SetActive(false);

            NewEvolvetraits();
            AddLevelUpOption("Contract the filter-feeding mouth and evolve a dorsal nerve (increase moving speed)", TailNotochord, false);
        }

        if (totalPoints == 9)
        {
            NewEvolvetraits();
            AddLevelUpOption("Extend the notochord to the head (Significantly enhance movement speed)", WholeBodyNotochord, true);
        }

        if (totalPoints == 15)
        {
            HeadButton.gameObject.SetActive(false);

            NewEvolvetraits();
            AddLevelUpOption("Evlolve Brain", evolveBrain, true);
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

    private void AddLevelUpOption(string text, Action action, bool isHead)
    {
        Button optionButton;
        if (isHead)
        {
            optionButton = HeadButton;
        }
        else
        {
            optionButton = tailButton;
        }

        optionButton.gameObject.SetActive(true);

        TextMeshProUGUI buttonText = optionButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = text;

        optionButton.onClick.RemoveAllListeners();
        optionButton.onClick.AddListener(() => action.Invoke());
        optionButton.onClick.AddListener(() => Disactive(optionButton));
    }

    private void Disactive(Button button)
    {
        button.gameObject.SetActive(false);
    }

    // 升级滤食口1
    private void MouthLeveloneUp()
    {
        Debug.Log("Level up month");
        player.ResizeSpriteAndCollider(new Vector2(1.5f, 1.5f));
        currentState = EvolutionaryStages.MouthPath;
    }

    // 升级滤食口2，引导向结局1
    private void MouthLeveltwoUp()
    {
        Debug.Log("Level up month");
        player.ResizeSpriteAndCollider(new Vector2(2f, 2f));
        GenerateExplainText("As the filter-feeding mouth enlarges, swimming becomes more difficult.", 3f);
        gameManager.EndingOne();
    }

    // 升级脊索1
    public void TailNotochord()
    {
        Debug.Log("Level up Tail Notochord");
        player.ChangeSpeed(7);
        player.ChangeSprite(1);
        textPageManager.ShowExplain();
    }

    // 升级脊索2
    private void WholeBodyNotochord()
    {
        player.ChangeSpeed(9);
        gameManager.EndingTwo();
    }

    // 演化大脑
    private void evolveBrain()
    {
        player.ChangeSpeed(9);
        FindAnyObjectByType<NutrientSpawner>().SeaFloorFood = true;

    }
}

public enum EvolutionaryStages
{
    Meatworms,
    MouthPath,
    NotochordL1,
    BrainAndEye,
    Final
}