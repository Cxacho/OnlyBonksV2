using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabalityScript : Card
{
    
    private int cost = 2;
    private Vector3 tempora;
    private Vector2 temporaY;
    List<Animator> anim = new List<Animator>(); 

        

    public override void OnDrop()
    {

        /* tempora = enemyGO.gameObject.transform.localScale;
         temporaY = enemyGO.gameObject.GetComponent<RectTransform>().anchoredPosition;
         tempora.x += -0.5f;
         tempora.y += -0.5f;
         temporaY = new Vector2(0,-50f);

         enemyGO.gameObject.GetComponent<RectTransform>().anchoredPosition = temporaY;
         enemyGO.gameObject.transform.localScale = tempora; */
        /*
        foreach(GameObject en in gm.enemieslist)
        {
            en.GetComponent<Animator>().SetTrigger("BabalityTrigger");
        }
        */
        foreach (Enemy en in enemies)
            if (en.targeted == true)
            {
                en.damage--;
                en.targeted = false;
            }
        //enemyGO.GetComponent<Animator>().SetTrigger("BabalityTrigger");




        gm.checkPlayerMana(cost);

        pl.manaText.text = pl.mana.ToString();


        base.OnDrop();

    }

}