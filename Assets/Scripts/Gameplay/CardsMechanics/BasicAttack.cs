using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BasicAttack : Card
{
    public float attack = 3;
    private int cost = 1;
    public GameObject bonk;



    public override void OnDrop()
    {
        base.OnDrop();

            Instantiate(bonk, new Vector3(0, -10, 0), Quaternion.identity, GameObject.Find("Player").transform);
            StartCoroutine(ExecuteAfterTime(1f));
            foreach (Enemy en in enemies)
            {
                if (en.targeted == true)
                {
                    en._currentHealth -= attack;
                    en.targeted = false;
                }
            } 
    }


    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        gm.checkPlayerMana(cost);
        //enemy.ReceiveDamage(attack * pl.strenght);
        

        pl.manaText.text = pl.mana.ToString();


        /* else
         {
             // enemy.currentHealth = 0;
             gm.state = BattleState.WON;
             StartCoroutine(gm.OnBattleWin());

         }*/





    }


}
