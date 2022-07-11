using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    NavMeshAgent nav;
    Vector3 mousePos;
    public PlayerState state;
    TerrainCollider terrainCollider;
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        terrainCollider = Terrain.activeTerrain.GetComponent<TerrainCollider>();
        nav.Warp(this.transform.position);
    }

    Ray ray;

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (terrainCollider.Raycast(ray, out hitData, 1000))
        {
            //gameObject.layer 
            mousePos = hitData.point;
        }
        if(Input.GetButtonUp("Fire1") && state == PlayerState.wandering)
        {
            if (hitData.collider.gameObject.layer == 5)
                return;
            else
            nav.destination = mousePos;
        }
    }

    public enum PlayerState
    {
        wandering,
        PanelActive
    }
}
