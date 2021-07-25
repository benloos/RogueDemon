using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current { get; private set; }

    public GameObject player;

    public GameObject[] rooms;
    public int roomLength = 60, roomWidth = 30; // Edges at 0,0 and 60,-30
    private List<int[,]> seeds;
    private int seed = 0;
    public AudioSource level_completion;
    [SerializeField] AudioClip roomClear;

    public bool setSpawnPoint = false;
    public Transform SpawnPointObject;
    private Vector3 spawnPoint;

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

    public void openAdjacentDoors(int roomID)
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
        current = this;

        seeds = new List<int[,]>(); // Start Room on the right, Bossroom on the left
        seeds.Add(new int[,] {
            {             -1, -1, -1, -1, -1 },
            {             -1,  7,  5,  3, -1 },
            {             -1, -1,  6,  1,  0 },
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
        for (int i = 0; i < seeds[seed].GetLength(0); i++)
        {
            for (int j = 0; j < seeds[seed].GetLength(1); j++)
            {
                if (seeds[seed][i, j] == 0)
                {
                    rooms[0].GetComponent<RoomManager>().setID(0);
                    rooms[0].transform.position = new Vector3(i * roomLength + 22.5f, 0, j * roomWidth - 15f);
                    spawnPoint = new Vector3(i * roomLength + 22.5f, 0, j * roomWidth - 15f);
                }
                else if (seeds[seed][i, j] == rooms.Length - 1)
                {
                    rooms[rooms.Length - 1].GetComponent<RoomManager>().bossRoom = true;
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
        resetPlayerPos();
        level_completion = GetComponent<AudioSource>();
        level_completion.clip = roomClear;
    }

    public void resetPlayerPos()
    {
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        if (setSpawnPoint)
        {
            player.transform.position = SpawnPointObject.position;
        }
        else
        {
            player.transform.position = spawnPoint + new Vector3(7.5f, 3f, -7.5f);
        }
        cc.enabled = true;
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
