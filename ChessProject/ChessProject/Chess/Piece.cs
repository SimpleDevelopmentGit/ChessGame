using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    //Set the starting pos but will change at runtime
    [Header("Starting Pos")]
    [Range(1,8)]
    public int startingPosX;
    [Range(1, 8)]
    public int startingPosY;

    //Our chess board refence
    [Header("Refences")]
    public ChessBoard chessBoard;
    public Canvas canvas;

    //Store our previous slot
    private GameObject previousSlot;
    bool IsDragging;

    //Store weather this is our first move
    [HideInInspector]
    public bool firstmove = true;

    private void Start()
    {
        chessBoard = GameObject.Find("ChessBoard").GetComponent<ChessBoard>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        //Make sure we set our fisrt move to treu
        firstmove = true;
    }

    void Update()
    {
        //Check if we not dragging
        if (!IsDragging)
        {
            //Set our pieces position to the board based on out x/y
            transform.position = chessBoard.NameToOBJ(chessBoard.CoordinateToId(new Vector2(startingPosX, startingPosY))).transform.position;
            chessBoard.NameToOBJ(chessBoard.CoordinateToId(new Vector2(startingPosX, startingPosY))).GetComponent<Slot>().currentPiece = gameObject;

            //Set the taken state of the current slot
            SetStarterSquare();
        }

        //If this gamobject is hovered
        if (chessBoard.activePieceSlot == gameObject)
        {
            if (Input.GetMouseButtonDown(0) && gameObject.name.Contains(chessBoard.turn))
            {
                chessBoard.selectedSlots.Clear();
                chessBoard.selectedSlotOnBoard = chessBoard.NameToOBJ(chessBoard.CoordinateToId(new Vector2(startingPosX, startingPosY)));

                if (gameObject.name.Contains(chessBoard.turn))
                {
                    chessBoard.ResetBoardColors();
                    chessBoard.NameToOBJ(chessBoard.CoordinateToId(new Vector2(startingPosX, startingPosY))).GetComponent<RawImage>().color = new Color(1, 0.514151f, 0.6251429f);
                }

                chessBoard.selectedPieceSlotSlot = gameObject;
            }
        }

        //set gameobject raycast target to weather we are dragging or not
        GetComponent<Image>().raycastTarget = !chessBoard.dragging;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Dragging the object
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, canvas.worldCamera, out position);
        transform.position = canvas.transform.TransformPoint(position);

        //Set the selected slot place
        chessBoard.selectedSlotOnBoard = chessBoard.NameToOBJ(chessBoard.CoordinateToId(new Vector2(startingPosX, startingPosY)));
        previousSlot = chessBoard.NameToOBJ(chessBoard.CoordinateToId(new Vector2(startingPosX, startingPosY)));

        //Set booleans
        IsDragging = true;
        chessBoard.dragging = true;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (chessBoard.activeHoverSlot && chessBoard.activeHoverSlot.GetComponent<Slot>().option)
        {
            //Set slot gameobjects
            GameObject slotOBJ = chessBoard.NameToOBJ(chessBoard.CoordinateToId(chessBoard.IdToCoordinate(chessBoard.activeHoverSlot.name)));
            Slot slot = slotOBJ.GetComponent<Slot>();
            Slot previousSlotC = chessBoard.NameToOBJ(previousSlot.name).GetComponent<Slot>();

            //If the slot is taken destroy the other piece
            if (!slot.taken.Contains("null"))
            {
                Destroy(slot.currentPiece);

                startingPosX = (int)chessBoard.IdToCoordinate(chessBoard.activeHoverSlot.name).x;
                startingPosY = (int)chessBoard.IdToCoordinate(chessBoard.activeHoverSlot.name).y;

                slot.taken = gameObject.name;
                slot.currentPiece = gameObject;
                previousSlotC.taken = "null";
                previousSlotC.currentPiece = null;

                //Change Board Color
                if (gameObject.name.Contains(chessBoard.turn))
                {
                    chessBoard.NameToOBJ(chessBoard.activeHoverSlot.name).GetComponent<RawImage>().color = new Color(1, 0.7877358f, 0.8356665f);
                }
            }
            else
            {
                startingPosX = (int)chessBoard.IdToCoordinate(chessBoard.activeHoverSlot.name).x;
                startingPosY = (int)chessBoard.IdToCoordinate(chessBoard.activeHoverSlot.name).y;

                slot.taken = gameObject.name;
                slot.currentPiece = gameObject;
                previousSlotC.taken = "null";
                previousSlotC.currentPiece = null;

                //Change Board Color
                if (gameObject.name.Contains(chessBoard.turn))
                {
                    chessBoard.NameToOBJ(chessBoard.activeHoverSlot.name).GetComponent<RawImage>().color = new Color(1, 0.7877358f, 0.8356665f);
                }
            }

            //change first move
            firstmove = false;

            //sounds
            chessBoard.GetComponent<AudioSource>().Play();

            //Change to the other players turn
            chessBoard.turn = chessBoard.OppisiteColor2(chessBoard.GetPieceColor(gameObject));
        }
        else
        {
            startingPosX = (int)chessBoard.IdToCoordinate(previousSlot.name).x;
            startingPosY = (int)chessBoard.IdToCoordinate(previousSlot.name).y;
        }

        chessBoard.selectedSlotOnBoard = null;
        chessBoard.selectedPieceSlotSlot = null;
        chessBoard.selectedSlots.Clear();

        IsDragging = false;
        chessBoard.dragging = false;
    }

    public void SetStarterSquare()
    {
        //Update slots taken
        chessBoard.NameToOBJ(chessBoard.CoordinateToId(new Vector2(startingPosX, startingPosY))).GetComponent<Slot>().taken = gameObject.name;
    }

    public void OnPointerEnter(PointerEventData eventData) { chessBoard.activePieceSlot = gameObject; }
    public void OnPointerExit(PointerEventData eventData) { chessBoard.activePieceSlot = null; }
}
