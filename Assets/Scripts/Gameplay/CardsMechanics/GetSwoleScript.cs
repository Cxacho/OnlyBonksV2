using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSwoleScript : Card
{

    
    
    public override void OnDrop()
    {

        gm.checkPlayerMana(cost);
        if (gm.canPlayCards == true)
        {
            base.OnDrop();

            pl.GetComponentInChildren<Animator>().SetTrigger("GetSwoleTrigger");
            Invoke("GainHP", 1f);

            pl.manaText.text = pl.mana.ToString();
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }
    }
    




        public void GainHP()
        {
            pl.currentHealth = pl.currentHealth + 10;
            pl.strenght++;
            
            pl.setHP();
        }

}
