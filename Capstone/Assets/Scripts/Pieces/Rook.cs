using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Rook : Piece
{
    bool IsFirstMove { get; set; }

    private int indexInList = -1;

    PieceController _pc;

    public Rook(int layer) : base("Rook", layer, 8, true, true, true, true, false, true)
    {
        IsFirstMove = true;
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
        int L = gridPositions.GetLength(0);
        //check in different directions depending on Piece restrictions
        //bishop moves and attacks diagonally. There are 4 diagonals around the piece
        int stopPoint = j + 1;
        //check up
        //starts with 1 to check point in front of piece
        for (int k = 1; k < stopPoint; k++)
        {
            //if spot is free, can place movetile
            if (!IsTherePieceInSpot(gridPositions[i, j - k], piecesPositions))
            {
                Pair pair = new Pair();
                pair.X = j - k;
                pair.Z = i;
                movableSpots.Add(pair);
            }
            //if spot is taken, check if enemy or friend
            else
            {
                string attack = gridPositions[i, j - k];
                int index = piecesPositions.IndexOf(attack);
                ThirdPersonCharacter attackable = gamePieces[index].GetComponent<ThirdPersonCharacter>();
                //piece is enemy
                if (attackable.gameObject.layer != Layer)
                {
                    Pair pair = new Pair();
                    pair.X = j - k;
                    pair.Z = i;
                    attackableSpots.Add(pair);
                    break;
                }
                //piece is friend, end loop
                else
                    break;
            }
        }

        stopPoint = L - j;
        //check down
        for (int k = 1; k < stopPoint; k++)
        {
            if (!IsTherePieceInSpot(gridPositions[i, j + k], piecesPositions))
            {
                Pair pair = new Pair();
                pair.X = j + k;
                pair.Z = i;
                movableSpots.Add(pair);
            }
            //if spot is taken, check if enemy or friend
            else
            {
                string attack = gridPositions[i, j + k];
                int index = piecesPositions.IndexOf(attack);
                ThirdPersonCharacter attackable = gamePieces[index].GetComponent<ThirdPersonCharacter>();
                //piece is enemy
                if (attackable.gameObject.layer != Layer)
                {
                    Pair pair = new Pair();
                    pair.X = j + k;
                    pair.Z = i;
                    attackableSpots.Add(pair);
                    break;
                }
                //piece is friend, end loop
                else
                    break;
            }
        }


        stopPoint = i + 1;
        //check left
        //starts with 1 to check point in front of piece
        for (int k = 1; k < stopPoint; k++)
        {

            if (!IsTherePieceInSpot(gridPositions[i - k, j], piecesPositions))
            {
                Pair pair = new Pair();
                pair.X = j;
                pair.Z = i - k;
                movableSpots.Add(pair);
            }
            //if spot is taken, check if enemy or friend
            else
            {
                string attack = gridPositions[i - k, j];
                int index = piecesPositions.IndexOf(attack);
                ThirdPersonCharacter attackable = gamePieces[index].GetComponent<ThirdPersonCharacter>();
                //piece is enemy
                if (attackable.gameObject.layer != Layer)
                {
                    Pair pair = new Pair();
                    pair.X = j;
                    pair.Z = i - k;
                    attackableSpots.Add(pair);
                    break;
                }
                //piece is friend, end loop
                else
                    break;
            }

        }

        stopPoint = L - i;
        //check right
        for (int k = 1; k < stopPoint; k++)
        {
            if (!IsTherePieceInSpot(gridPositions[i + k, j], piecesPositions))
            {
                Pair pair = new Pair();
                pair.X = j;
                pair.Z = i + k;
                movableSpots.Add(pair);
            }
            //if spot is taken, check if enemy or friend
            else
            {
                string attack = gridPositions[i + k, j];
                int index = piecesPositions.IndexOf(attack);
                ThirdPersonCharacter attackable = gamePieces[index].GetComponent<ThirdPersonCharacter>();
                //piece is enemy
                if (attackable.gameObject.layer != Layer)
                {
                    Pair pair = new Pair();
                    pair.X = j;
                    pair.Z = i + k;
                    attackableSpots.Add(pair);
                    break;
                }
                //piece is friend, end loop
                else
                    break;
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
        if (IsFirstMove)
            IsFirstMove = false;

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
