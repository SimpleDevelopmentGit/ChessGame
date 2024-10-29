using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //The sprites for the slots when selected/taken
    [Header("Refences")]
    public ChessBoard chessBoard;

    [Space]
    public string id;

    //Visual Refences
    [HideInInspector]
    public Sprite AvailableIcon;
    [HideInInspector]
    public Sprite TakenIcon;
    [HideInInspector]
    public GameObject circle;
    [HideInInspector]
    public GameObject currentPiece;

    //Booleans
    [HideInInspector]
    public bool option;

    public string taken;

    private void Start()
    {
        //Instalation
        chessBoard = GameObject.Find("ChessBoard").GetComponent<ChessBoard>();
        circle = chessBoard.circle;
        taken = "null";
    }

    void Update()
    {
        //set the id
        id = gameObject.name;

        //Set slot as an option
        option = chessBoard.selectedSlots.Contains(gameObject);

        //Available Moves Visibility Scipt
        if (chessBoard.selectedSlots.Contains(gameObject))
        {
            //Instansiate visibillity object if object dosent already has it
            if (gameObject.transform.childCount == 0)
            {
                GameObject circleOBJ = Instantiate(circle, transform.position, Quaternion.identity);
                circleOBJ.transform.SetParent(transform, true);
            }
            else 
            {
                //Change design if space is empty or has an enemy in it
                if (taken.Contains("null"))
                {
                    //empty
                    transform.GetChild(0).GetComponent<RawImage>().texture = AvailableIcon.texture;
                    transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(25,25);
                }
                else
                {
                    //taken
                    transform.GetChild(0).GetComponent<RawImage>().texture = TakenIcon.texture;
                    transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(45, 45);
                }

                transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        else
        {
            //deactive 
            if (gameObject.transform.childCount > 0)
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    //Set gameobject when mouse is over
    public void OnPointerEnter(PointerEventData eventData)
    {
        chessBoard.activeHoverSlot = gameObject;
    }

    //remove gameobject when mouse is over
    public void OnPointerExit(PointerEventData eventData)
    {
        chessBoard.activeHoverSlot = null;
    }
}
