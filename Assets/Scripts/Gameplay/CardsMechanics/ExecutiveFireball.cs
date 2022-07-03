using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ExecutiveFireball : Card
{
    //If enemy has less than 40% hp execute him

    //Else deal 20 dmg

    private void Start()
    {
        desc = $"If target is below 40% of their max hp execute them, else deal <color=white>{attack.ToString()}</color> damage";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"If target is below 40% of their max hp execute them, else deal <color=white>{attack.ToString()}</color> damage.";
        else if (attack < defaultattack)
            desc = $"If target is below 40% of their max hp execute them, else deal <color=red>{attack.ToString()}</color> damage.";
        else
            desc = $"If target is below 40% of their max hp execute them, else deal <color=green>{attack.ToString()}</color> damage.";
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
                if (en.targeted == true)
                {
                    if (en._currentHealth<(float)(en.maxHealth*0.4))
                    {
                        en.RecieveDamage(en.maxHealth,this);
                    }
                    else 
                    {
                        en.RecieveDamage(attack,this);
                    }
                    en.targeted = false;
                }
            }
            resetTargetting();

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
