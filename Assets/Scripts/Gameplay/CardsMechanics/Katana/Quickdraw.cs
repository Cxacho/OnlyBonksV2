using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class Quickdraw : Card
{
    [SerializeField]GameObject cutVfx;
    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage and apply 2 energize";
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);
        var secondAttack = Mathf.RoundToInt(attack * 0.3f);
        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage and apply 2 energize";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage and apply 2 energize";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage and apply 2 energize";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }

    public override void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();
            player.setStatusIndicator(2, 4, player.buffIndicators[0]);

            //ui.DisableButtons(getPanel);
            foreach (Enemy en in _enemies)
            {
                if (en.targeted == true)
                {
                    {
                        en.RecieveDamage(attack, this);
                        en.targeted = false;
                    }

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


