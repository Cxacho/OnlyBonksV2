using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class Upgrade : Card
{
    public int armor;
    [SerializeField] private AnimationCurve anCurve;
    [SerializeField] private GameObject batVFX;
    [SerializeField] private GameObject hammerVFX,smokeVFX;
    [SerializeField] private Vector3 hammerOffset, smokeOffset,batOffset;
    List<GameObject> vfxses = new List<GameObject>();
    private Rigidbody2D rb;

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
        var hammerObj=Instantiate(hammerVFX, gameplayManager.player.transform.position +hammerOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var batObj = Instantiate(batVFX, gameplayManager.player.transform.position + batOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        batObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
        vfxses.Add(batObj);
        vfxses.Add(hammerObj);
        //Sequence anim = new Sequence();
        hammerObj.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 50));
        var getRot = hammerObj.transform.rotation;
        await Task.Delay(200);
        hammerObj.transform.DORotate(new Vector3(0, 180, -40), 0.5f, RotateMode.Fast).SetEase(anCurve).SetLoops(1,LoopType.Yoyo);
        await Task.Delay(400);
        var smok1=Instantiate(smokeVFX, gameplayManager.player.transform.position + smokeOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        vfxses.Add(smok1);
        hammerObj.transform.DORotate(getRot.eulerAngles, 0.3f, RotateMode.Fast).SetEase(anCurve).SetLoops(1, LoopType.Yoyo);
        await Task.Delay(300);
        hammerObj.transform.DORotate(new Vector3(0, 180, -40), 0.5f, RotateMode.Fast).SetEase(anCurve).SetLoops(1, LoopType.Yoyo);
        await Task.Delay(400);
        var smok2=Instantiate(smokeVFX, gameplayManager.player.transform.position + smokeOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        vfxses.Add(smok2);
        //instanitiate smokevfx
        foreach (GameObject obj in vfxses)
            Destroy(obj, 3f);



    }
}
