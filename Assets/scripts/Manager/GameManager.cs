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

    public string[] currentPages;              // 用于存储每页文本内容的数组
    public string[] currentImagePages;         // 用于存储每页图片的数组
    private int currentPageNumber = 0;         // 当前页数

    private bool endingScene = false;
    private bool EndingFiveBool = false;

    LevelUpManager levelUpManager;
    TextPageManager textPageManager;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerMovements>();
        levelUpManager = FindAnyObjectByType<LevelUpManager>();
        textPageManager = FindAnyObjectByType<TextPageManager>();
        Invoke("GameStart", 0);
    }

    public void GameStart()
    {
        // 存档点3
        if (TextPageManager.StageThreeExplain)
        {
            Debug.Log("从拥有大脑开始");
            levelUpManager.UpdateText(50);
            levelUpManager.evolveBrain();
        }
        // 存档点2
        else if (TextPageManager.StageTwoExplain)
        {
            Debug.Log("从华夏鳗鱼开始");
            levelUpManager.UpdateText(30);
            levelUpManager.TailNotochord();
        }
        // 正常从头开始游戏
        else
        {
            if (!TextPageManager.GameStartExplain)
            {
                textPageManager.ShowExplain();
            }
        }
    }

    void ShowEndScreenView(string imageLocation, string textInformation)
    {
        endSceneImage.sprite = Resources.Load<Sprite>(imageLocation);
        endSceneText.text = textInformation;
    }

    void ShowEndScreen()
    {
        currentPageNumber = 0;
        ShowNextPage();
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
        Time.timeScale = 0;
    }

    // 切换到下一页的方法
    void ShowNextPage()
    {
        if (currentPageNumber < currentPages.Length)
        {
            ShowEndScreenView(currentImagePages[currentPageNumber], currentPages[currentPageNumber]);

            // 如果当前是最后一页同时是结局，那把按钮改成重来
            if ((currentPageNumber == currentPages.Length - 1) && (endingScene))
            {
                if (EndingFiveBool)
                {
                    EndSceneButton.GetComponentInChildren<TextMeshProUGUI>().text = "Thank You";
                    EndSceneButton.onClick.AddListener(BackToMainMenu);
                }
                else
                {
                    EndSceneButton.GetComponentInChildren<TextMeshProUGUI>().text = "try again";
                    EndSceneButton.onClick.AddListener(TryAgain);
                }
            }
            else
            {
                EndSceneButton.GetComponentInChildren<TextMeshProUGUI>().text = "next";
                EndSceneButton.onClick.AddListener(ShowNextPage);
            }

            currentPageNumber++;
        }
        else
        {
            // 如果已经是最后一页，可以在这里执行一些额外的逻辑，比如关闭对话框等
            Debug.Log("已到最后一页");
        }
    }

    void TryAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 回到住菜单
    void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }

    // 结局1 
    public void EndingOne()
    {
        if (endingScene)
        {
            return;
        }
        endingScene = true;
        StartCoroutine(EndingOneHelper());
    }

    private IEnumerator EndingOneHelper()
    {
        player.ChangeSpeed(0.5f);

        yield return new WaitForSeconds(3f);

        player.StopMoving();
        Rigidbody2D rb = player.gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;

        yield return new WaitForSeconds(3);  // Wait for the specified time

        // 加载结局
        currentPages = EndOneText;
        currentImagePages = EndOneImage;
        ShowEndScreen();
    }

    // 结局2
    public void EndingTwo()
    {
        if (endingScene)
        {
            return;
        }
        endingScene = true;
        StartCoroutine(EndingTwoHelper());
    }

    private IEnumerator EndingTwoHelper()
    {
        yield return new WaitForSeconds(3f);

        player.StopMoving();
        Rigidbody2D rb = player.gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;

        yield return new WaitForSeconds(3f);  // Wait for the specified time

        // 加载结局
        currentPages = EndTwoText;
        currentImagePages = EndTwoImage;
        ShowEndScreen();
    }

    // 结局3, 没能抵抗住诱惑结局
    public void EndingThree()
    {
        if (endingScene)
        {
            return;
        }
        endingScene = true;
        StartCoroutine(EndingThreeHelper());
    }

    private IEnumerator EndingThreeHelper()
    {
        player.StopMoving();
        Rigidbody2D rb = player.gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;
        rb.velocity = Vector2.zero; // Set linear velocity to zero

        yield return new WaitForSeconds(3f);  // Wait for the specified time

        // 加载结局
        currentPages = EndThreeText;
        currentImagePages = EndThreeImage;
        ShowEndScreen();
    }

    // 结局4
    public void EndingFour()
    {
        if (endingScene)
        {
            return;
        }
        endingScene = true;
        StartCoroutine(EndingFourHelper());
    }

    private IEnumerator EndingFourHelper()
    {
        player.StopMoving();
        Rigidbody2D rb = player.gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;
        rb.velocity = Vector2.zero; // Set linear velocity to zero

        yield return new WaitForSeconds(3f);  // Wait for the specified time

        // 加载结局
        currentPages = EndFourText;
        currentImagePages = EndFourImage;
        ShowEndScreen();
    }

    // 结局5
    public void EndingFive()
    {
        if (endingScene)
        {
            return;
        }
        StartCoroutine(EndingFiveHelper());
    }

    private IEnumerator EndingFiveHelper()
    {
        // 留给玩家的躲避时间
        yield return new WaitForSeconds(15f);  // Wait for the specified time

        endingScene = true;
        EndingFiveBool = true;

        yield return new WaitForSeconds(3f);  // Wait for the specified time

        // 加载结局
        currentPages = EndFiveText;
        currentImagePages = EndFiveImage;
        ShowEndScreen();
    }

    // 结局1文本
    private string[] EndOneText = new string[] {
        "Due to the excessively large filter-feeding holes in your head, you gradually " +
        "realize that your species is unable to swim in the ocean. Eventually, you settle on " +
            "the seafloor to filter-feed, becoming what would later evolve into echinoderms (such as starfish).",

        "Perhaps avoiding the enlargement of the head is another possible evolutionary path."
    };
    private string[] EndOneImage = new string[] {
        "EndSceneImage/End1_1",
        "EndSceneImage/End1_2",
    };

    // 结局2
    private string[] EndTwoText = new string[] {
        "The notochord was a great help for swimming, but your group of chordates chose to extend the notochord all the way to the head, preventing the development of a brain.",
        "By doing this, you closed off future possibilities and became a new branch called cephalochordates. The lancelet is one of your descendants.",
        "Although a faster swimming speed is tempting, keeping the notochord in the back half of the body might be a better choice.",
    };

    private string[] EndTwoImage = new string[] {
        "EndSceneImage/End2_1",
        "EndSceneImage/End2_2",
        "EndSceneImage/End2_3",
    };

    // 结局3
    private string[] EndThreeText = new string[] {
        "With a wider field of vision, you discovered that most nutrients in the early Cambrian seas were concentrated near the ocean floor.",
        "Your group of chordates gradually chose to reduce their nerve and movement abilities, attaching themselves to the seabed to filter-feed from the water. This is how you became tunicates. ",
        "Perhaps moving away from the ocean floor and continuing to explore the potential of the nervous system is a more promising evolutionary path for the future",
    };

    private string[] EndThreeImage = new string[] {
        "EndSceneImage/End3_1",
        "EndSceneImage/End3_2",
        "EndSceneImage/End3_3",
    };

    // 结局4
    private string[] EndFourText = new string[] {
        "Evolution is often this cruel—choosing the right path doesn’t guarantee survival, as natural disasters and predators are always lurking. As Earth faced its first mass extinction, the Late Ordovician extinction, your group of chordates also became a part of history.",
        "But life finds a way, and evolution continues on.",
    };

    private string[] EndFourImage = new string[] {
        "EndSceneImage/End4_1",
        "EndSceneImage/End4_1",
    };

    // 结局5
    private string[] EndFiveText = new string[] {
        "After surviving the Late Ordovician mass extinction, the once-small worm-like creatures began to reveal their true potential on the evolutionary path.",
        "Finally, your small group of chordates made it to the end of this path of freedom, evolving into vertebrates. In the mid-early Cambrian period, only three fossil types were found in small quantities, showing just how rare it was for deuterostomes to reach the end of this road.",
        "At this time, the nervous systems of most other animals were improved versions of a nerve net, making it difficult for them to adapt to bilaterally symmetrical body structures.",
        "But your group of chordates, after what seemed like a series of blind struggles, essentially built a new nervous system from scratch—one that was highly adapted to a symmetrical body.",
        "Following this, your vertebrate group evolved bones that were both strong and lightweight. You also deeply improved the nervous system, evolving myelin sheaths, marking a new era in the speed of nerve transmission.",
        "Finally, vertebrates evolved jaws, leading to the emergence of jawed fish.",
        "Once upon a time, trilobites reigned supreme, sea scorpions perfected their limb adaptations, and the cephalopods were on the rise. But no one could have imagined that the seemingly harmless little worms had already undergone a complete bodily revolution. While your rivals might have reached the middle of their game, you have only just begun. But remember, we’re not even playing on the same board.",
    };

    private string[] EndFiveImage = new string[] {
        "EndSceneImage/End5_2",
        "EndSceneImage/End5_2",
        "EndSceneImage/End5_1",
        "EndSceneImage/End5_1",
        "EndSceneImage/End5_3",
        "EndSceneImage/End5_3",
        "EndSceneImage/End5_3",
    };
}
