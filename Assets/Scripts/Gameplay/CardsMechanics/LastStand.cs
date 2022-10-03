using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
public class LastStand : Card
{
    [SerializeField] private GameObject shieldVFX;
    [SerializeField] private GameObject smokeVFX;
    [SerializeField] private Vector3 offset, anim, smokOffset;

    public override async void OnDrop()
    {

        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            
            if (player.armor < 4)
                player.GetArmor(9);
            else
                player.GetArmor(6);

            player.manaText.text = player.mana.ToString();

            base.OnDrop();
            gameplayManager.state = BattleState.INANIM;
            await DoAnim();
            gameplayManager.state = BattleState.PLAYERTURN;
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }
    async Task DoAnim()
    {
        var sVfx = Instantiate(shieldVFX, gameplayManager.player.transform.position+offset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        sVfx.transform.DOMove(gameplayManager.player.transform.position, 0.3f);
        await Task.Delay(300);
        var getMyPos = sVfx.transform.position;
        sVfx.transform.DOMove(gameplayManager.player.transform.position + anim, 0.5f);
        await Task.Delay(500);
        sVfx.transform.DOMoveY(getMyPos.y-2, 0.5f);
        await Task.Delay(500);
        var smok = Instantiate(smokeVFX, sVfx.transform.position + smokOffset, Quaternion.identity,gameplayManager.vfxCanvas.transform);
        await Task.Delay(100);
        sVfx.transform.DOScale(Vector3.zero, 1f);
        Destroy(smok, 1f);
        Destroy(sVfx, 1f);
    }
}
