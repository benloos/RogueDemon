using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    private GameObject player;

    [SerializeField] private GameObject[] rooms;
    private int roomLength = 60, roomWidth = 30; // Edges at 0,0 and 60,-30
    private int[,] seed = new int[,] { // Start in Right Column, Boss in Left Column
            { -1, -1, -1, -1, -1 },
            { -1,  7,  1,  3, -1 },
            { -1, -1,  6,  5,  0 },
            { -1,  8,  4,  2, -1 },
            { -1, -1, -1, -1, -1 }
        };

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
        for (int i = 0; i < seed.GetLength(0); i++)
        {
            for (int j = 0; j < seed.GetLength(1); j++)
            {
                if (seed[i, j] == roomID)
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

        if (roomCoords.X + 1 < seed.GetLength(0))
        {
            if (seed[roomCoords.X + 1, roomCoords.Y] > -1)
            {
                rooms[roomID].GetComponent<RoomManager>().DoorPositiveX.GetComponent<DoorOpener>().Open();
                rooms[seed[roomCoords.X + 1, roomCoords.Y]].GetComponent<RoomManager>().DoorNegativeX.GetComponent<DoorOpener>().Open();
            }
        }
        if (roomCoords.X - 1 > -1)
        {
            if (seed[roomCoords.X - 1, roomCoords.Y] > -1)
            {
                rooms[roomID].GetComponent<RoomManager>().DoorNegativeX.GetComponent<DoorOpener>().Open();
                rooms[seed[roomCoords.X - 1, roomCoords.Y]].GetComponent<RoomManager>().DoorPositiveX.GetComponent<DoorOpener>().Open();
            }
        }
        if (roomCoords.Y + 1 < seed.GetLength(1))
        {
            if (seed[roomCoords.X, roomCoords.Y + 1] > -1)
            {
                rooms[roomID].GetComponent<RoomManager>().DoorPositiveZ.GetComponent<DoorOpener>().Open();
                rooms[seed[roomCoords.X, roomCoords.Y + 1]].GetComponent<RoomManager>().DoorNegativeZ.GetComponent<DoorOpener>().Open();
            }
        }
        if (roomCoords.Y - 1 > -1)
        {
            if (seed[roomCoords.X, roomCoords.Y - 1] > -1)
            {
                rooms[roomID].GetComponent<RoomManager>().DoorNegativeZ.GetComponent<DoorOpener>().Open();
                rooms[seed[roomCoords.X, roomCoords.Y - 1]].GetComponent<RoomManager>().DoorPositiveZ.GetComponent<DoorOpener>().Open();
            }
        }
    }

    private void Awake()
    {
        current = this;
        player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < seed.GetLength(0); i++)
        {
            for (int j = 0; j < seed.GetLength(1); j++)
            {
                if (seed[i, j] == 0)
                {
                    rooms[0].GetComponent<RoomManager>().setID(0);
                    rooms[0].transform.position = new Vector3(i * roomLength + 22.5f, 0, j * roomWidth - 15f);
                    player.transform.position = new Vector3(rooms[0].transform.position.x + 7.5f, 3f, rooms[0].transform.position.z - 7.5f);
                }
                else if (seed[i, j] > 0)
                {
                    rooms[seed[i, j]].GetComponent<RoomManager>().setID(seed[i, j]);
                    rooms[seed[i, j]].transform.position = new Vector3(i * roomLength, 0, j * roomWidth);
                }
            }
        }
    }

    private void Start()
    {
        
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
}
