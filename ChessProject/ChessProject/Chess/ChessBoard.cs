using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChessBoard : MonoBehaviour
{
    //Gamobjects that will be set during play
    [HideInInspector]
    public GameObject slotsParent, activeHoverSlot, activePieceSlot, selectedSlotOnBoard, selectedPieceSlotSlot;

    //The sprites for the slots when selected/taken
    [Header("Options")]
    public Sprite AvailableIcon;
    public Sprite TakenIcon;

    //The gamobject that will be over any slot that we hover over
    [Space]
    public GameObject selectedOutline;
    public GameObject circle;

    //Black And White pieces prefab
    [Space]
    public GameObject WhitePieces;
    public GameObject BlackPieces;

    //Player Text
    [Space]
    public TMP_Text player1;
    public TMP_Text player2;

    [Space]
    public Color textDisabled;
    public Color textActive;

    //Lists
    [Header("Lists")]
    private List<GameObject> Slots = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> selectedSlots = new List<GameObject>();

    //booleans
    [HideInInspector]
    public bool dragging;
    public bool playing;
    public string turn;

    void Start()
    {
        //Set first player turn
        turn = "white";
        playing = true;

        slotsParent = GetComponentInChildren<GridLayoutGroup>().gameObject;

        //Add all slots to list
        foreach (Transform slot in slotsParent.GetComponentInChildren<Transform>())
        {
            Slots.Add(slot.gameObject);
        }

        //Instalation
        SetSlotsID(Slots);
        SetComponets(Slots);
    }

    void Update()
    {
        //Move the outline to active position
        if (activeHoverSlot)
            selectedOutline.transform.position = activeHoverSlot.transform.position;

        //Set outline active if your hovering over the chess board
        selectedOutline.SetActive(activeHoverSlot);

        //check for available moves
        if(selectedPieceSlotSlot)
            CheckSelectedSlots();

        HandlePlayerStats();
    }

    public void CheckSelectedSlots()
    {
        if (selectedPieceSlotSlot.name.Contains("Pawn"))
        {
            SelectPawnSlots(GetPieceColor(selectedPieceSlotSlot));
        }
        if (selectedPieceSlotSlot.name.Contains("Rook"))
        {
            SelectRookSlots();
        }
        if (selectedPieceSlotSlot.name.Contains("Knight"))
        {
            SelectKnightSlots();
        }
        if (selectedPieceSlotSlot.name.Contains("Bishop"))
        {
            SelectBishopSlots();
        }
        if (selectedPieceSlotSlot.name.Contains("King"))
        {
            SelectKingSlots();
        }
        if (selectedPieceSlotSlot.name.Contains("Queen"))
        {
            SelectQueenSlots();
        }
    }
    public void HandlePlayerStats()
    {
        //Switch whos player text color by turn
        if(turn == "white")
        {
            player1.color = textActive;
            player2.color = textDisabled;
        }
        if (turn == "black")
        {
            player2.color = textActive;
            player1.color = textDisabled;
        }
    }
    public void ResetBoardColors()
    {
        ChessColors chessColors = GetComponent<ChessColors>();

        foreach (RawImage square in chessColors.WhiteSquares)
        {
            square.color = chessColors.Color1;
        }

        foreach (RawImage square in chessColors.BlackSquares)
        {
            square.color = chessColors.Color2;
        }
    }
    public void ResetBoard()
    {
        Destroy(GameObject.Find("Pieces").transform.GetChild(0).gameObject);
        Destroy(GameObject.Find("Pieces").transform.GetChild(1).gameObject);

        turn = "white";
        ResetBoardColors();

        Instantiate(WhitePieces, GameObject.Find("Pieces").transform);
        Instantiate(BlackPieces, GameObject.Find("Pieces").transform);
    }

    //Piece Moves
    private void SelectPawnSlots(int color)
    {
        if (selectedSlotOnBoard)
        {
            if (color == 0)
            {
                if (selectedPieceSlotSlot.GetComponent<Piece>().firstmove)
                {
                    CheckSingleSlot(0, 1, false, color);
                    CheckSingleSlot(0, 2, false, color);
                    CheckSingleSlot(1, 1, true,  color);
                    CheckSingleSlot(-1, 1, true, color);
                }
                else
                {
                    CheckSingleSlot(0, 1, false, color);
                    CheckSingleSlot(1, 1, true,  color);
                    CheckSingleSlot(-1, 1, true, color);
                }
            }
            if (color == 1)
            {
                if (selectedPieceSlotSlot.GetComponent<Piece>().firstmove)
                {
                    CheckSingleSlot(0, -1, false, color);
                    CheckSingleSlot(0, -2, false, color);
                    CheckSingleSlot(1, -1, true,  color);
                    CheckSingleSlot(-1, -1, true, color);
                }
                else
                {
                    CheckSingleSlot(0, -1, false, color);
                    CheckSingleSlot(1, -1, true,  color);
                    CheckSingleSlot(-1, -1, true, color);
                }
            }
        }
        else
        {
            selectedSlots.Clear();
        }
    }
    private void SelectKnightSlots()
    {
        if (selectedSlotOnBoard)
        {
            CheckSingleSlot(1, 2, false, GetPieceColor(selectedPieceSlotSlot));
            CheckSingleSlot(-1, 2, false, GetPieceColor(selectedPieceSlotSlot));

            CheckSingleSlot(1, -2, false, GetPieceColor(selectedPieceSlotSlot));
            CheckSingleSlot(-1, -2, false, GetPieceColor(selectedPieceSlotSlot));

            CheckSingleSlot(2, 1, false, GetPieceColor(selectedPieceSlotSlot));
            CheckSingleSlot(2, -1, false, GetPieceColor(selectedPieceSlotSlot));

            CheckSingleSlot(-2, 1, false, GetPieceColor(selectedPieceSlotSlot));
            CheckSingleSlot(-2, -1, false, GetPieceColor(selectedPieceSlotSlot));
        }
        else
        {
            selectedSlots.Clear();
        }
    }
    private void SelectRookSlots()
    {
        if (selectedSlotOnBoard)
        {
            CheckVerticalLine(8, GetPieceColor(selectedPieceSlotSlot));
            CheckVerticalLine(-8, GetPieceColor(selectedPieceSlotSlot));

            CheckHorizontalLine(8, GetPieceColor(selectedPieceSlotSlot));
            CheckHorizontalLine(-8, GetPieceColor(selectedPieceSlotSlot));
        }
        else
        {
            selectedSlots.Clear();
        }
    }
    private void SelectBishopSlots()
    {
        if (selectedSlotOnBoard)
        {
            CheckDiagnalRight(8, GetPieceColor(selectedPieceSlotSlot));
            CheckDiagnalRight(-8, GetPieceColor(selectedPieceSlotSlot));

            CheckDiagnalLeft(8, GetPieceColor(selectedPieceSlotSlot));
            CheckDiagnalLeft(-8, GetPieceColor(selectedPieceSlotSlot));
        }
        else
        {
            selectedSlots.Clear();
        }
    }
    private void SelectKingSlots()
    {
        if (selectedSlotOnBoard)
        {
            CheckSingleSlot(0, 1, false, GetPieceColor(selectedPieceSlotSlot));
            CheckSingleSlot(-1, 1, false, GetPieceColor(selectedPieceSlotSlot));
            CheckSingleSlot(1, 1, false, GetPieceColor(selectedPieceSlotSlot));

            CheckSingleSlot(0, -1, false, GetPieceColor(selectedPieceSlotSlot));
            CheckSingleSlot(-1, -1, false, GetPieceColor(selectedPieceSlotSlot));
            CheckSingleSlot(1, -1, false, GetPieceColor(selectedPieceSlotSlot));

            CheckSingleSlot(-1, 0, false, GetPieceColor(selectedPieceSlotSlot));
            CheckSingleSlot( 1, 0, false, GetPieceColor(selectedPieceSlotSlot));
        }
        else
        {
            selectedSlots.Clear();
        }
    }
    private void SelectQueenSlots()
    {
        if (selectedSlotOnBoard)
        {
            CheckHorizontalLine(8, GetPieceColor(selectedPieceSlotSlot));
            CheckHorizontalLine(-8, GetPieceColor(selectedPieceSlotSlot));

            CheckVerticalLine(8, GetPieceColor(selectedPieceSlotSlot));
            CheckVerticalLine(-8, GetPieceColor(selectedPieceSlotSlot));

            CheckDiagnalRight(8, GetPieceColor(selectedPieceSlotSlot));
            CheckDiagnalRight(-8, GetPieceColor(selectedPieceSlotSlot));

            CheckDiagnalLeft(8, GetPieceColor(selectedPieceSlotSlot));
            CheckDiagnalLeft(-8, GetPieceColor(selectedPieceSlotSlot));
        }
        else
        {
            selectedSlots.Clear();
        }
    }

    //Slots checks
    private void CheckSingleSlot(float AddedX, float AddedY, bool AvailableWhenTakenException, int color)
    {
        Vector2 offset = new Vector2(AddedX, AddedY);
        GameObject currentSlot = NameToOBJ(CoordinateToId(IdToCoordinate(selectedSlotOnBoard.name) + offset));

        if (currentSlot != null)
        {
            if (!AvailableWhenTakenException)
            {
                if (color == 0)
                {
                    if (currentSlot.GetComponent<Slot>().taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                    {
                        selectedSlots.Add(currentSlot);
                    }
                    if (currentSlot.GetComponent<Slot>().taken.Contains("black") && !selectedSlots.Contains(currentSlot))
                    {
                        selectedSlots.Add(currentSlot);
                    }
                }
                else if (color == 1)
                {
                    if (currentSlot.GetComponent<Slot>().taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                    {
                        selectedSlots.Add(currentSlot);
                    }
                    if (currentSlot.GetComponent<Slot>().taken.Contains("white") && !selectedSlots.Contains(currentSlot))
                    {
                        selectedSlots.Add(currentSlot);
                    }
                }
            }
            else
            {
                if (color == 0)
                {
                    if (currentSlot.GetComponent<Slot>().taken.Contains("black") && !selectedSlots.Contains(currentSlot))
                    {
                        selectedSlots.Add(currentSlot);
                    }
                }
                else if (color == 1)
                {
                    if (currentSlot.GetComponent<Slot>().taken.Contains("white") && !selectedSlots.Contains(currentSlot))
                    {
                        selectedSlots.Add(currentSlot);
                    }
                }
            }
        }
    }
    private void CheckVerticalLine(int distance, int color)
    {
        if (color == 0)
        {
            if (distance > 0)
            {
                for (int i = 1; i <= Mathf.Abs(distance); i++)
                {
                    Vector2 offset = new Vector2(0, i);
                    GameObject currentSlot = NameToOBJ(CoordinateToId(IdToCoordinate(selectedSlotOnBoard.name) + offset));

                    if (currentSlot != null)
                    {
                        Slot slot = currentSlot.GetComponent<Slot>();
                        if (slot.taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                        }
                        else if (slot.taken.Contains("black") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                            i = 999;
                        }
                        else
                        {
                            i = 999;
                        }
                    }
                }
            }
            else
            {
                for (int i = 1; i <= Mathf.Abs(distance); i++)
                {
                    Vector2 offset = new Vector2(0, i);
                    GameObject currentSlot = NameToOBJ(CoordinateToId(IdToCoordinate(selectedSlotOnBoard.name) - offset));

                    if (currentSlot != null)
                    {
                        Slot slot = currentSlot.GetComponent<Slot>();
                        if (slot.taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                        }
                        else if (slot.taken.Contains("black") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                            i = 999;
                        }
                        else
                        {
                            i = 999;
                        }
                    }
                }
            }
        }
        if (color == 1)
        {
            if (distance > 0)
            {
                for (int i = 1; i <= Mathf.Abs(distance); i++)
                {
                    Vector2 offset = new Vector2(0, i);
                    GameObject currentSlot = NameToOBJ(CoordinateToId(IdToCoordinate(selectedSlotOnBoard.name) + offset));

                    if (currentSlot != null)
                    {
                        Slot slot = currentSlot.GetComponent<Slot>();
                        if (slot.taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                        }
                        else if (slot.taken.Contains("white") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                            i = 999;
                        }
                        else
                        {
                            i = 999;
                        }
                    }
                }
            }
            else
            {
                for (int i = 1; i <= Mathf.Abs(distance); i++)
                {
                    Vector2 offset = new Vector2(0, i);
                    GameObject currentSlot = NameToOBJ(CoordinateToId(IdToCoordinate(selectedSlotOnBoard.name) - offset));

                    if (currentSlot != null)
                    {
                        Slot slot = currentSlot.GetComponent<Slot>();
                        if (slot.taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                        }
                        else if (slot.taken.Contains("white") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                            i = 999;
                        }
                        else
                        {
                            i = 999;
                        }
                    }
                }
            }
        }
    }
    private void CheckHorizontalLine(int distance, int color)
    {
        if (color == 0)
        {
            if (distance > 0)
            {
                for (int i = 1; i <= Mathf.Abs(distance); i++)
                {
                    Vector2 offset = new Vector2(i, 0);
                    GameObject currentSlot = NameToOBJ(CoordinateToId(IdToCoordinate(selectedSlotOnBoard.name) + offset));

                    if (currentSlot != null)
                    {
                        Slot slot = currentSlot.GetComponent<Slot>();
                        if (slot.taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                        }
                        else if (slot.taken.Contains("black") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                            i = 999;
                        }
                        else
                        {
                            i = 999;
                        }
                    }
                }
            }
            else
            {
                for (int i = 1; i <= Mathf.Abs(distance); i++)
                {
                    Vector2 offset = new Vector2(i, 0);
                    GameObject currentSlot = NameToOBJ(CoordinateToId(IdToCoordinate(selectedSlotOnBoard.name) - offset));

                    if (currentSlot != null)
                    {
                        Slot slot = currentSlot.GetComponent<Slot>();
                        if (slot.taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                        }
                        else if (slot.taken.Contains("black") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                            i = 999;
                        }
                        else
                        {
                            i = 999;
                        }
                    }
                }
            }
        }
        if (color == 1)
        {
            if (distance > 0)
            {
                for (int i = 1; i <= Mathf.Abs(distance); i++)
                {
                    Vector2 offset = new Vector2(i, 0);
                    GameObject currentSlot = NameToOBJ(CoordinateToId(IdToCoordinate(selectedSlotOnBoard.name) + offset));

                    if (currentSlot != null)
                    {
                        Slot slot = currentSlot.GetComponent<Slot>();
                        if (slot.taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                        }
                        else if (slot.taken.Contains("white") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                            i = 999;
                        }
                        else
                        {
                            i = 999;
                        }
                    }
                }
            }
            else
            {
                for (int i = 1; i <= Mathf.Abs(distance); i++)
                {
                    Vector2 offset = new Vector2(i, 0);
                    GameObject currentSlot = NameToOBJ(CoordinateToId(IdToCoordinate(selectedSlotOnBoard.name) - offset));

                    if (currentSlot != null)
                    {
                        Slot slot = currentSlot.GetComponent<Slot>();
                        if (slot.taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                        }
                        else if (slot.taken.Contains("white") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                            i = 999;
                        }
                        else
                        {
                            i = 999;
                        }
                    }
                }
            }
        }
    }
    private void CheckDiagnalRight(int distance, int color)
    {
        if (color == 0)
        {
            if (distance > 0)
            {
                for (int i = 1; i <= Mathf.Abs(distance); i++)
                {
                    Vector2 offset = new Vector2(i, i);
                    GameObject currentSlot = NameToOBJ(CoordinateToId(IdToCoordinate(selectedSlotOnBoard.name) + offset));

                    if (currentSlot != null)
                    {
                        Slot slot = currentSlot.GetComponent<Slot>();
                        if (slot.taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                        }
                        else if (slot.taken.Contains("black") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                            i = 999;
                        }
                        else
                        {
                            i = 999;
                        }
                    }
                }
            }
            else
            {
                for (int i = 1; i <= Mathf.Abs(distance); i++)
                {
                    Vector2 offset = new Vector2(i, -i);
                    GameObject currentSlot = NameToOBJ(CoordinateToId(IdToCoordinate(selectedSlotOnBoard.name) + offset));

                    if (currentSlot != null)
                    {
                        Slot slot = currentSlot.GetComponent<Slot>();
                        if (slot.taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                        }
                        else if (slot.taken.Contains("black") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                            i = 999;
                        }
                        else
                        {
                            i = 999;
                        }
                    }
                }
            }
        }
        if (color == 1)
        {
            if (distance > 0)
            {
                for (int i = 1; i <= Mathf.Abs(distance); i++)
                {
                    Vector2 offset = new Vector2(i, i);
                    GameObject currentSlot = NameToOBJ(CoordinateToId(IdToCoordinate(selectedSlotOnBoard.name) + offset));

                    if (currentSlot != null)
                    {
                        Slot slot = currentSlot.GetComponent<Slot>();
                        if (slot.taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                        }
                        else if (slot.taken.Contains("white") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                            i = 999;
                        }
                        else
                        {
                            i = 999;
                        }
                    }
                }
            }
            else
            {
                for (int i = 1; i <= Mathf.Abs(distance); i++)
                {
                    Vector2 offset = new Vector2(i, -i);
                    GameObject currentSlot = NameToOBJ(CoordinateToId(IdToCoordinate(selectedSlotOnBoard.name) + offset));

                    if (currentSlot != null)
                    {
                        Slot slot = currentSlot.GetComponent<Slot>();
                        if (slot.taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                        }
                        else if (slot.taken.Contains("white") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                            i = 999;
                        }
                        else
                        {
                            i = 999;
                        }
                    }
                }
            }
        }
    }
    private void CheckDiagnalLeft(int distance, int color)
    {
        if (color == 0)
        {
            if (distance > 0)
            {
                for (int i = 1; i <= Mathf.Abs(distance); i++)
                {
                    Vector2 offset = new Vector2(-i, i);
                    GameObject currentSlot = NameToOBJ(CoordinateToId(IdToCoordinate(selectedSlotOnBoard.name) + offset));

                    if (currentSlot != null)
                    {
                        Slot slot = currentSlot.GetComponent<Slot>();
                        if (slot.taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                        }
                        else if (slot.taken.Contains("black") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                            i = 999;
                        }
                        else
                        {
                            i = 999;
                        }
                    }
                }
            }
            else
            {
                for (int i = 1; i <= Mathf.Abs(distance); i++)
                {
                    Vector2 offset = new Vector2(-i, -i);
                    GameObject currentSlot = NameToOBJ(CoordinateToId(IdToCoordinate(selectedSlotOnBoard.name) + offset));

                    if (currentSlot != null)
                    {
                        Slot slot = currentSlot.GetComponent<Slot>();
                        if (slot.taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                        }
                        else if (slot.taken.Contains("black") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                            i = 999;
                        }
                        else
                        {
                            i = 999;
                        }
                    }
                }
            }
        }
        if (color == 1)
        {
            if (distance > 0)
            {
                for (int i = 1; i <= Mathf.Abs(distance); i++)
                {
                    Vector2 offset = new Vector2(-i, i);
                    GameObject currentSlot = NameToOBJ(CoordinateToId(IdToCoordinate(selectedSlotOnBoard.name) + offset));

                    if (currentSlot != null)
                    {
                        Slot slot = currentSlot.GetComponent<Slot>();
                        if (slot.taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                        }
                        else if (slot.taken.Contains("white") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                            i = 999;
                        }
                        else
                        {
                            i = 999;
                        }
                    }
                }
            }
            else
            {
                for (int i = 1; i <= Mathf.Abs(distance); i++)
                {
                    Vector2 offset = new Vector2(-i, -i);
                    GameObject currentSlot = NameToOBJ(CoordinateToId(IdToCoordinate(selectedSlotOnBoard.name) + offset));

                    if (currentSlot != null)
                    {
                        Slot slot = currentSlot.GetComponent<Slot>();
                        if (slot.taken.Contains("null") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                        }
                        else if (slot.taken.Contains("white") && !selectedSlots.Contains(currentSlot))
                        {
                            selectedSlots.Add(currentSlot);
                            i = 999;
                        }
                        else
                        {
                            i = 999;
                        }
                    }
                }
            }
        }
    }

    //Functions
    public Vector2 IdToCoordinate(string id)
    {
        string row = id[0].ToString();
        string colum = id[2].ToString();

        return new Vector2(LetterToNumber(colum), float.Parse(row));
    }
    public string CoordinateToId(Vector2 Coordinate)
    {
        float row = Coordinate.y;
        string colum = Coordinate.x.ToString();

        return row.ToString() + ":" + NumberToLetter(colum);
    }
    public float LetterToNumber(string letter)
    {
        if (letter == "a")
            return 1;
        if (letter == "b")
            return 2;
        if (letter == "c")
            return 3;
        if (letter == "d")
            return 4;
        if (letter == "e")
            return 5;
        if (letter == "f")
            return 6;
        if (letter == "g")
            return 7;
        if (letter == "h")
            return 8;
        else
        {
            return -999999;
        }
    }
    public string NumberToLetter(string letter)
    {
        if (letter == "1")
            return "a";
        if (letter == "2")
            return "b";
        if (letter == "3")
            return "c";
        if (letter == "4")
            return "d";
        if (letter == "5")
            return "e";
        if (letter == "6")
            return "f";
        if (letter == "7")
            return "g";
        if (letter == "8")
            return "h";
        else
        {
            return "Error";
        }
    }
    public GameObject NameToOBJ(string name)
    {
        return GameObject.Find(name);
    }
    public string OppisiteColor(int color)
    {
        //0 = white
        //1 = black

        if(color == 0)
        {
            return "white";
        }
        if (color == 1)
        {
            return "black";
        }
        else
        {
            Debug.LogError("index color --> "+ color + " dose not exsist");
            return "Invalid";
        }
    }
    public string OppisiteColor2(int color)
    {
        //0 = white
        //1 = black

        if (color == 0)
        {
            return "black";
        }
        if (color == 1)
        {
            return "white";
        }
        else
        {
            Debug.LogError("index color --> " + color + " dose not exsist");
            return "Invalid";
        }
    }
    public int GetPieceColor(GameObject piece)
    {
        string name = piece.name;

        if (name.Contains("white"))
        {
            return 0;
        }
        if (name.Contains("black"))
        {
            return 1;
        }
        else
        {
            return -999;
        }
    }

    //Instilation
    private void SetSlotsID(List<GameObject> slots)
    {
        for (int i = 0; i < 65; i++)
        {
            //first row
            if (i == 0)
                slots[i].name = 8 + ":" + "a";
            if (i == 1)
                slots[i].name = 8 + ":" + "b";
            if (i == 2)
                slots[i].name = 8 + ":" + "c";
            if (i == 3)
                slots[i].name = 8 + ":" + "d";
            if (i == 4)
                slots[i].name = 8 + ":" + "e";
            if (i == 5)
                slots[i].name = 8 + ":" + "f";
            if (i == 6)
                slots[i].name = 8 + ":" + "g";
            if (i == 7)
                slots[i].name = 8 + ":" + "h";

            //second row
            if (i == 0 + 8)
                slots[i].name = 7 + ":" + "a";
            if (i == 1 + 8)
                slots[i].name = 7 + ":" + "b";
            if (i == 2 + 8)
                slots[i].name = 7 + ":" + "c";
            if (i == 3 + 8)
                slots[i].name = 7 + ":" + "d";
            if (i == 4 + 8)
                slots[i].name = 7 + ":" + "e";
            if (i == 5 + 8)
                slots[i].name = 7 + ":" + "f";
            if (i == 6 + 8)
                slots[i].name = 7 + ":" + "g";
            if (i == 7 + 8)
                slots[i].name = 7 + ":" + "h";

            //third row
            if (i == 0 + 16)
                slots[i].name = 6 + ":" + "a";
            if (i == 1 + 16)
                slots[i].name = 6 + ":" + "b";
            if (i == 2 + 16)
                slots[i].name = 6 + ":" + "c";
            if (i == 3 + 16)
                slots[i].name = 6 + ":" + "d";
            if (i == 4 + 16)
                slots[i].name = 6 + ":" + "e";
            if (i == 5 + 16)
                slots[i].name = 6 + ":" + "f";
            if (i == 6 + 16)
                slots[i].name = 6 + ":" + "g";
            if (i == 7 + 16)
                slots[i].name = 6 + ":" + "h";

            //fourth row
            if (i == 0 + 24)
                slots[i].name = 5 + ":" + "a";
            if (i == 1 + 24)
                slots[i].name = 5 + ":" + "b";
            if (i == 2 + 24)
                slots[i].name = 5 + ":" + "c";
            if (i == 3 + 24)
                slots[i].name = 5 + ":" + "d";
            if (i == 4 + 24)
                slots[i].name = 5 + ":" + "e";
            if (i == 5 + 24)
                slots[i].name = 5 + ":" + "f";
            if (i == 6 + 24)
                slots[i].name = 5 + ":" + "g";
            if (i == 7 + 24)
                slots[i].name = 5 + ":" + "h";

            //five row
            if (i == 0 + 32)
                slots[i].name = 4 + ":" + "a";
            if (i == 1 + 32)
                slots[i].name = 4 + ":" + "b";
            if (i == 2 + 32)
                slots[i].name = 4 + ":" + "c";
            if (i == 3 + 32)
                slots[i].name = 4 + ":" + "d";
            if (i == 4 + 32)
                slots[i].name = 4 + ":" + "e";
            if (i == 5 + 32)
                slots[i].name = 4 + ":" + "f";
            if (i == 6 + 32)
                slots[i].name = 4 + ":" + "g";
            if (i == 7 + 32)
                slots[i].name = 4 + ":" + "h";

            //sixth row
            if (i == 0 + 40)
                slots[i].name = 3 + ":" + "a";
            if (i == 1 + 40)
                slots[i].name = 3 + ":" + "b";
            if (i == 2 + 40)
                slots[i].name = 3 + ":" + "c";
            if (i == 3 + 40)
                slots[i].name = 3 + ":" + "d";
            if (i == 4 + 40)
                slots[i].name = 3 + ":" + "e";
            if (i == 5 + 40)
                slots[i].name = 3 + ":" + "f";
            if (i == 6 + 40)
                slots[i].name = 3 + ":" + "g";
            if (i == 7 + 40)
                slots[i].name = 3 + ":" + "h";

            //seventh row
            if (i == 0 + 48)
                slots[i].name = 2 + ":" + "a";
            if (i == 1 + 48)
                slots[i].name = 2 + ":" + "b";
            if (i == 2 + 48)
                slots[i].name = 2 + ":" + "c";
            if (i == 3 + 48)
                slots[i].name = 2 + ":" + "d";
            if (i == 4 + 48)
                slots[i].name = 2 + ":" + "e";
            if (i == 5 + 48)
                slots[i].name = 2 + ":" + "f";
            if (i == 6 + 48)
                slots[i].name = 2 + ":" + "g";
            if (i == 7 + 48)
                slots[i].name = 2 + ":" + "h";
            //seventh row
            if (i == 0 + 56)
                slots[i].name = 1 + ":" + "a";
            if (i == 1 + 56)
                slots[i].name = 1 + ":" + "b";
            if (i == 2 + 56)
                slots[i].name = 1 + ":" + "c";
            if (i == 3 + 56)
                slots[i].name = 1 + ":" + "d";
            if (i == 4 + 56)
                slots[i].name = 1 + ":" + "e";
            if (i == 5 + 56)
                slots[i].name = 1 + ":" + "f";
            if (i == 6 + 56)
                slots[i].name = 1 + ":" + "g";
            if (i == 7 + 56)
                slots[i].name = 1 + ":" + "h";
        }
    }
    private void SetComponets(List<GameObject> slots)
    {
        foreach (GameObject slot in slots)
        {
            slot.GetComponent<Slot>().AvailableIcon = AvailableIcon;
            slot.GetComponent<Slot>().TakenIcon = TakenIcon;
        }
    }
}
