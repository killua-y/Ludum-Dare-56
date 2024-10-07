using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject ReditPage;
    public GameObject CreditButton;
    public Transform position1;
    public Transform position2;
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
            CreditButton.transform.position = position1.transform.position;
            ReditPage.SetActive(false);
            CreditButton.GetComponent<Button>().interactable = false;
            CreditButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            CreditButton.transform.position = position2.transform.position;
            ReditPage.SetActive(true);
            CreditButton.GetComponent<Button>().interactable = false;
            CreditButton.GetComponent<Button>().interactable = true;
        }
    }
}
