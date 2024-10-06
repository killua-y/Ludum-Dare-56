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
    public Transform buttonParent;

    public Transform headPosition;
    public Transform tailPosition;


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
            AddLevelUpOption("Upgrade month", MouthLevelUp, headPosition);
        }
    }

    private void AddLevelUpOption(string text, Action action, Transform buttonPosition)
    {
        GameObject buttonObject = Instantiate(optionButtonPrefab, buttonParent);
        buttonObject.transform.position = buttonPosition.position;
        Button optionButton = buttonObject.GetComponent<Button>();
        TextMeshProUGUI buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = text;

        optionButton.onClick.AddListener(() => action.Invoke());
    }

    private void MouthLevelUp()
    {
        Debug.Log("Level up month");
    }

    private void EvolveBiggerMonth()
    {

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