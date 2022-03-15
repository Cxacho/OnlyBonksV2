using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabalityScript : Draggable
{
    
    private int cost = 2;
    private Vector3 tempora;
    private Vector2 temporaY;


    public override void OnDrop()
    {

        /* tempora = enemyGO.gameObject.transform.localScale;
         temporaY = enemyGO.gameObject.GetComponent<RectTransform>().anchoredPosition;
         tempora.x += -0.5f;
         tempora.y += -0.5f;
         temporaY = new Vector2(0,-50f);

         enemyGO.gameObject.GetComponent<RectTransform>().anchoredPosition = temporaY;
         enemyGO.gameObject.transform.localScale = tempora; */

        enemyGO.GetComponent<Animator>().SetTrigger("BabalityTrigger");

        enemy.damage--;


        pl.mana -= cost;

        pl.manaText.text = pl.mana.ToString();


        base.OnDrop();

    }

}