using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DefensiveAttack : Card
{
    public int armor = 3;

    public GameObject shield;

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage and gain " + armor + " armor";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage and gain " + armor + " armor";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage and gain " + armor+ " armor";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage and gain " + armor + " armor";
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

                    if (player.armor == 0)
                    {
                        Instantiate(shield, new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, 0), Quaternion.identity, GameObject.Find("Player").transform);
                    }
                    player.armor += armor;
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
