using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject ReditPage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void ShowCredit()
    {
        if (ReditPage.activeSelf)
        {
            ReditPage.SetActive(false);
        }
        else
        {
            ReditPage.SetActive(true);
        }
    }
}
