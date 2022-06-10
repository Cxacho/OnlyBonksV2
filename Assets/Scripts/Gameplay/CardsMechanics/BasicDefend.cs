using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BasicDefend : Card
{
    public int armor;
    public GameObject shield;

    private void Start()
    {
        desc = $"Gain <color=white>{armor.ToString()}</color> armor";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }

    public override void OnDrop()
    {
        gm.checkPlayerMana(cost);
        if (gm.canPlayCards == true)
        {
            pl.GetArmor(armor);


            pl.manaText.text = pl.mana.ToString();
            base.OnDrop();
            /*
            foreach (Enemy en in _enemies)
            {
                en.setStatusIndicator(3, 0, gm.enemiesIndicators[0]);
                en.setStatusIndicator(3, 1, gm.enemiesIndicators[1]);
                    en.setStatusIndicator(3, 2, gm.enemiesIndicators[2]);
            }
            */
            //gm.CreateCard(0);
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }
        
    }

    
}