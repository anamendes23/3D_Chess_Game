using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public abstract class Piece
{
    public List<Pair> movableSpots = new List<Pair>();
    public List<Pair> attackableSpots = new List<Pair>();

    public string Tag { get; set; }
    public int Layer { get; set; }
    public int MaxMoves { get; set; }
    public bool MovesForward { get; set; }
    public bool MovesBackwards { get; set; }
    public bool MovesLeft { get; set; }
    public bool MovesRight { get; set; }
    public bool MovesDiagonal { get; set; }
    public bool IsZombie { get; set; }

    protected Piece(string tag, int layer, int maxMoves, bool movesForward, bool movesBackwards,
        bool movesLeft, bool movesRight, bool movesDiagonal, bool isZombie)
    {
        Tag = tag;
        Layer = layer;
        MaxMoves = maxMoves;
        MovesForward = movesForward;
        MovesBackwards = movesBackwards;
        MovesLeft = movesLeft;
        MovesRight = movesRight;
        MovesDiagonal = movesDiagonal;
        IsZombie = isZombie;

        Physics.IgnoreLayerCollision(11, 13);
        Physics.IgnoreLayerCollision(12, 13);
    }

    public abstract void PlaceTiles(
        Vector3 tilePosition, GameObject[] tiles, PieceController pc,
        ThirdPersonCharacter selectedPiece, GameController gc, Grid grid);
    public abstract void CheckSurroundings(string position, List<string> piecesPositions, string[,] gridPositions,
        Transform[] gamePieces); 
    public abstract void Move(Vector3 movePosition, GameObject go, GameController gc);
}

public struct Pair
{
    //in code, X axis is represented by j in the matrix
    public int X { get; set; }
    //in code, z axis is represented by i in the matrix
    public int Z { get; set; }
}
