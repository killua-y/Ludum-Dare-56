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
            Debug.Log("从拥有开始");
            levelUpManager.UpdateText(40);
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
                EndSceneButton.GetComponentInChildren<TextMeshProUGUI>().text = "try again";
                EndSceneButton.onClick.AddListener(TryAgain);
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
        "EndSceneImage/end1",
        "EndSceneImage/end1",
    };

    // 结局2
    private string[] EndTwoText = new string[] {
        "This is ending two",
        "This is ending two",
    };

    private string[] EndTwoImage = new string[] {
        "EndSceneImage/end1",
        "EndSceneImage/end1",
    };

    // 结局3
    private string[] EndThreeText = new string[] {
        "This is ending three",
        "This is ending three",
    };

    private string[] EndThreeImage = new string[] {
        "EndSceneImage/end1",
        "EndSceneImage/end1",
    };

    // 结局4
    private string[] EndFourText = new string[] {
        "This is ending four",
        "This is ending two",
    };

    private string[] EndFourImage = new string[] {
        "EndSceneImage/end1",
        "EndSceneImage/end1",
    };

    // 结局5
    private string[] EndFiveText = new string[] {
        "This is ending five",
        "This is ending two",
    };

    private string[] EndFiveImage = new string[] {
        "EndSceneImage/end1",
        "EndSceneImage/end1",
    };
}
