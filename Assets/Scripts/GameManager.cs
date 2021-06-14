using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    public GameObject player;

    [SerializeField] private GameObject[] rooms;
    private int roomLength = 60, roomWidth = 30; // Edges at 0,0 and 60,-30
    private List<int[,]> seeds;
    private int seed = 0;

    public struct Coords
    {
        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
    }

    private Coords findIndexOfRoom(int roomID)
    {
        for (int i = 0; i < seeds[seed].GetLength(0); i++)
        {
            for (int j = 0; j < seeds[seed].GetLength(1); j++)
            {
                if (seeds[seed][i, j] == roomID)
                {
                    return new Coords(i, j);
                }
            }
        }
        return new Coords(-1, -1);
    }

    void openAdjacentDoors(int roomID)
    {
        Coords roomCoords = findIndexOfRoom(roomID);

        if (roomCoords.X + 1 < seeds[seed].GetLength(0))
        {
            if (seeds[seed][roomCoords.X + 1, roomCoords.Y] > -1)
            {
                rooms[roomID].GetComponent<RoomManager>().DoorPositiveX.GetComponent<DoorOpener>().Open();
                rooms[seeds[seed][roomCoords.X + 1, roomCoords.Y]].GetComponent<RoomManager>().DoorNegativeX.GetComponent<DoorOpener>().Open();
            }
        }
        if (roomCoords.X - 1 > -1)
        {
            if (seeds[seed][roomCoords.X - 1, roomCoords.Y] > -1)
            {
                rooms[roomID].GetComponent<RoomManager>().DoorNegativeX.GetComponent<DoorOpener>().Open();
                rooms[seeds[seed][roomCoords.X - 1, roomCoords.Y]].GetComponent<RoomManager>().DoorPositiveX.GetComponent<DoorOpener>().Open();
            }
        }
        if (roomCoords.Y + 1 < seeds[seed].GetLength(1))
        {
            if (seeds[seed][roomCoords.X, roomCoords.Y + 1] > -1)
            {
                rooms[roomID].GetComponent<RoomManager>().DoorPositiveZ.GetComponent<DoorOpener>().Open();
                rooms[seeds[seed][roomCoords.X, roomCoords.Y + 1]].GetComponent<RoomManager>().DoorNegativeZ.GetComponent<DoorOpener>().Open();
            }
        }
        if (roomCoords.Y - 1 > -1)
        {
            if (seeds[seed][roomCoords.X, roomCoords.Y - 1] > -1)
            {
                rooms[roomID].GetComponent<RoomManager>().DoorNegativeZ.GetComponent<DoorOpener>().Open();
                rooms[seeds[seed][roomCoords.X, roomCoords.Y - 1]].GetComponent<RoomManager>().DoorPositiveZ.GetComponent<DoorOpener>().Open();
            }
        }
    }

    private void Awake()
    {
        seeds = new List<int[,]>(); // Start Room on the right, Bossroom on the left
        seeds.Add(new int[,] {
            {             -1, -1, -1, -1, -1 },
            {             -1,  7,  1,  3, -1 },
            {             -1, -1,  6,  5,  0 },
            { rooms.Length-1,  8,  4,  2, -1 },
            {             -1, -1, -1, -1, -1 }
        });
        seeds.Add(new int[,] {
            {             -1,  6, -1, -1, -1 },
            { rooms.Length-1,  7,  1,  3, -1 },
            {             -1, -1, -1,  5, -1 },
            {             -1,  8,  4,  2,  0 },
            {             -1, -1, -1, -1, -1 }
        });
        seeds.Add(new int[,] {
            {             -1,  6, -1, -1, -1 },
            {             -1,  7,  1,  3, -1 },
            {             -1, -1, -1,  5, -1 },
            { rooms.Length-1,  8,  4,  2,  0 },
            {             -1, -1, -1, -1, -1 }
        });
        seeds.Add(new int[,] {
            {             -1, -1, -1, -1, -1 },
            {             -1,  7,  1,  3,  0 },
            { rooms.Length-1,  6, -1,  5, -1 },
            {             -1,  8,  4,  2, -1 },
            {             -1, -1, -1, -1, -1 }
        });
        seed = Random.Range(0, seeds.Count);
        Debug.Log("Current seed: " + seed);
        current = this;
        player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < seeds[seed].GetLength(0); i++)
        {
            for (int j = 0; j < seeds[seed].GetLength(1); j++)
            {
                if (seeds[seed][i, j] == 0)
                {
                    rooms[0].GetComponent<RoomManager>().setID(0);
                    rooms[0].transform.position = new Vector3(i * roomLength + 22.5f, 0, j * roomWidth - 15f);
                }
                else if (seeds[seed][i, j] == rooms.Length - 1)
                {
                    rooms[rooms.Length-1].GetComponent<RoomManager>().setID(rooms.Length - 1);
                    rooms[rooms.Length-1].transform.position = new Vector3(i * roomLength + 8f, 0, j * roomWidth);
                }
                else if (seeds[seed][i, j] > 0)
                {
                    rooms[seeds[seed][i, j]].GetComponent<RoomManager>().setID(seeds[seed][i, j]);
                    rooms[seeds[seed][i, j]].transform.position = new Vector3(i * roomLength, 0, j * roomWidth);
                }
            }
        }
    }

    private void Start()
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.enabled = false;
        resetPlayerPos();
        pc.enabled = true;
    }

    public void StartRoom(int id)
    {
        rooms[id].GetComponent<RoomManager>().isStarted = true;
        rooms[id].GetComponent<RoomManager>().DoorNegativeX.GetComponent<DoorOpener>().Close();
        rooms[id].GetComponent<RoomManager>().DoorNegativeZ.GetComponent<DoorOpener>().Close();
        rooms[id].GetComponent<RoomManager>().DoorPositiveX.GetComponent<DoorOpener>().Close();
        rooms[id].GetComponent<RoomManager>().DoorPositiveZ.GetComponent<DoorOpener>().Close();
    }

    public void ClearedRoom(int id)
    {
        openAdjacentDoors(id);
        rooms[id].GetComponent<RoomManager>().isCleared = true;
    }

    public void resetPlayerPos()
    {
        player.transform.position = new Vector3(rooms[0].transform.position.x + 7.5f, 3f, rooms[0].transform.position.z - 7.5f);
    }
}
