using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class Howl : Card
{
    [SerializeField] GameObject howlVFX;
    [SerializeField] private Vector3 offset;


    public override async void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {

            base.OnDrop();
            StartCoroutine(ExecuteAfterTime(1f));
            gameplayManager.state = BattleState.INANIM;
            await DoAnim();
            gameplayManager.state = BattleState.PLAYERTURN;
            player.setStatusIndicator(4, 5, player.buffIndicators[6]);
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


        player.manaText.text = player.mana.ToString();


    }

    async Task DoAnim()
    {
        var prefab = Instantiate(howlVFX, player.gameObject.transform.position+offset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        Destroy(prefab, 5);


    }
}
