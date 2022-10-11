using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class BloodShed : Card
{

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage and apply 3 bleed to all enemies";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);
        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage and apply 3 bleed to all enemies";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage and apply 3 bleed to all enemies";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage and apply 3 bleed to all enemies";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    public override void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();
            /*
            gameplayManager.state = BattleState.INANIM;

            await DoAnim();
            gameplayManager.state = BattleState.PLAYERTURN;
            */
            StartCoroutine(ExecuteAfterTime(1f));
            
            foreach(Enemy en in _enemies)
            {

                en.RecieveDamage(attack, this);
                en.setStatus(Enemy.statuses.bleeding, 3, en);

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

        player.manaText.text = player.mana.ToString();

    }



}
