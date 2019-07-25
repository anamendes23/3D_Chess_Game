using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(CameraRaycaster))]
public class GameController : MonoBehaviour
{
    //get GameController object
    private GameObject gameManager;
    private GameObject gameController;

    [SerializeField]
    private GameObject redTeam;
    [SerializeField]
    private GameObject blueTeam;
    private List<Transform> gamePieces = new List<Transform>();
    private List<Piece> chessPieces = new List<Piece>();
    public List<string> piecesPositions = new List<string>();
    private List<Piece> kings = new List<Piece>();

    public int firstBlue;
    public int lastBlue;
    public int firstRed;
    public int lastRed;

    CameraRaycaster cameraRaycaster;

    public List<string> graveyard = new List<string>();
    public int turn;
    public bool whoTurn;

    public List<List<string>> attackBluePieces = new List<List<string>>();
    public List<List<string>> attackRedPieces = new List<List<string>>();

    public Transform[] GamePieces { get { return gamePieces.ToArray(); } }
    public Piece[] ChessPieces { get { return chessPieces.ToArray(); } }

    private SelectorController sc;
    public Grid grid;

    public delegate void GameOver(string message); //declare new delegate type
    public event GameOver gameOver; // instantiate an observer set

    // Start is called before the first frame update
    void Awake()
    {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        sc = FindObjectOfType<SelectorController>();
        gameManager = GameObject.Find("GameManager");
        gameController = GameObject.Find("GameController");
        grid = FindObjectOfType<Grid>();
        AddPieces(blueTeam);
        AddPieces(redTeam);
        turn = -1;
        whoTurn = true;
        firstBlue = 0;
        lastBlue = 16;
        firstRed = 16;
        lastRed = 32;

        if (GameManager.toBeOrNotToBe)
            AddAiController();
    }

    void AddAiController()
    {
        gameController.AddComponent<AiController>();
    }

    private void AddPieces(GameObject go)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            //Transform t = go.transform.GetChild(i);
            //int x = Mathf.RoundToInt(t.position.x);
            //int y = Mathf.RoundToInt(t.position.y);
            //int z = Mathf.RoundToInt(t.position.z);
            //t.position = new Vector3(x, y, z);
            gamePieces.Add(go.transform.GetChild(i));
            //create Piece based on tag
            chessPieces.Add(CreatePieces(go.transform.GetChild(i)));
            InitializePositions(go.transform.GetChild(i));
        }
    }

    private void InitializePositions(Transform child)
    {
        Vector3 position = sc.GetPosition(child.gameObject.transform.position);
        ThirdPersonCharacter ethan = child.GetComponent<ThirdPersonCharacter>();

        string[,] positions = grid.Positions;
        int i = Mathf.Abs(Mathf.RoundToInt(position.x / grid.Size));
        int j = Mathf.Abs(Mathf.RoundToInt(position.z / grid.Size));
        ethan.boardPosition = positions[j, i];
        piecesPositions.Add(positions[j, i]);

        //initialize attackables lists
        if (child.gameObject.layer == 11) //blue
            attackBluePieces.Add(ethan.attackPositions);
        else if (child.gameObject.layer == 12)
            attackRedPieces.Add(ethan.attackPositions);
    }

    private Piece CreatePieces(Transform child)
    {
        string tag = child.tag;
        int layer = child.gameObject.layer;
        ThirdPersonCharacter ethan = child.GetComponent<ThirdPersonCharacter>();

        Piece piece;
        switch (tag)
        {
            case "Pawn":
                piece = new Pawn(layer);
                break;
            case "Knight":
                piece = new Knight(layer);
                break;
            case "Queen":
                piece = new Queen(layer);
                break;
            case "Bishop":
                piece = new Bishop(layer);
                break;
            case "Rook":
                piece = new Rook(layer);
                break;
            case "King":
                piece = new King(layer);
                kings.Add(piece);
                break;
            default:
                piece = null;
                break;
        }

        ethan.pieceType = piece;

        return piece;
    }

    public void Attack(GameObject go)
    {
        ThirdPersonCharacter deadOne = go.GetComponent<ThirdPersonCharacter>();

        if (go.name == "BlueKing" || go.name == "RedKing")
        {
            King k = (King)deadOne.pieceType;
            k.IsInCheckMate = true;
            //stop game
            string message = "Check mate!";
            //message += k.Layer + "lost";
            gameOver(message);
        }
        //why destroying twice??
        if (deadOne.pieceType.IsZombie == false)
        {
            Destroy(go);
        }

        //get index to remove item from all lists
        int indexInLists = gamePieces.IndexOf(go.transform);
        //remove from lists
        gamePieces.RemoveAt(indexInLists);
        chessPieces.RemoveAt(indexInLists);
        piecesPositions.RemoveAt(indexInLists);

        Debug.Log("layer bitch: " + go.layer);
        if (go.layer == 11) //blue
        {
            lastBlue--;
            firstRed--;
            lastRed--;
        }
        else if (go.layer == 12) //red
            lastRed--;

        string pieceType = go.name;
        graveyard.Add(pieceType);        
        Destroy(go);
        //clear attacks list
        deadOne.attackPositions.Clear();
    }

    public bool PlayerTurn(int turn)
    {
        switch (turn)
        {
            case -1:
                whoTurn = true;
                break;
            case 1:
                whoTurn = false;
                break;
            default:
                break;
        }

        return whoTurn;
    }

    public void CheckGameStatus()
    {
        foreach (King k in kings)
        {
            KingInCheck(k);
            Debug.Log(k.Layer + " King is in check? " + k.IsInCheck);
            Debug.Log(k.Layer + " King is in check mate? " + k.IsInCheckMate);
        }
    }

    public void KingInCheck(King k)
    {
        //check layer first
        if (k.Layer == 12)
            RunThroughOpponents(k, firstBlue, lastBlue);
        else if (k.Layer == 11)
            RunThroughOpponents(k, firstRed, lastRed);
    }

    public void RunThroughOpponents(King k, int first, int last)
    {
        k.IsInCheck = false;
        k.IsInCheckMate = false;

        int index = chessPieces.IndexOf(k);
        Debug.Log(index);
        ThirdPersonCharacter king = gamePieces[index].GetComponent<ThirdPersonCharacter>();
        Pair p = GetPairInGrid(king.boardPosition, grid.Positions);

        for (int i = first; i < last; i++)
        {
            ThirdPersonCharacter ethan = gamePieces[i].GetComponent<ThirdPersonCharacter>();
            //update movables and attackables
            chessPieces[i].CheckSurroundings(ethan.boardPosition, piecesPositions, grid.Positions, GamePieces);
            //check if king belongs to attackables
            if (chessPieces[i].attackableSpots.Contains(p))
            {
                k.IsInCheck = true;
                break;
            }
            //countMovesAroundKing++;
            //check if king surroundings belong to movables
            //foreach (Pair item in chessPieces[i].movableSpots)
            //{
            //    foreach (Pair pp in k.possibleChecks)
            //    {
            //        if (item.Equals(pp))
            //            countMovesAroundKing++;
            //    }
            //}
        }

        //if (countMovesAroundKing > 0)
        //    k.IsInCheck = true;
        //if (countMovesAroundKing >= k.countOfPossibleChecks)
        //{
        //    k.IsInCheckMate = true;
        //    //stop game
        //    string message = "Check mate!";
        //    //message += k.Layer + "lost";
        //    gameOver(message);
        //}
    }

    public Pair GetPairInGrid(string position, string[,] gridPositions)
    {
        //hold position of piece in grid
        int i = -1;
        int j = -1;
        //loop through grid to find piece
        for (int k = 0; k < gridPositions.GetLength(0); k++)
        {
            for (int m = 0; m < gridPositions.GetLength(1); m++)
            {
                if (gridPositions[k, m] == position)
                {
                    i = k;
                    j = m;
                    break;
                }
            }
            if (i == k)
                break;
        }

        return new Pair { X = j, Z = i };
    }
}
