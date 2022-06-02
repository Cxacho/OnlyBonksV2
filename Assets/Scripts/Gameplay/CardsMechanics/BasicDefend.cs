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
            foreach (Enemy en in _enemies)
            {
                en.setStatusIndicator(3, 0, gm.enemiesIndicators[0]);
                en.setStatusIndicator(3, 1, gm.enemiesIndicators[1]);
                    en.setStatusIndicator(3, 2, gm.enemiesIndicators[2]);
            }
            //gm.DrawCards(6);
            //gm.CreateCard(0);
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }
        
    }

    
}