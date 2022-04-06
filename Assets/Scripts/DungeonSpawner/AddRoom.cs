using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    private RoomManager roomManager;

    private void Start()
    {
        roomManager = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomManager>();
        roomManager.rooms.Add(this.gameObject);
    }
}
