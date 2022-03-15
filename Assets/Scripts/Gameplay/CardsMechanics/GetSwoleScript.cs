using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSwoleScript : Draggable
{

    private int cost = 2;
    private int ifcost;

    public override void OnDrop()
    {
        pl.GetComponentInChildren<Animator>().SetTrigger("GetSwoleTrigger");
        Invoke("GainHP", 1f);

            pl.mana -= cost;

            pl.manaText.text = pl.mana.ToString();
      
   

        base.OnDrop();
    }

    public void GainHP()
    {
        pl.currentHealth = pl.currentHealth + 10;
        pl.setHP();
    }

}
