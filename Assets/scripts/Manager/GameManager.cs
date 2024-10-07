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

        }
        // 存档点2
        else if (TextPageManager.StageTwoExplain)
        {
            Debug.Log("从华夏鳗鱼开始");
            levelUpManager.UpdateText(30);
            TextPageManager.StageTwoExplain = true;
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

    // 结局3
    public void EndingThree()
    {
        if (endingScene)
        {
            return;
        }
        endingScene = true;
        StartCoroutine(EndingTwoHelper());
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

    // 结局1文本
    public string[] EndOneText = new string[] {
        "Due to the excessively large filter-feeding holes in your head, you gradually " +
        "realize that your species is unable to swim in the ocean. Eventually, you settle on " +
            "the seafloor to filter-feed, becoming what would later evolve into echinoderms (such as starfish).",

        "Perhaps avoiding the enlargement of the head is another possible evolutionary path."
    };
    public string[] EndOneImage = new string[] {
        "EndSceneImage/end1",
        "EndSceneImage/end1",
    };

    // 结局2
    public string[] EndTwoText = new string[] {
        "第一",
        "第二",
    };

    public string[] EndTwoImage = new string[] {
        "第一",
        "第二",
    };

    // 结局3
    public string[] EndThreeText = new string[] {
        "第一",
        "第二",
    };

    public string[] EndThreeImage = new string[] {
        "第一",
        "第二",
    };

    // 结局4
    public string[] EndFourText = new string[] {
        "第一",
        "第二",
    };

    public string[] EndFourImage = new string[] {
        "第一",
        "第二",
    };

    // 结局5
    public string[] EndFiveText = new string[] {
        "第一",
        "第二",
    };

    public string[] EndFiveImage = new string[] {
        "第一",
        "第二",
    };
}
