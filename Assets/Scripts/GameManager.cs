using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    [SerializeField] private GameObject[] rooms;
    private int roomLength = 60, roomWidth = 30; // Edges at 0,0 and 60,-30
    private int[,] seed = new int[,] {
            { -1,  0,  3 },
            { -1, -1,  5 },
            {  1,  4,  2}
        };

private void Awake()
    {
        current = this;
        for (int i = 0; i < seed.GetLength(0); i++)
        {
            for (int j = 0; j < seed.GetLength(1); j++)
            {
                if (seed[i,j] >= 0)
                {
                    rooms[seed[i, j]].transform.position = new Vector3(i * roomLength, 0, j * roomWidth - roomWidth);
                }
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
