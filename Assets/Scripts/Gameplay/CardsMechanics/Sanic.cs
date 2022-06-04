using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanic : Card
{

    [SerializeField]private int cost = 2;


    public override void OnDrop()
    {
        
        gm.checkPlayerMana(cost);
        if (gm.canPlayCards == true)
        {
            Debug.Log(_enemies.Count);
            StartCoroutine(ExecuteAfterTime(1f));
            foreach (Enemy en in _enemies)
            {

                en.ReceiveDamage(attack + pl.strenght);
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


        pl.manaText.text = pl.mana.ToString();


        /* else
         {
             // enemy.currentHealth = 0;
             gm.state = BattleState.WON;
             StartCoroutine(gm.OnBattleWin());

         }*/





    }

}
