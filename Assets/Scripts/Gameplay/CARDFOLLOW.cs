using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CARDFOLLOW : MonoBehaviour
{
    private Vector3 mousePos;
    public void Update()
    {
       mousePos  = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public void Onclick()
    {
        
        this.transform.position = mousePos;
    }
}


