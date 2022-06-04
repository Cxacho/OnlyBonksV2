using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homerun :  Card
{
    private int cost = 2;
    public GameObject bonk;



    public override void OnDrop()
    {
        gm.checkPlayerMana(cost);
        if (gm.canPlayCards == true)
        {
            base.OnDrop();

            StartCoroutine(ExecuteAfterTime(1f));
            foreach (Enemy en in _enemies)
            {
                if (en.isFirstTarget == true)
                {
                    en.ReceiveDamage(attack + pl.strenght);
                    en.targeted = false;
                    en.isFirstTarget = false;
                }
                if (en.isSecondTarget == true)
                {
                    en.ReceiveDamage(attack * 0.3f + pl.strenght);
                    en.targeted = false;
                    en.isSecondTarget = false;
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


        pl.manaText.text = pl.mana.ToString();


        /* else
         {
             // enemy.currentHealth = 0;
             gm.state = BattleState.WON;
             StartCoroutine(gm.OnBattleWin());

         }*/





    }

}


