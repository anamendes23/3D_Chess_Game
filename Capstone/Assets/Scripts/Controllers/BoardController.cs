using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public List<GameObject> piecesPrefabs;
    public List<GameObject> activePieces;
    private Grid grid;

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        SpawnAllPieces();
    }

    private void SpawnPiece(int index, Vector3 position)
    {
        GameObject go = Instantiate(piecesPrefabs[index], position, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        activePieces.Add(go);
    }

    private void SpawnAllPieces()
    {
        activePieces = new List<GameObject>();

        //Blue Team
        //King
        SpawnPiece(0, GetCenter(3, 0));

        //Queen
        SpawnPiece(1, GetCenter(4, 0));

        //Bishops
        SpawnPiece(2, GetCenter(5, 0));
        SpawnPiece(2, GetCenter(2, 0));

        //Knights
        SpawnPiece(3, GetCenter(6, 0));
        SpawnPiece(3, GetCenter(1, 0));

        //Rooks
        SpawnPiece(4, GetCenter(7, 0));
        SpawnPiece(4, GetCenter(0, 0));

        //Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnPiece(5, GetCenter(i, 1));
        }

        //Red Team
        //King
        SpawnPiece(6, GetCenter(3, 7));

        //Queen
        SpawnPiece(7, GetCenter(4, 7));

        //Bishops
        SpawnPiece(8, GetCenter(5, 7));
        SpawnPiece(8, GetCenter(2, 7));

        //Knights
        SpawnPiece(9, GetCenter(6, 7));
        SpawnPiece(9, GetCenter(1, 7));

        //Rooks
        SpawnPiece(10, GetCenter(7, 7));
        SpawnPiece(10, GetCenter(0, 7));

        //Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnPiece(11, GetCenter(i, 6));
        }
    }

    private Vector3 GetCenter(int x, int y)
    {
        Vector3 position = Vector3.zero;
        position.x += (grid.Size * x) - 35;
        position.z += (grid.Size * y) - 35;
        position.y = .5f;
        return position;
    }
}
