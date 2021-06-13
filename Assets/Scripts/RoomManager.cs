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

    private void OnTriggerEnter(Collider other)
    {
        if (roomID == 0)
        {
            GameManager.current.ClearedRoom(0);
        }
        else if (!isStarted && !isCleared)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.current.StartRoom(roomID);
                StartCoroutine(delayedClear());
            }
        }
    }

    private IEnumerator delayedClear()
    {
        yield return new WaitForSeconds(5);
        GameManager.current.ClearedRoom(roomID);
    }
}
