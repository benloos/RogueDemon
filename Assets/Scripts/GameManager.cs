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
            {  1,  4,  2 }
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
                rooms[roomID].GetComponent<RoomManager>().DoorPositiveX.GetComponent<DoorOpener>().Open();
                rooms[seed[roomCoords.X - 1, roomCoords.Y]].GetComponent<RoomManager>().DoorNegativeX.GetComponent<DoorOpener>().Open();
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
                rooms[roomID].GetComponent<RoomManager>().DoorPositiveZ.GetComponent<DoorOpener>().Open();
                rooms[seed[roomCoords.X, roomCoords.Y - 1]].GetComponent<RoomManager>().DoorNegativeZ.GetComponent<DoorOpener>().Open();
            }
        }
    }

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

    private void Start()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].GetComponent<RoomManager>().setID(i);
        }
    }

    public void StartRoom(int id)
    {
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
