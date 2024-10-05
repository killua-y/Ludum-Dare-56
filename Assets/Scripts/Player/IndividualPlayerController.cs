using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IndividualPlayerController : MonoBehaviour, IPointerDownHandler
{
    PlayerMovement playerMovement;

    public bool isFacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        playerMovement.ChangePlayerControll(this);
    }
}