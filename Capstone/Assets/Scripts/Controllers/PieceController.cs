using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class PieceController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tiles;

    public List<GameObject> moveTiles = new List<GameObject>();
    public List<GameObject> attackTiles = new List<GameObject>();    

    GameController gc;
    Grid grid;
    PieceController pc;

    private void Awake()
    {
        gc = FindObjectOfType<GameController>();
        grid = FindObjectOfType<Grid>();
        pc = this;
    }

    public void ResetMoveTiles()
    {
        foreach (GameObject go in moveTiles)
        {
            Destroy(go);
        }

        foreach (GameObject go in attackTiles)
        {
            Destroy(go);
        }

        moveTiles.Clear();
        attackTiles.Clear();
    }

    public void PlaceMoveTile(Vector3 position, ThirdPersonCharacter selectedPiece)
    {
        ResetMoveTiles();

        Piece piece = selectedPiece.pieceType;

        piece.PlaceTiles(position, tiles, pc, selectedPiece, gc, grid);
    }
}