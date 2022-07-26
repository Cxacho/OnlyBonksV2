using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MassiveBonk : Card
{
    public GameObject bonk;

    private void Start()
    {
        desc = $"Summon a huge bat dealing <color=white>{attack.ToString()}</color> damage to targetted enemy";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Summon a huge bat dealing <color=white>{attack.ToString()}</color> damage to targetted enemy";
        else if (attack < defaultattack)
            desc = $"Summon a huge bat dealing <color=red>{attack.ToString()}</color> damage to targetted enemy";
        else
            desc = $"Summon a huge bat dealing <color=green>{attack.ToString()}</color> damage to targetted enemy";
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
                    en.RecieveDamage(attack,this);

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
             gameplayManager.state = BattleState.WON;
             StartCoroutine(gameplayManager.OnBattleWin());

         }*/





    }

}

