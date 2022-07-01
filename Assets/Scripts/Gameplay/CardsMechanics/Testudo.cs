using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testudo : Card
{
    public int armor;
    public override void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            player.GetArmor(armor);
            

            player.manaText.text = player.mana.ToString();

            base.OnDrop();
            gameplayManager.DrawCards(2);
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }
}
