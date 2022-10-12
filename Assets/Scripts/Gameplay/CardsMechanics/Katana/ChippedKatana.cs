using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ChippedKatana : Card
{

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage and swap weapon";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage and swap weapon";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage and swap weapon";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage and swap weapon";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }

    public override async void OnDrop()
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
                    /*gameplayManager.state = BattleState.INANIM;
                    await DoAnim(en);
                    gameplayManager.state = BattleState.PLAYERTURN;
                    */
                    //gameplayManager.OnEnemyKilled += AddMeSomeMana;
                    en.RecieveDamage(attack, this);
                    gameplayManager.SwitchWeapon();
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

        player.manaText.text = player.mana.ToString();

    }
}
