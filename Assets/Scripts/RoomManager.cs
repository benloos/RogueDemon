using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private bool roomCleared = false;
    public bool isCleared() { return roomCleared; }

    private bool gameIsRunning = false;

    [SerializeField] private GameObject DoorNegativeX;
    [SerializeField] private GameObject DoorNegativeZ;
    [SerializeField] private GameObject DoorPositiveX;
    [SerializeField] private GameObject DoorPositiveZ;

    private void Start()
    {
        DoorNegativeX.GetComponent<DoorOpener>().Open();
        DoorNegativeZ.GetComponent<DoorOpener>().Open();
        DoorPositiveX.GetComponent<DoorOpener>().Open();
        DoorPositiveZ.GetComponent<DoorOpener>().Open();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCleared())
        {
            if (other.CompareTag("Player"))
            {
                DoorNegativeX.GetComponent<DoorOpener>().Close();
                DoorNegativeZ.GetComponent<DoorOpener>().Close();
                DoorPositiveX.GetComponent<DoorOpener>().Close();
                DoorPositiveZ.GetComponent<DoorOpener>().Close();

                gameIsRunning = true;
            }
        }
    }
}
