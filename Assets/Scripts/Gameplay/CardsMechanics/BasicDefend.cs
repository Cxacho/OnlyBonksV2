using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDefend : Draggable
{
    private int armor = 3;
    private int cost = 1;
    public GameObject shield;

    public override void OnDrop()
    {

        if (pl.armor == 0)
        {
            Instantiate(shield, new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, 0), Quaternion.identity, GameObject.Find("Player").transform);
        }

        pl.armor += armor;

        

        
            pl.mana -= cost;

            pl.manaText.text = pl.mana.ToString();
       

        base.OnDrop();
    }

    
}