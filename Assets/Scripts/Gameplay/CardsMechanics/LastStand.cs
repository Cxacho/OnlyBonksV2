using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastStand : Card
{
    public override void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            if (player.armor < 4)
                player.GetArmor(9);
            else
                player.GetArmor(6);

            player.manaText.text = player.mana.ToString();

            base.OnDrop();

        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }
}
