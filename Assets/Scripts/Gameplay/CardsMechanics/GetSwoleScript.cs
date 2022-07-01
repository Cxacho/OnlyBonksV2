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

            player.GetComponentInChildren<Animator>().SetTrigger("GetSwoleTrigger");
            player.setDebuffIndicator(5, 3, player.buffIndicators[3]);
            player.GetArmor(10);
            player.manaText.text = player.mana.ToString();
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }
    }
    




        public void GainStr()
        {
            player.strenght +=4;
            
        }

}
