using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Cringe : Card
{

    public override void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();

            StartCoroutine(ExecuteAfterTime(1f));

            //Dzia³anie karty

            if(gameplayManager.currentWeapon == GameplayManager.Weapon.Palka)
            {
                player.setStatusIndicator(2, 3, player.buffIndicators[3]);
            }
            else
            {
                player.setStatusIndicator(2, 5, player.buffIndicators[6]);
            }
            //
        }
         
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);


        //enemy.ReceiveDamage(attack * pl.strenght);


        player.manaText.text = player.mana.ToString();


        /* else
         {
             // enemy.currentHealth = 0;
             gameplayManager.state = BattleState.WON;
             StartCoroutine(gameplayManager.OnBattleWin());

         }*/





    }
}
