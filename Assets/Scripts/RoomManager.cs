using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private int roomID;
    public int getID() { return roomID; }
    public void setID(int id) { roomID = id; }

    public bool isCleared = false;
    public bool isStarted = false;

    public GameObject DoorNegativeX;
    public GameObject DoorNegativeZ;
    public GameObject DoorPositiveX;
    public GameObject DoorPositiveZ;

    public GameObject Zombies;

    private void Start()
    {
        Zombies.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (roomID == 0)
        {
            ClearedRoom();
        }
        else if (!isStarted && !isCleared)
        {
            if (other.CompareTag("Player"))
            {
                isStarted = true;
                DoorNegativeX.GetComponent<DoorOpener>().Close();
                DoorNegativeZ.GetComponent<DoorOpener>().Close();
                DoorPositiveX.GetComponent<DoorOpener>().Close();
                DoorPositiveZ.GetComponent<DoorOpener>().Close();
                Zombies.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (isStarted && !isCleared)
        {
            bool allZombiesDead = true;
            // Check if Zombies are dead
            foreach (Transform child in Zombies.transform)
            {
                if (child.GetComponent<EnemyAI>().isActive)
                {
                    allZombiesDead = false;
                }
            }
            if (allZombiesDead)
            {
                ClearedRoom();
            }
        }
    }

    public void ClearedRoom()
    {
        GameManager.current.openAdjacentDoors(roomID);
        isCleared = true;
    }

}
