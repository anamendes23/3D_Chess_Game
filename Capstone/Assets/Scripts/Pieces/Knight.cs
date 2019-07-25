using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Knight : Piece
{
    private int indexInList = -1;

    PieceController _pc;

    public Knight(int layer) : base("Knight", layer, 3, true, true, true, true, false, true) { }

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
        int L = gridPositions.GetLength(0);
        Pair[] pairs = new Pair[8];
        //add all 8 possible options for knight
        pairs[0] = new Pair() { X = j - 2, Z = i + 1 };
        pairs[1] = new Pair() { X = j - 1, Z = i + 2 };
        pairs[2] = new Pair() { X = j - 2, Z = i - 1 };
        pairs[3] = new Pair() { X = j - 1, Z = i - 2 };
        pairs[4] = new Pair() { X = j + 2, Z = i + 1 };
        pairs[5] = new Pair() { X = j + 1, Z = i + 2 };
        pairs[6] = new Pair() { X = j + 2, Z = i - 1 };
        pairs[7] = new Pair() { X = j + 1, Z = i - 2 };
        //check in different directions depending on Piece restrictions
        //knight jumps to move and attack. There are 8 possible positions for it

        for (int k = 0; k < pairs.Length; k++)
        {
            //check first if out of index
            if (pairs[k].X < gridPositions.GetLength(0) && pairs[k].X > -1)
            {
                if (pairs[k].Z < gridPositions.GetLength(1) && pairs[k].Z > -1)
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
            Vector3 newPosition = new Vector3(movableSpots[i].X * 10, tilePosition.y, movableSpots[i].Z * 10);
            GameObject go = GameObject.Instantiate(moveTile, newPosition, Quaternion.identity);
            _pc.moveTiles.Add(go);
        }
    }

    public void AttackTiles(Vector3 tilePosition, GameObject attackTile)
    {
        for (int i = 0; i < attackableSpots.Count; i++)
        {
            Vector3 newPosition = new Vector3(attackableSpots[i].X * 10, tilePosition.y, attackableSpots[i].Z * 10);
            GameObject go = GameObject.Instantiate(attackTile, newPosition, Quaternion.identity);
            _pc.attackTiles.Add(go);
        }
    }

    public override void Move(Vector3 movePosition, GameObject selectedPiece, GameController gc)
    {
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

        //CheckSurroundings(ethan.boardPosition, gc.piecesPositions, gc.grid.Positions, gc.GamePieces);
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
}
