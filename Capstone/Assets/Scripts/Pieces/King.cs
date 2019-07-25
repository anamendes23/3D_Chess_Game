using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class King : Piece
{
    public bool IsFirstMove { get; set; }
    public bool IsInCheck { get; set; }
    public bool IsAttacking { get; set; }
    public bool IsInCheckMate { get; set; }

    public Pair[] possibleChecks = new Pair[9];
    //public int countOfPossibleChecks;
    private int indexInList = -1;

    PieceController _pc;

    public King(int layer) : base("King", layer, 1, true, true, true, true, true, false)
    {
        IsFirstMove = true;
        IsInCheck = false;
        IsAttacking = false;
        IsInCheckMate = false;
        //countOfPossibleChecks = 5;
        InitializePossibleChecks();
    }

    public void InitializePossibleChecks()
    {
        if (Layer == 11) //blue
        {
            possibleChecks[0].X = 3;
            possibleChecks[0].Z = 0;
            possibleChecks[1].X = 5;
            possibleChecks[1].Z = 0;
            possibleChecks[2].X = 3;
            possibleChecks[2].Z = 1;
            possibleChecks[3].X = 4;
            possibleChecks[3].Z = 1;
            possibleChecks[4].X = 5;
            possibleChecks[4].Z = 1;
            possibleChecks[5].X = -1;
            possibleChecks[5].Z = -1;
            possibleChecks[6].X = -1;
            possibleChecks[6].Z = -1;
            possibleChecks[7].X = -1;
            possibleChecks[7].Z = -1;
            possibleChecks[8].X = -1;
            possibleChecks[8].Z = -1;
        }
        else if (Layer == 12) //red
        {
            possibleChecks[0].X = 3;
            possibleChecks[0].Z = 7;
            possibleChecks[1].X = 5;
            possibleChecks[1].Z = 7;
            possibleChecks[2].X = 3;
            possibleChecks[2].Z = 6;
            possibleChecks[3].X = 4;
            possibleChecks[3].Z = 6;
            possibleChecks[4].X = 5;
            possibleChecks[4].Z = 6;
            possibleChecks[5].X = -1;
            possibleChecks[5].Z = -1;
            possibleChecks[6].X = -1;
            possibleChecks[6].Z = -1;
            possibleChecks[7].X = -1;
            possibleChecks[7].Z = -1;
            possibleChecks[8].X = -1;
            possibleChecks[8].Z = -1;
        }
    }

    public override void PlaceTiles(Vector3 tilePosition, GameObject[] tiles, PieceController pc,
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
        Pair[] pairs = new Pair[8];
        //add all 8 possible options for knight
        pairs[0] = new Pair() { X = j, Z = i + 1 };
        pairs[1] = new Pair() { X = j, Z = i - 1 };
        pairs[2] = new Pair() { X = j - 1, Z = i };
        pairs[3] = new Pair() { X = j - 1, Z = i - 1 };
        pairs[4] = new Pair() { X = j - 1, Z = i + 1 };
        pairs[5] = new Pair() { X = j + 1, Z = i };
        pairs[6] = new Pair() { X = j + 1, Z = i - 1 };
        pairs[7] = new Pair() { X = j + 1, Z = i + 1 };
        //check in different directions depending on Piece restrictions
        //knight jumps to move and attack. There are 8 possible positions for it

        for (int k = 0; k < pairs.Length; k++)
        {
            //check first if out of index
            if (pairs[k].X < gridPositions.GetLength(0) && pairs[k].X != -1)
            {
                if (pairs[k].Z < gridPositions.GetLength(1) && pairs[k].Z != -1)
                {
                    //if spot is free, can place movetile
                    if (!IsTherePieceInSpot(gridPositions[pairs[k].Z, pairs[k].X], piecesPositions))
                    {
                        movableSpots.Add(pairs[k]);
                    }
                    //if spot is taken, check if enemy or friend
                    else
                    {
                        string attack = gridPositions[pairs[k].Z, pairs[k].X];
                        int index = piecesPositions.IndexOf(attack);
                        ThirdPersonCharacter attackable = gamePieces[index].GetComponent<ThirdPersonCharacter>();
                        //piece is enemy
                        if (attackable.gameObject.layer != Layer)
                        {
                            attackableSpots.Add(pairs[k]);
                        }
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
        for (int i = 0; i < movableSpots.Count; i++)
        {
            Debug.Log("x: " + movableSpots[i].X + " z: " + movableSpots[i].Z);
            Vector3 newPosition = new Vector3(movableSpots[i].X * 10, tilePosition.y, movableSpots[i].Z * 10);
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
        movableSpots.Clear();
        attackableSpots.Clear();
        CheckSurroundings(selectedPiece.GetComponent<ThirdPersonCharacter>().boardPosition, gc.piecesPositions, gc.grid.Positions, gc.GamePieces);
        //PopulateChecks();
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

        //copy the attackables to list
        for (int i = 0; i < attackableSpots.Count; i++)
        {
            Pair pair = attackableSpots[i];
            string a = gc.grid.Positions[pair.Z, pair.X];
            ethan.attackPositions.Add(a);
        }

        //movableSpots.Clear();
        //attackableSpots.Clear();
    }

    //public void PopulateChecks()
    //{
    //    //clean array
    //    for (int k = 0; k < possibleChecks.Length; k++)
    //    {
    //        possibleChecks[k].X = -1;
    //        possibleChecks[k].Z = -1;
    //    }
    //    countOfPossibleChecks = 0;
    //    int i = 0;
    //    //add possible spots
    //    foreach (Pair p in movableSpots)
    //    {
    //        if (i < possibleChecks.Length)
    //        {
    //            possibleChecks[i] = p;
    //            countOfPossibleChecks++;
    //            i++;
    //        }
    //    }
    //    foreach (Pair p in attackableSpots)
    //    {
    //        if (i < possibleChecks.Length)
    //        {
    //            possibleChecks[i] = p;
    //            countOfPossibleChecks++;
    //            i++;
    //        }
    //    }
    //}
}
