using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Homerun :  Card
{

    private void Start()
    {
        var get03procent = defaultattack * 0.3f;

        desc = $"Deal <color=white>{attack.ToString()}</color> damage to first enemy and <color=white>{get03procent.ToString()}</color> to second enemy";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);
        var secondAttack = Mathf.RoundToInt(attack * 0.3f);
        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage to first enemy and <color=white>{secondAttack.ToString()}</color> to second enemy";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage to first enemy and <color=red>{secondAttack.ToString()}</color> to second enemy";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage to first enemy and <color=green>{secondAttack.ToString()}</color> to second enemy";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }

    public override void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();

            StartCoroutine(ExecuteAfterTime(1f));
            foreach (Enemy en in _enemies)
            {
                if (en.isFirstTarget == true)
                {
                    en.RecieveDamage(attack,this);
                    en.targeted = false;
                    en.isFirstTarget = false;
                }
                if (en.isSecondTarget == true)
                {
                    en.RecieveDamage(Mathf.RoundToInt((attack)*0.3f),this); // do zmiany po demie
                    en.targeted = false;
                    en.isSecondTarget = false;
                }
               // resetTargetting();
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
             gm.state = BattleState.WON;
             StartCoroutine(gm.OnBattleWin());

         }*/





    }

}


