using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BasicAttack : Draggable
{
    private int attack = 3;
    private int cost = 1;
    public GameObject bonk;

    

    public override void OnDrop()
    {

        Instantiate(bonk, new Vector3(0, -10, 0), Quaternion.identity, GameObject.Find("Player").transform);
        trail.enabled = true;
        StartCoroutine(ExecuteAfterTime(1f));
        
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);


        enemy.ReceiveDamage(attack);


       /* else
        {
            // enemy.currentHealth = 0;
            gm.state = BattleState.WON;
            StartCoroutine(gm.OnBattleWin());
            
        }*/
        
            pl.mana -= cost;

            pl.manaText.text = pl.mana.ToString();
   
   

        base.OnDrop();

    }

}
