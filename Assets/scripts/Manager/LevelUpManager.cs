using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    public int totalPoints;
    public int pointsAfterBrain;
    public int SeaFloorFoodCounter;
    public TextMeshProUGUI foodText;
    private EvolutionaryStages currentState = EvolutionaryStages.Meatworms;

    public GameObject optionButtonPrefab;

    public Button HeadButton;
    public Button tailButton;

    public TextMeshProUGUI explainText;

    private PlayerMovements player;
    private GameManager gameManager;
    private TextPageManager textPageManager;

    public GameObject Enemy;
    public GameObject StageOneImage;
    public GameObject StageTwoImage;
    public GameObject StageThreeImage;

    private void Start()
    {
        explainText.gameObject.SetActive(false);
        player = FindAnyObjectByType<PlayerMovements>();
        gameManager = FindAnyObjectByType<GameManager>();
        textPageManager = FindAnyObjectByType<TextPageManager>();
    }

    public void EatSeaFloorFood()
    {
        totalPoints += 1;
        foodText.text = "nutrients: " + totalPoints;
        SeaFloorFoodCounter += 1;
        CheckProgress();
    }

    public void eat()
    {
        totalPoints += 1;
        if (currentState == EvolutionaryStages.BrainAndEye)
        {
            Debug.Log("抵御诱惑分" + pointsAfterBrain);
            pointsAfterBrain += 1;
        }
        foodText.text = "nutrients: " + totalPoints;

        CheckProgress();
    }

    public void UpdateText(int points)
    {
        totalPoints = points;
        foodText.text = "nutrients: " + totalPoints;
    }

    private bool hasMouthLevelTwoUpgradeTriggered = false;

    private void CheckProgress()
    {

        // 如果升级口部就注定会是结局1
        if (currentState == EvolutionaryStages.MouthPath)
        {
            if (totalPoints >= 6 && !hasMouthLevelTwoUpgradeTriggered)
            {
                hasMouthLevelTwoUpgradeTriggered = true;
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

        // 升级滤食口
        if (totalPoints == 15)
        {
            NewEvolvetraits();
            AddLevelUpOption("Enlarge the filter-feeding mouth (increase size)", MouthLeveloneUp, true);
        }
        // 演化脊索
        else if (totalPoints == 30)
        {
            HeadButton.gameObject.SetActive(false);

            NewEvolvetraits();
            AddLevelUpOption("Contract the filter-feeding mouth and evolve a dorsal nerve (increase moving speed)", TailNotochord, false);
        }
        // 结局2选项，脊索延伸到头部
        else if (totalPoints == 38)
        {
            NewEvolvetraits();
            AddLevelUpOption("Extend the notochord to the head (Significantly enhance movement speed)", WholeBodyNotochord, true);
        }
        // 演化大脑，挤占结局2选项
        else if (totalPoints == 50 && currentState != EvolutionaryStages.BrainAndEye)
        {
            HeadButton.gameObject.SetActive(false);

            NewEvolvetraits();
            HeadButton.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("OtherImage/Brain");
            AddLevelUpOption("Evolve Brain", evolveBrain, true);
        }
        // 抵抗住了海底的诱惑，前提是进化了大脑才能开始计数
        else if (pointsAfterBrain == 10)
        {
            Debug.Log("触发奥陶纪大灭绝");
            GenerateExplainText("Ordovician extinction", 2f);
            FindAnyObjectByType<SoundManager>().PlayFinalBGM();
            FindAnyObjectByType<NutrientSpawner>().gameObject.SetActive(false);
            // 奥陶纪大灭绝
            Enemy.gameObject.SetActive(true);
            gameManager.EndingFive();
        }

        // 没能抵抗住诱惑
        if (SeaFloorFoodCounter == 10)
        {
            gameManager.EndingThree();
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

    private void GenerateEffect()
    {
        FindAnyObjectByType<SoundManager>().PlaySound(3, 0);
        FindAnyObjectByType<EffectManager>().PlayEffect(1, player.transform.position, 0.7f);
    }

    // 升级滤食口1
    private void MouthLeveloneUp()
    {
        GenerateEffect();
        Debug.Log("Level up month");
        player.ResizeSpriteAndCollider(new Vector2(1.5f, 1.5f));
        currentState = EvolutionaryStages.MouthPath;
    }

    // 升级滤食口2，引导向结局1
    private void MouthLeveltwoUp()
    {
        GenerateEffect();
        Debug.Log("Level up month");
        player.ResizeSpriteAndCollider(new Vector2(2f, 2f));
        GenerateExplainText("As the filter-feeding mouth enlarges, swimming becomes more difficult.", 3f);
        gameManager.EndingOne();
    }

    // 升级脊索1
    public void TailNotochord()
    {
        GenerateEffect();
        player.ChangeSpeed(7);
        player.ChangeSprite(1);
        StageOneImage.SetActive(false);
        StageTwoImage.SetActive(true);
        StageThreeImage.SetActive(false);
        currentState = EvolutionaryStages.NotochordL1;
        if (!TextPageManager.StageTwoExplain)
        {
            textPageManager.ShowExplain();
        }

        Debug.Log("Show explain");
    }

    // 升级脊索至头部,结局2
    private void WholeBodyNotochord()
    {
        GenerateEffect();
        player.ChangeSpeed(9);
        gameManager.EndingTwo();
    }

    // 演化大脑
    public void evolveBrain()
    {
        GenerateEffect();
        pointsAfterBrain = 0;
        currentState = EvolutionaryStages.BrainAndEye;
        player.ChangeSpeed(7);
        player.ChangeRotation(150f);
        player.ChangeSprite(2);
        StageOneImage.SetActive(false);
        StageTwoImage.SetActive(false);
        StageThreeImage.SetActive(true);
        FindAnyObjectByType<CameraFollow>().SetCameraSize(7);
        FindAnyObjectByType<NutrientSpawner>().StartSpwan();

        if (!TextPageManager.StageThreeExplain)
        {
            textPageManager.ShowExplain();
        }
    }

    // 触发奥陶纪末大灭绝
    private void triggerExtinction()
    {
        currentState = EvolutionaryStages.Final;
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