using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSolid : Card
{
    public override void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            if (player.currentHealth < player.maxHealth*0.3f)
                player.GetArmor(10);
            else
                player.GetArmor(5);

            player.manaText.text = player.mana.ToString();

            base.OnDrop();

        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }
}
