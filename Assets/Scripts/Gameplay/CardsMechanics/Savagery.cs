using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Savagery : Card
{
    public GameObject bonk;
    [SerializeField]private int value;



    public override void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();

            player.TakeDamage(value);
            player.mana += 1;
            StartCoroutine(ExecuteAfterTime(1f));
            gameplayManager.DrawCards(1);

        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        player.manaText.text = player.mana.ToString();


    }

}
