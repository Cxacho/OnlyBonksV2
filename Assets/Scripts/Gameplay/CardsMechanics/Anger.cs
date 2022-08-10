using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class Anger : Card
{
    public int armor;
    [SerializeField] private GameObject angerVFX, fireVFX;
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
            await Doanim();
            gameplayManager.DrawCards(2);
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }

    async Task Doanim()
    {
        var dogeSpriteVFX=Instantiate(angerVFX, offset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        vfxses.Add(dogeSpriteVFX);
        await Task.Delay(3500);
        var fireOne = Instantiate(fireVFX, offset + fire1Offset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        vfxses.Add(fireOne);
        var fireTwo = Instantiate(fireVFX, offset + fire2Offset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        vfxses.Add(fireTwo);
        foreach (GameObject obj in vfxses)
            Destroy(obj, 1f);



    }
}
