using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSwoleScript : Card
{

    private int cost = 2;
    private int ifcost;
    private void Start()
    {

    }
    
    public override void OnDrop()
    {
        

        base.OnDrop();

            pl.GetComponentInChildren<Animator>().SetTrigger("GetSwoleTrigger");
            Invoke("GainHP", 1f);

            pl.mana -= cost;
        
    }
    




        public void GainHP()
    {
        pl.currentHealth = pl.currentHealth + 10;
        pl.setHP();
    }

}
