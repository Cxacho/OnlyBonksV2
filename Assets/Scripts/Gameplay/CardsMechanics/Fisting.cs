using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fisting : Card
{
    
    void Start()
    {
        
    }

    public override void OnDrop()
    {

        gm.checkPlayerMana(cost);
        if (gm.canPlayCards == true)
        {
            base.OnDrop();

            pl.GetComponentInChildren<Animator>().SetTrigger("GetSwoleTrigger");
            Invoke("GainStr", 1f);
            gm.DrawCards(3);
            pl.manaText.text = pl.mana.ToString();
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }
    }
    public void GainStr()
    {
        pl.strenght += 2;

    }
}
