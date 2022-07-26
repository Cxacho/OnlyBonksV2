using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabalityScript : Card
{
    
    
    List<Animator> anim = new List<Animator>(); 

        

    public override void OnDrop()
    {

        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            foreach (Enemy en in _enemies)
                if (en.targeted == true)
                {
                    anim.Add(en.gameObject.GetComponent<Animator>());
                    /*
                    foreach (GameObject enm in gameplayManager.enemies)
                    {
                        anim.Add(enm.GetComponent<Animator>());
                    }
                    */
                    //foreach (Animator an in anim)
                        //an.SetTrigger("BabalityTrigger");
                    en.damage-=3;
                    en.targeted = false;
                }

            //enemyGO.GetComponent<Animator>().SetTrigger("BabalityTrigger");
            resetTargetting();





            player.manaText.text = player.mana.ToString();
            base.OnDrop();
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

        

    }

}