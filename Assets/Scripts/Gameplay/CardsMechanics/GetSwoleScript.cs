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
            Invoke("GainStr", 1f);

            pl.manaText.text = pl.mana.ToString();
        }
        else
        {
            Debug.Log("fajnie dzia�a");
        }
    }
    




        public void GainStr()
        {
            pl.strenght +=3;
            
        }

}
