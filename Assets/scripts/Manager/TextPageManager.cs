using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextPageManager : MonoBehaviour
{
    private string[] currentPages;              // 用于存储每页文本内容的数组
    private string[] currentImagePages;         // 用于存储每页图片的数组
    private int currentPageNumber = 0;         // 当前页数
    public TextMeshProUGUI endSceneText;
    public Image endSceneImage;
    public Button EndSceneButton;

    public static bool GameStartExplain = false;
    public static bool StageTwoExplain = false;
    public static bool StageThreeExplain = false;
    public GameObject NarrationPanel;

    void Start()
    {
    }

    public void ShowExplain()
    {
        if (!GameStartExplain)
        {
            GameStartExplain = true;
            currentPages = NarrationText1;
            currentImagePages = NarrationImage1;
            StartExplain();
        }
        else if (!StageTwoExplain)
        {
            StageTwoExplain = true;
            currentPages = NarrationText2;
            currentImagePages = NarrationImage2;
            StartExplain();
        }
        else if (!StageThreeExplain)
        {
            StageThreeExplain = true;
            currentPages = NarrationText3;
            currentImagePages = NarrationImage3;
            StartExplain();
        }
        else
        {
            Debug.Log("Do nothing");
        }
    }

    void StartExplain()
    {
        Time.timeScale = 0;
        currentPageNumber = 0;
        NarrationPanel.SetActive(true);
        ShowNextPage();
    }

    void Updatetext(string imageLocation, string textInformation)
    {
        endSceneImage.sprite = Resources.Load<Sprite>(imageLocation);
        endSceneText.text = textInformation;
    }

    // 切换到下一页的方法
    public void ShowNextPage()
    {
        FindAnyObjectByType<SoundManager>().PlaySound(2, 0);
        if (currentPageNumber < currentPages.Length)
        {
            Updatetext(currentImagePages[currentPageNumber], currentPages[currentPageNumber]);

            currentPageNumber++;
        }
        else
        {
            // 如果已经是最后一页，可以在这里执行一些额外的逻辑，比如关闭对话框等
            Time.timeScale = 1;
            NarrationPanel.SetActive(false);
        }
        EndSceneButton.interactable = false;
        EndSceneButton.interactable = true;
    }

    // 科普1
    private string[] NarrationText1 = new string[] {
        "You are a deuterostome trying to explore possible evolutionary paths, your body structure is roughly divided into a filter-feeding mouth at the front and a 'tail' that aids in swimming at the back.",
        "Such a simple structure can only filter solid nutrients. So, for now, just focus on eating enough.",
    };
    private string[] NarrationImage1 = new string[] {
        "NarrationImage/houkou",
        "NarrationImage/houkou",
    };

    // 科普2
    private string[] NarrationText2 = new string[] {
        "On the path of evolution, your species doesn’t want to give up swimming. So, it decides to shrink its filter-feeding mouth and strengthen its nerves and muscles. ",
        "The big nerve on its back becomes a control center, and eventually, a tough rod-like structure forms inside its body. This rod works like a spring, storing energy from body movements to make swimming easier.",
        "This is how the notochord came to be, and chordates appeared. But for now, your main goal is still to eat enough.",
    };
    private string[] NarrationImage2 = new string[] {
        "NarrationImage/Narration2_1",
        "NarrationImage/Narration2_2",
        "NarrationImage/Narration2_3",
    };

    // 科普3
    private string[] NarrationText3 = new string[] {
        "Congratulations! You've discovered how vertebrates got their most powerful tool: the brain.",
        "This species kept its notochord in the back half of its body and stretched its back forward to grow a simple brain.",
        "With this new brain, you can see better and farther. Now, go out and find more food!",
    };
    private string[] NarrationImage3 = new string[] {
        "NarrationImage/Narration3_1",
        "NarrationImage/Narration3_2",
        "NarrationImage/Narration3_3",
    };
}
