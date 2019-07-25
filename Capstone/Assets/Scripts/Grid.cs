using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private float size = 10f;
    private string[,] positions = new string[8, 8];

    public string[,] Positions { get { return positions; } }
    public float Size { get { return size; } }

    private void Awake()
    {
        InitializePositions();
    }

    private void InitializePositions()
    {
        char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
        char[] numbers = { '1', '2', '3', '4', '5', '6', '7', '8' };

        for (int i = 0; i < positions.GetLength(0); i++)
        {
            for (int j = 0; j < positions.GetLength(1); j++)
            {
                positions[i, j] = "" + numbers[i] + letters[j];
            }
        }
    }

    public Vector3 GridPoint(Vector3 position)
    {
        //position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3((float)xCount * size, (float)yCount * size, (float)zCount * size);

        result += transform.position;

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (float x = 0; x < 80; x += size)
        {
            for (float z = 0; z < 80; z += size)
            {
                var point = GridPoint(new Vector3(x, 0f, z));
                Gizmos.DrawSphere(point, 0.3f);                
            }
        }
    }
}
