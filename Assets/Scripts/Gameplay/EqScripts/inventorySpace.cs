using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventorySpace : MonoBehaviour
{
    public bool occupied;
    SpriteRenderer sr;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (occupied)
            sr.color = Color.blue;
        else
            sr.color = Color.white;
    }
}
