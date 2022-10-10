using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samuraisway: Card
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

            /*
            gameplayManager.state = BattleState.INANIM;

            await DoAnim();
            gameplayManager.state = BattleState.PLAYERTURN;
            */

            //player.GetComponentInChildren<Animator>().SetTrigger("GetSwoleTrigger");
            player.setStatusIndicator(3, 5, player.buffIndicators[6]);
            gameplayManager.DrawCards(2);
            player.manaText.text = player.mana.ToString();
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }
    }

}
