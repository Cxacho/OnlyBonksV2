using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBonk : Card
{
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
                if (en.targeted == true)
                {
                    IEnumerator Bonking()
                    {
                        yield return new WaitForSeconds(0.15f);
                        //anim/playsfx
                        en.ReceiveDamage(attack + pl.strenght);

                        yield return new WaitForSeconds(0.2f);
                        //anim/playsfx
                        en.ReceiveDamage(attack + pl.strenght);
                    }
                    StartCoroutine(Bonking());
                    //InvokeRepeating(en.ReceiveDamage(attack + pl.strenght), 0.1f, 0.3f);

                    en.targeted = false;
                    en.isFirstTarget = false;
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