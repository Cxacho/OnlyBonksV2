using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirections;
    // 1 -> bot door
    // 2 -> top door
    // 3 -> left door
    // 4 -> right door

    private RoomManager roomManager;
    private int rand;
    private bool spawned = false;


    public float waitTime = 4f;

    private void Awake()
    {
        Destroy(gameObject, waitTime);
        roomManager = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomManager>();
        Invoke("Spawn", 0.5f);
    }

    // lewo = 4 prawo = 3 dó³ = 2 góra = 1
    private void Spawn()
    {
        if(spawned == false && roomManager.roomCap >= roomManager.pokoje)
        {
            if (openingDirections == 1)
            {
                // Needs room with Bottom door
                rand = Random.Range(0, roomManager.bottomRooms.Length);
                Instantiate(roomManager.bottomRooms[rand], transform.position, Quaternion.identity);
                roomManager.pokoje++;
            }
            else if (openingDirections == 2)
            {
                // Needs room with Top door
                rand = Random.Range(0, roomManager.topRooms.Length);
                Instantiate(roomManager.topRooms[rand], transform.position, Quaternion.identity);
                roomManager.pokoje++;
            }
            else if (openingDirections == 3)
            {
                // Needs room with Left door
                rand = Random.Range(0, roomManager.leftRooms.Length);
                Instantiate(roomManager.leftRooms[rand], transform.position, Quaternion.identity);
                roomManager.pokoje++;
            }
            else if (openingDirections == 4)
            {
                // Needs room with Right door
                rand = Random.Range(0, roomManager.rightRooms.Length);
                Instantiate(roomManager.rightRooms[rand], transform.position, Quaternion.identity);
                roomManager.pokoje++;
            }
            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("SpawnPoint"))
        {
            if(collision.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                roomManager = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomManager>();
                Instantiate(roomManager.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
