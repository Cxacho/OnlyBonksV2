using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Sanic : Card
{

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage to every enemy";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage to every enemy";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage to every enemy";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }

    public override void OnDrop()
    {
        
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            Debug.Log(_enemies.Count);
            StartCoroutine(ExecuteAfterTime(1f));
            foreach (Enemy en in _enemies)
            {

                en.RecieveDamage(attack,this);
                Debug.Log("wykonaj");

            }

            base.OnDrop();

            
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
