using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSwoleScript : Card
{

    
    
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

            player.GetComponentInChildren<Animator>().SetTrigger("GetSwoleTrigger");
            player.setStatusIndicator(5, 3, player.buffIndicators[3]);
            player.setStatusIndicator(5, 5, player.buffIndicators[6]);
            player.GetArmor(10);
            player.manaText.text = player.mana.ToString();
        }
        else
        {
            Debug.Log("fajnie dzia�a");
        }
    }
    




        public void GainStr()
        {
            player.strenght +=4;
            
        }

}
