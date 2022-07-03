using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoubleBonk : Card
{
    public GameObject bonk;

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage twice";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage twice";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage twice";
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
                    IEnumerator Bonking()
                    {
                        yield return new WaitForSeconds(0.15f);
                        //anim/playsfx
                        en.RecieveDamage(attack,this);

                        yield return new WaitForSeconds(0.2f);
                        //anim/playsfx
                        en.RecieveDamage(attack,this);
                    }
                    StartCoroutine(Bonking());
                    //InvokeRepeating(en.ReceiveDamage(attack + pl.strenght), 0.1f, 0.3f);

                    en.targeted = false;
                    en.isFirstTarget = false;
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