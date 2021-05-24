using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GameObject DoorNegativeX;
    [SerializeField] private GameObject DoorNegativeZ;
    [SerializeField] private GameObject DoorPositiveX;
    [SerializeField] private GameObject DoorPositiveZ;

    private void Start()
    {
        // DoorNegativeX.GetComponent<DoorOpener>().Open();
        
    }

}
