using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class SelectorController : MonoBehaviour
{
    private Grid grid;
    private ThirdPersonCharacter selectedPiece;
    public GameController gc;
    private PieceController pc;
    private CameraRaycaster cameraRaycaster;
    private GameObject newSelected = null;
    private bool pieceSelected = false;
    private Vector3 position;
    private bool movePiece;
    private bool canMove;
    private int blueturn;
    private int redturn;
    private int view;
    public int turn;

    public delegate void TurnChange(int nextTurn); //declare new delegate type
    public event TurnChange turnChange; // instantiate an observer set
    public delegate void ViewChange(int viewChanged);
    public event ViewChange viewChange;

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        gc = FindObjectOfType<GameController>();
        pc = FindObjectOfType<PieceController>();
        canMove = false;
        blueturn = 1;
        redturn = -1;
        turn = -1;
        view = 0;
    }

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
    }


    // Update is called once per frame
    void Update()
    {
        ProcessGameAction();
    }

    public Vector3 GetPosition(Vector3 clickPoint)
    {
        Vector3 position2 = grid.GridPoint(clickPoint);
        //string[,] positions = grid.Positions;
        //Debug.Log("x: " + position.x + " y: " + position.y + " z: " + position.z);
        //int i = Mathf.Abs(Mathf.RoundToInt(position.x / grid.Size));
        //int j = Mathf.Abs(Mathf.RoundToInt(position.z / grid.Size));
        //Debug.Log("" + j + " " + i);        
        //Debug.Log(positions[j, i]);
        return position2;
    }

    //process actions in game
    private void ProcessGameAction()
    {
        //GameObject newSelected = null;
        if (Input.GetKeyDown("v"))
        {
            view++;
            ChangeView(view % 3);
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = cameraRaycaster.hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Collider[] objectsInArea = Physics.OverlapSphere(hitInfo.point, 2);
            //Debug.Log(cameraRaycaster.currentLayerHit);
            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Board:
                    if (pieceSelected)
                    {
                        pc.ResetMoveTiles();
                        selectedPiece.SetSelection(false);
                        pieceSelected = false;
                    }
                    break;
                case Layer.BlueTeam:
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        if (pieceSelected == true)
                        {
                            selectedPiece.SetSelection(false);
                            pieceSelected = false;

                            if (pc.attackTiles.Count > 0)
                            {
                                float minDist = 14;

                                foreach (var attack in pc.attackTiles)
                                {
                                    Vector3 test = hitInfo.transform.position - attack.transform.position;
                                    float test1 = test.magnitude;
                                    if (test1 < minDist)
                                    {
                                        Move_Attack(hitInfo);
                                        //Debug.Log(attack.name);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //selectedPiece.SetSelection(false);
                            //pieceSelected = false;

                            GrabPiece(hitInfo);
                        }
                        //spawn move tile
                        //Vector3 tilePosition = new Vector3(position.x, position.y, position.z - 10);
                        pc.PlaceMoveTile(position, selectedPiece);
                    }
                    canMove = gc.PlayerTurn(turn * blueturn);
                    break;
                case Layer.RedTeam:
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        if (pieceSelected == true)
                        {
                            selectedPiece.SetSelection(false);
                            pieceSelected = false;

                            if (pc.attackTiles.Count > 0)
                            {
                                float minDist = 14;

                                foreach (var attack in pc.attackTiles)
                                {
                                    Vector3 test = hitInfo.transform.position - attack.transform.position;
                                    float test1 = test.magnitude;
                                    if (test1 < minDist)
                                    {
                                        Move_Attack(hitInfo);
                                        //Debug.Log(attack.name);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //selectedPiece.SetSelection(false);
                            //pieceSelected = false;

                            GrabPiece(hitInfo);
                        }
                        //newSelected = hitInfo.collider.gameObject;
                        //selectedPiece = newSelected.GetComponent<ThirdPersonCharacter>();
                        //position = selectedPiece.transform.position;
                        //selectedPiece.SetSelection(true);
                        //pieceSelected = true;
                        //movePiece = true;
                        //spawn move tile
                        //Vector3 tilePosition = new Vector3(position.x, position.y, position.z - 10);   
                        pc.PlaceMoveTile(position, selectedPiece);
                        canMove = gc.PlayerTurn(turn * redturn);
                    }
                    break;
                case Layer.MoveTiles:
                    if (canMove)
                    {
                        Move_Attack(hitInfo);
                    }
                    break;
                case Layer.RaycastEndStop:
                    break;
                default:
                    break;
            }
        }
    }

    private void GrabPiece(RaycastHit hitInfo)
    {
        newSelected = hitInfo.collider.gameObject;
        selectedPiece = newSelected.GetComponent<ThirdPersonCharacter>();
        position = selectedPiece.transform.position;
        selectedPiece.SetSelection(true);
        pieceSelected = true;
        movePiece = true;
    }

    private void Move_Attack(RaycastHit hitInfo)
    {
        Vector3 movePosition = GetPosition(hitInfo.collider.transform.position);
        ThirdPersonCharacter attacker = selectedPiece;

        if (hitInfo.collider.gameObject.tag == "Move")
        {
            pc.ResetMoveTiles();
            movePiece = false;
            pieceSelected = false;
            selectedPiece.SetSelection(false);
            selectedPiece.pieceType.Move(movePosition, selectedPiece.gameObject, gc);
        }

        if (hitInfo.collider.gameObject.tag == "Attack")
        {

            //creates an array of colliders inside the sphere(center, radius)
            Collider[] deadCircle = Physics.OverlapSphere(hitInfo.point, 5);

            //find the piece being attacked and pass it to the Attack method
            foreach (Collider item in deadCircle)
            {
                newSelected = item.gameObject;
                if (newSelected.GetComponent<ThirdPersonCharacter>())
                {
                    gc.Attack(newSelected);
                }
            }

            pc.ResetMoveTiles();
            movePiece = false;
            pieceSelected = false;
            attacker.SetSelection(false);
            Debug.Log(movePosition);
            attacker.pieceType.Move(movePosition, selectedPiece.gameObject, gc);
        }

        if (hitInfo.collider.gameObject.layer == 11 || hitInfo.collider.gameObject.layer == 12)
        {
            gc.Attack(hitInfo.collider.gameObject);

            pc.ResetMoveTiles();
            movePiece = false;
            pieceSelected = false;
            attacker.SetSelection(false);
            //Debug.Log(movePosition);
            attacker.pieceType.Move(movePosition, selectedPiece.gameObject, gc);
        }
        turn *= -1;
        turnChange(turn);


        gc.CheckGameStatus();
    }

    public void Move_Attack(ThirdPersonCharacter attacker, GameObject attacked, Vector3 movePosition)
    {
        //not attackable pieces, just move
        if (attacked == null)
        {
            pc.ResetMoveTiles();
            movePiece = false;
            pieceSelected = false;
            selectedPiece.SetSelection(false);
            attacker.pieceType.Move(movePosition, attacker.gameObject, gc);
        }
        //attackable piece, attack and move
        else
        {
            gc.Attack(attacked);
            pc.ResetMoveTiles();
            movePiece = false;
            pieceSelected = false;
            selectedPiece.SetSelection(false);
            //Debug.Log(movePosition);
            attacker.pieceType.Move(movePosition, attacker.gameObject, gc);
        }

        turn *= -1;
        turnChange(turn);

        gc.CheckGameStatus();
    }

    private void ChangeView(int view)
    {
        viewChange(view);
        string[] cameraArms = { "CameraArm", "CameraArm1", "CameraArm2" };

        Vector3 newViewAngle = GameObject.Find(cameraArms[view]).transform.position;
        Quaternion rotation = GameObject.Find(cameraArms[view]).transform.rotation;
        Camera.main.transform.position = newViewAngle;
        Camera.main.transform.localRotation = rotation;
    }
}
