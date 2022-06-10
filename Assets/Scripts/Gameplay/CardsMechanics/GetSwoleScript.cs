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
            pl.setDebuffIndicator(5, 3, pl.buffIndicators[3]);
            pl.GetArmor(10);
            pl.manaText.text = pl.mana.ToString();
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }
    }
    




        public void GainStr()
        {
            pl.strenght +=4;
            
        }

}
