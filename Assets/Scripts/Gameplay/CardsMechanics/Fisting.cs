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

        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();

            player.GetComponentInChildren<Animator>().SetTrigger("GetSwoleTrigger");
            player.setDebuffIndicator(3, 3, player.buffIndicators[3]);
            gameplayManager.DrawCards(3);
            player.manaText.text = player.mana.ToString();
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }
    }
    public void GainStr()
    {
        player.strenght += 2;

    }
}
