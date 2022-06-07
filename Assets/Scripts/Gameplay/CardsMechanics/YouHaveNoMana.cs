using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouHaveNoMana : Card
{


    public override void OnDrop()
    {
        gm.checkPlayerMana(cost);
        if (gm.canPlayCards == true)
        {
            base.OnDrop();
 
            pl.mana += 1;
            StartCoroutine(ExecuteAfterTime(1f));
            
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        pl.manaText.text = pl.mana.ToString();


    }
}
