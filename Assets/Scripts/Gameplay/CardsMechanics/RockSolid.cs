using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSolid : Card
{
    public override void OnDrop()
    {
        gm.checkPlayerMana(cost);
        if (gm.canPlayCards == true)
        {
            if (pl.currentHealth < pl.maxHealth*0.3f)
                pl.GetArmor(10);
            else
                pl.GetArmor(5);

            pl.manaText.text = pl.mana.ToString();

            base.OnDrop();

        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }
}
