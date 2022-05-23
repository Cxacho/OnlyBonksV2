using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Savagery : Card
{
    private int cost = 0;
    public GameObject bonk;
    [SerializeField]private int value;



    public override void OnDrop()
    {
        gm.checkPlayerMana(cost);
        if (gm.canPlayCards == true)
        {
            base.OnDrop();
            
            pl.TakeDamage(value);
            pl.mana += 1;
            StartCoroutine(ExecuteAfterTime(1f));
            pl.strenght += 2;

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
