using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Pawn : Piece
{
    bool IsFirstMove { get; set; }
    bool IsAttacking { get; set; }

    private int indexInList = -1;

    PieceController _pc;

    public Pawn(int layer) :
        base("Pawn", layer, 2, true, false, false, false, false, false)
    {
        IsFirstMove = true;
        IsAttacking = false;
    }

    public override void PlaceTiles(
        Vector3 tilePosition, GameObject[] tiles, PieceController pc,
        ThirdPersonCharacter selectedPiece, GameController gc, Grid grid)
    {
        _pc = pc;
        movableSpots.Clear();
        attackableSpots.Clear();

        CheckSurroundings(selectedPiece.boardPosition, gc.piecesPositions, grid.Positions, gc.GamePieces);
        MoveTiles(tilePosition, tiles[0]);
        AttackTiles(tilePosition, tiles[1]);        
    }

    //method used to populate lists of attackable and movable spots in the board
    public override void CheckSurroundings(string position, List<string> piecesPositions, string[,] gridPositions, Transform[] gamePieces)
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

        indexInList = piecesPositions.IndexOf(gridPositions[i, j]);

        //variable to define if looping in the positive or negative direction on the grid
        int offSet = 0;

        if (Layer == 11)
            offSet = 1; //when checking forward, is going to add one in loop
        else if (Layer == 12)
            offSet = -1; //when checking forward, is going to add subtract in loop

        //check in different directions depending on Piece restrictions
        //pawn moves forward and attacks diagonally
        //check movables
        int diffOnGrid = 0;
        if (Layer == 11)
            diffOnGrid = gridPositions.GetLength(0) - (i + 1);
        else if (Layer == 12)
            diffOnGrid = i;

        int stopPoint = Mathf.Min(MaxMoves, diffOnGrid);
        for (int k = 1; k < stopPoint + 1; k++)
        {
            if (!IsTherePieceInSpot(gridPositions[i + offSet * k, j], piecesPositions))
            {
                Pair pair = new Pair();
                pair.X = j;
                pair.Z = i + offSet * k;
                movableSpots.Add(pair);
            }
        }
        //check attackables
        //special case for Pawn
        if (i + offSet < gridPositions.GetLength(0) && i + offSet != -1)
        {
            if (j + offSet < gridPositions.GetLength(1) && j + offSet != -1)
            {
                string attack = gridPositions[i + offSet, j + offSet];
                if (IsTherePieceInSpot(attack, piecesPositions))
                {
                    int index = piecesPositions.IndexOf(attack);
                    ThirdPersonCharacter attackable = gamePieces[index].GetComponent<ThirdPersonCharacter>();
                    if (attackable.gameObject.layer != Layer)
                    {
                        Pair pair = new Pair();
                        pair.X = j + offSet;
                        pair.Z = i + offSet;
                        attackableSpots.Add(pair);
                    }
                }
            }
        }

        if (i + offSet < gridPositions.GetLength(0) && i + offSet != -1)
        {
            if (j - offSet < gridPositions.GetLength(1) && j - offSet != -1)
            {
                string attack = gridPositions[i + offSet, j - offSet];
                if (IsTherePieceInSpot(attack, piecesPositions))
                {
                    int index = piecesPositions.IndexOf(attack);
                    ThirdPersonCharacter attackable = gamePieces[index].GetComponent<ThirdPersonCharacter>();
                    if (attackable.gameObject.layer != Layer)
                    {
                        Pair pair = new Pair();
                        pair.X = j - offSet;
                        pair.Z = i + offSet;
                        attackableSpots.Add(pair);
                    }
                }
            }
        }
    }

    public bool IsTherePieceInSpot(string position, List<string> piecesPositions)
    {
        if (piecesPositions.Contains(position))
            return true;
        else
            return false;
    }

    public void MoveTiles(Vector3 tilePosition, GameObject moveTile)
    {
        //setting the first positing in front of the character
        //tilePosition.z -= zOffsetFactor;
        //pawn only moves forward, so only one loop to place tiles
        for (int i = 0; i < movableSpots.Count; i++)
        {
            Debug.Log("x: " + movableSpots[i].X + " z: " + movableSpots[i].Z);
            Vector3 newPosition = new Vector3(tilePosition.x, tilePosition.y, movableSpots[i].Z * 10);
            GameObject go = GameObject.Instantiate(moveTile, newPosition, Quaternion.identity);
            _pc.moveTiles.Add(go);
        }
    }

    public void AttackTiles(Vector3 tilePosition, GameObject attackTile)
    {
        for (int i = 0; i < attackableSpots.Count; i++)
        {
            //Debug.Log("x: " + attackableSpots[i].X + " z: " + attackableSpots[i].Z);
            Vector3 newPosition = new Vector3(attackableSpots[i].X * 10, tilePosition.y, attackableSpots[i].Z * 10);
            GameObject go = GameObject.Instantiate(attackTile, newPosition, Quaternion.identity);
            _pc.attackTiles.Add(go);
        }
    }

    public override void Move(Vector3 movePosition, GameObject selectedPiece, GameController gc)
    {
        if (IsFirstMove)
        {
            IsFirstMove = false;
            MaxMoves = 1;
        }

        selectedPiece.transform.position = movePosition;
        UpdatePosition(movePosition, selectedPiece, gc);
        UpdateAttacks(selectedPiece, gc);
    }

    public void UpdatePosition(Vector3 movePosition, GameObject selectedPiece, GameController gc)
    {
        ThirdPersonCharacter ethan = selectedPiece.GetComponent<ThirdPersonCharacter>();

        string[,] positions = gc.grid.Positions;
        int i = Mathf.Abs(Mathf.RoundToInt(movePosition.x / gc.grid.Size));
        int j = Mathf.Abs(Mathf.RoundToInt(movePosition.z / gc.grid.Size));
        ethan.boardPosition = positions[j, i];

        gc.piecesPositions[indexInList] = ethan.boardPosition;
    }

    public void UpdateAttacks(GameObject selectedPiece, GameController gc)
    {
        ThirdPersonCharacter ethan = selectedPiece.GetComponent<ThirdPersonCharacter>();

        //clear out lists
        ethan.attackPositions.Clear();

        CheckSurroundings(ethan.boardPosition, gc.piecesPositions, gc.grid.Positions, gc.GamePieces);
        //copy the attackables to list
        for (int i = 0; i < attackableSpots.Count; i++)
        {
            Pair pair = attackableSpots[i];
            string a = gc.grid.Positions[pair.Z, pair.X];
            ethan.attackPositions.Add(a);
        }

        for (int i = 0; i < movableSpots.Count; i++)
        {
            Pair pair = movableSpots[i];
            string a = gc.grid.Positions[pair.Z, pair.X];
            ethan.futureMovePositions.Add(a);
        }

        movableSpots.Clear();
        attackableSpots.Clear();
    }
}
