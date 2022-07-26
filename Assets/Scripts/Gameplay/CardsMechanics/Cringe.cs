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

            player.frail = 0;
            player.vurneable = 0;
            player.poison = 0;

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("BuffIndicator"))
            {
                if (obj.name!= "StrengthBuffIndcator(Clone)") {
                    var some = obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    int statusAmount;
                    int.TryParse(some.text.ToString(), out statusAmount);
                    statusAmount = 0;
                    if (statusAmount > 0)
                    {
                        some.text = statusAmount.ToString();

                    }
                    else
                    {
                        Destroy(obj);

                    } 
                }
            }
        }
        else
        {
            Debug.Log("fajnie dzia³a");
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
