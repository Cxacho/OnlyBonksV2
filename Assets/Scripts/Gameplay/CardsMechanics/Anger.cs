using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class Anger : Card
{
    public int armor;
    [SerializeField] private GameObject angerVFX, fireVFX,lensVFX;
    [SerializeField] private Vector3 fire1Offset, fire2Offset,offset;
    List<GameObject> vfxses = new List<GameObject>();

    public override async void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            player.GetArmor(armor);



            player.manaText.text = player.mana.ToString();

            base.OnDrop();

            gameplayManager.state = BattleState.INANIM;
            await DoAnim();
            gameplayManager.state = BattleState.PLAYERTURN;

            gameplayManager.DrawCards(2);
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }


    async Task DoAnim()
    {
        var dogeSpriteVFX=Instantiate(angerVFX, offset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        vfxses.Add(dogeSpriteVFX);
        await Task.Delay(1500);
        var fireOne = Instantiate(fireVFX, offset + fire1Offset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        vfxses.Add(fireOne);
        var fireTwo = Instantiate(fireVFX, offset + fire2Offset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        vfxses.Add(fireTwo);
        await Task.Delay(500);
        var lensflare1 = Instantiate(lensVFX, offset + fire1Offset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var lensflare2 = Instantiate(lensVFX, offset + fire2Offset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        vfxses.Add(lensflare1);
        vfxses.Add(lensflare2);
        foreach (GameObject obj in vfxses)
            Destroy(obj.gameObject, 3f);


    }
}
