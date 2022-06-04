using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutiveFireball : Card
{

    [SerializeField]private int cost = 3;

    public override void OnDrop()
    {
        gm.checkPlayerMana(cost);
        if (gm.canPlayCards == true)
        {
            base.OnDrop();

            StartCoroutine(ExecuteAfterTime(1f));
            foreach (Enemy en in _enemies)
            {
                if (en.targeted == true)
                {
                    if (en._currentHealth<(float)(en.maxHealth*0.4))
                    {
                        en.ReceiveDamage(en.maxHealth);
                    }
                    else 
                    {
                        en.ReceiveDamage(attack);
                    }
                    en.targeted = false;
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
