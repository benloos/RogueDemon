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
    public bool bossRoom = false;

    public GameObject DoorNegativeX;
    public GameObject DoorNegativeZ;
    public GameObject DoorPositiveX;
    public GameObject DoorPositiveZ;

    public GameObject Zombies;

    public GameObject[] lights;

    private void Start()
    {
        if (Zombies != null)
        {
            Zombies.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (roomID == 0)
        {
            ClearedRoom();
        }
        else if (Zombies == null)
        {
            ClearedRoom();
        }
        else if (!isStarted && !isCleared)
        {
            if (other.CompareTag("Player"))
            {
                isStarted = true;
                GetComponent<BoxCollider>().enabled = false;
                /*if (lights != null)
                {
                    foreach (var light in lights)
                    {
                        light.SetActive(true);
                    }
                }*/
                DoorNegativeX.GetComponent<DoorOpener>().Close();
                DoorNegativeZ.GetComponent<DoorOpener>().Close();
                DoorPositiveX.GetComponent<DoorOpener>().Close();
                DoorPositiveZ.GetComponent<DoorOpener>().Close();
                if (Zombies != null)
                {
                    Zombies.SetActive(true);
                }
            }
        }
    }

    private void Update()
    {
        if (isStarted && !isCleared)
        {
            bool allZombiesDead = true;
            // Check if Zombies are dead
            if (Zombies != null)
            {
                foreach (Transform child in Zombies.transform)
                {
                    if (child.tag == "ZombieP")
                    {
                        if (child.GetComponent<EnemyAI>().health > 0)
                        {
                            allZombiesDead = false;
                        }
                    }
                    else if (child.tag == "Imp")
                    {
                        if (child.GetComponent<EnemyAIRanged>().health > 0)
                        {
                            allZombiesDead = false;
                        }
                    }
                    else if (child.tag == "Boss")
                    {
                        if (child.GetComponent<BossAI>().health > 0)
                        {
                            allZombiesDead = false;
                        }
                    }
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
        if (lights != null)
        {
            foreach (var light in lights)
            {
                light.SetActive(false);
            }
        }
        if (roomID > 0)
        {
            GameManager.current.level_completion.Play();
        }
        if (bossRoom)
        {
            DoorNegativeX.GetComponent<DoorOpener>().Open();
        }
        isCleared = true;
    }

}
