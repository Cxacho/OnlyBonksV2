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
            

            pl.manaText.text = pl.mana.ToString();

            base.OnDrop();
            gm.DrawCards(2);
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }
}
