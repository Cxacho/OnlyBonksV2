using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.UI;

public class MidasToutch : Card
{
    public int armor;
    [SerializeField] private GameObject fistVFX,hitVFX;
    [SerializeField] private Vector3 spawnOffset,startRot,rot,vfxOffset;
    [SerializeField] private Vector2 movePos;
    [SerializeField] private float animTime; 
    [SerializeField] private AnimationCurve anCurve;
    [SerializeField] private Material goldMat;

    public override async void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();
            foreach (Enemy en in _enemies)
            {
                if (en.targeted == true)
                {

                    await DoAnim(en);
                    en.RecieveDamage(gameplayManager.gold*0.1f, this);
                    //metoda pod lose gold
                    
                    en.targeted = false;
                }
            }
        }
        else
        {
            Debug.Log("fajnie dzia�a");
        }

    }

    async Task DoAnim(Enemy en)
    {
        var enPos = en.transform.parent.GetComponent<RectTransform>().anchoredPosition3D;
        var fist = Instantiate(fistVFX, en.transform.parent.position +spawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);

        fist.transform.rotation = Quaternion.Euler(startRot);
        fist.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
        fist.GetComponent<SpriteRenderer>().material.DOFade(1, 0.5f);
        await Task.Delay(500);
        var getRect=fist.GetComponent<RectTransform>();
        getRect.DOAnchorPos(movePos+new Vector2(enPos.x,enPos.y), animTime).SetEase(anCurve);
        var oldMat=en.GetComponent<Image>().material;
        en.GetComponent<Image>().material = goldMat;
        fist.transform.DORotate(rot, animTime, RotateMode.Fast).SetEase(anCurve);
        await Task.Delay(Mathf.RoundToInt(animTime * 1000));
        var hit = Instantiate(hitVFX, fist.transform.position+vfxOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        fist.GetComponent<SpriteRenderer>().material.DOFade(0, 1.5f);
        en.GetComponent<Image>().material = oldMat;
        Destroy(fist, 1.5f);
        Destroy(hit, 1.5f);


    }
}
