using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDefend : Card
{
    private int armor = 3;
    private int cost = 1;
    public GameObject shield;

    public override void OnDrop()
    {
        gm.checkPlayerMana(cost);
        if (gm.canPlayCards == true)
        {
            if (pl.armor == 0)
            {
                Instantiate(shield, new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, 0), Quaternion.identity, GameObject.Find("Player").transform);
            }
            pl.armor += armor;
            pl.manaText.text = pl.mana.ToString();
            base.OnDrop();
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }
        
    }

    
}