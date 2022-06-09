using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastStand : Card
{
    public override void OnDrop()
    {
        gm.checkPlayerMana(cost);
        if (gm.canPlayCards == true)
        {
            if (pl.armor < 4)
                pl.GetArmor(9);
            else
                pl.GetArmor(6);

            pl.manaText.text = pl.mana.ToString();

            base.OnDrop();

        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }
}
