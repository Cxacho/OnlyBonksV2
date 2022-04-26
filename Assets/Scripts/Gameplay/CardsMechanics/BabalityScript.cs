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

        foreach (Enemy en in enemies)
            if (en.targeted == true)
            {
                anim.Add(en.gameObject.GetComponent<Animator>());
                /*
                foreach (GameObject enm in gm.enemies)
                {
                    anim.Add(enm.GetComponent<Animator>());
                }
                */
                //foreach (Animator an in anim)
                    //an.SetTrigger("BabalityTrigger");
                    en.damage--;
                en.targeted = false;
            }
        //enemyGO.GetComponent<Animator>().SetTrigger("BabalityTrigger");




        gm.checkPlayerMana(cost);

        pl.manaText.text = pl.mana.ToString();


        base.OnDrop();

    }

}