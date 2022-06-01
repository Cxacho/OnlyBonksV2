using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cringe : Card
{

    private int cost = 1;
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
                    en.SetIndicator();
                    if (en.indicatorImagesInt[en.actionsInt] == 0)
                    {
                        
                        en.otherIndicatortxt.enabled = false;
                        en.attackIndicatortxt.enabled = true;
                        en.attackIndicatortxt.text = en.indicatorStrings[en.actionsInt];
                    }
                    else
                    {
                        en.otherIndicatortxt.enabled = true;
                        en.attackIndicatortxt.enabled = false;
                        en.otherIndicatortxt.text = en.indicatorStrings[en.actionsInt];
                    }
                    en.actionsInt++;
                    
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
