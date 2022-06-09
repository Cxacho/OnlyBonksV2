using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testudo : Card
{
    public int armor;
    public override void OnDrop()
    {
        gm.checkPlayerMana(cost);
        if (gm.canPlayCards == true)
        {
            pl.GetArmor(armor);
            gm.DrawCards(2);

            pl.manaText.text = pl.mana.ToString();

            base.OnDrop();
           
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }
}
