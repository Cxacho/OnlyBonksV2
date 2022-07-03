using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PocketKnifeCard : Card
{
    //Deals 3 damage and applies 3 bleed

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage, and apply 3 bleed";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage and apply 3 bleed";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage and apply 3 bleed";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }

    public override void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();

            //Instantiate(bonk, new Vector3(0, -10, 0), Quaternion.identity, GameObject.Find("Player").transform);
            StartCoroutine(ExecuteAfterTime(1f));
            foreach (Enemy en in _enemies)
            {
                if (en.targeted == true)
                {
                    en.RecieveDamage(attack,this);
                    en.setStatusIndicator(3, 2, gameplayManager.enemiesIndicators[2]);
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


    }
}
