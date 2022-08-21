using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class plus1 : Card
{
    [SerializeField]  private Vector3 spawnOffset;
    [SerializeField] private GameObject plusOneVFX;
    [SerializeField] float delay;
    public override async void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();
            DoAnim();
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


        //enemy.ReceiveDamage(attack * pl.strenght);


        player.manaText.text = player.mana.ToString();


        /* else
         {
             // enemy.currentHealth = 0;
             gameplayManager.state = BattleState.WON;
             StartCoroutine(gameplayManager.OnBattleWin());

         }*/





    }

    async Task DoAnim()
    {
        for (int i = 0; i < 3; i++)
        {
            var obj = Instantiate(plusOneVFX, player.transform.position + spawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            Destroy(obj, 3);
            await Task.Delay(Mathf.RoundToInt(1000*delay));
        }
    }
}
