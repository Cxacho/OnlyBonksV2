using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketKnifeCard : Card
{
    int cost = 0;
    int attack = 3;
    public override void OnDrop()
    {
        gm.checkPlayerMana(cost);
        if (gm.canPlayCards == true)
        {
            base.OnDrop();

            //Instantiate(bonk, new Vector3(0, -10, 0), Quaternion.identity, GameObject.Find("Player").transform);
            StartCoroutine(ExecuteAfterTime(1f));
            foreach (Enemy en in _enemies)
            {
                if (en.targeted == true)
                {
                    en.ReceiveDamage(attack + pl.strenght);
                    en.setStatusIndicator(3, 2, gm.enemiesIndicators[2]);
                    en.targeted = false;
                }
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


        pl.manaText.text = pl.mana.ToString();


    }
}
