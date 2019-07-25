using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;


public class AiController : MonoBehaviour
{
    private GameController gc;
    private SelectorController sc;
    [SerializeField] int delay = 8;

    private void Awake()
    {
        gc = FindObjectOfType<GameController>();
        sc = FindObjectOfType<SelectorController>();
        sc.turnChange += TurnChange;
    }

    void TurnChange(int turn)
    {
        Debug.Log("Turn = " + turn);
        if (turn == 1)
        {
            int newDelay = Random.Range(3, delay);
            WaitForSecondsRealtime wait = new WaitForSecondsRealtime(newDelay);
            StartCoroutine(TimeDelay(wait));
        }

    }

    private void MakeMovesSon()
    {
        if (sc.turn == 1)
        {
        here:

            int pickOne = Random.Range(gc.firstRed, gc.lastRed - 1);

            ThirdPersonCharacter piece = gc.GamePieces[pickOne].GetComponent<ThirdPersonCharacter>();

            Piece ptype = piece.pieceType;

            ptype.CheckSurroundings(piece.boardPosition, gc.piecesPositions, gc.grid.Positions, gc.GamePieces);

            if (ptype.attackableSpots.Count > 0)
            {
                pickOne = Random.Range(0, ptype.attackableSpots.Count - 1);

                //get the grid position based on the Pair
                Pair p = ptype.attackableSpots[pickOne];
                string gridPosition = gc.grid.Positions[p.Z, p.X];

                //loop through reds and find piece with boardPosition == grid position
                GameObject attacked = new GameObject();
                for (int i = gc.firstBlue; i < gc.lastBlue; i++)
                {
                    if (gc.piecesPositions[i] == gridPosition)
                    {
                        attacked = gc.GamePieces[i].gameObject;
                        break;
                    }
                }
                //call Attack method from SC for that piece
                sc.Move_Attack(piece, attacked, new Vector3(p.X * 10, 0.5f, p.Z * 10));
            }
            //else if for movables > 0
            else if (ptype.movableSpots.Count > 0)
            {
                //pickOne random for movables and just move there
                pickOne = Random.Range(0, ptype.movableSpots.Count - 1);

                //get the grid position based on the Pair
                Pair p = ptype.movableSpots[pickOne];
                string gridPosition = gc.grid.Positions[p.Z, p.X];

                //call Attack method from SC for that piece
                sc.Move_Attack(piece, null, new Vector3(p.X * 10, 0.5f, p.Z * 10));                
            }
            //else for both empty
            else
            {
                goto here;
            }
        }
    }

    private IEnumerator TimeDelay(WaitForSecondsRealtime wait)
    {
        wait.Reset();
        yield return wait;
        //Debug.Log("Waited for: " + wait.waitTime);
        MakeMovesSon();
    }
}
