using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.VFX;

public class MidasToutch : Card
{
    public int armor;
    [SerializeField] private GameObject handVFX,hitVFX,particlesVFX;
    [SerializeField] private Vector3 spawnOffset,startRot,rot,vfxOffset,particlesOffset,hitRot;
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

                    StartCoroutine(ExecuteAfterTime(1f));

                    gameplayManager.state = BattleState.INANIM;
                    await DoAnim(en);
                    gameplayManager.state = BattleState.PLAYERTURN;

                    en.RecieveDamage(gameplayManager.gold*0.1f, this);
                    //gdy zabije dodaj golda
                    
                    
                    en.targeted = false;
                }
            }
            resetTargetting();
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

    async Task DoAnim(Enemy en)
    {
        var enPos = en.transform.parent.GetComponent<RectTransform>().anchoredPosition3D;
        var fist = Instantiate(handVFX, en.transform.parent.position +spawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var par = Instantiate(particlesVFX, fist.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        par.transform.SetParent(fist.transform);
        fist.transform.rotation = Quaternion.Euler(startRot);
        fist.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
        fist.GetComponent<SpriteRenderer>().material.DOFade(1, 0.5f);
        await Task.Delay(500);
        var getRect=fist.GetComponent<RectTransform>();
        getRect.DOAnchorPos(movePos+new Vector2(enPos.x,enPos.y), animTime).SetEase(anCurve);
        //var oldMat=en.GetComponent<Image>().material;
        //en.GetComponent<Image>().material = goldMat;
        fist.transform.DORotate(rot, animTime, RotateMode.Fast).SetEase(anCurve);
        await Task.Delay(Mathf.RoundToInt(animTime * 1000));
        var par1 = Instantiate(particlesVFX, en.transform.parent.transform.position + particlesOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var hit = Instantiate(hitVFX, fist.transform.position+vfxOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        hit.transform.rotation = Quaternion.Euler(hitRot);


        
        await Task.Delay(500);
        fist.GetComponent<SpriteRenderer>().material.DOFade(0, 1.5f);
        await Task.Delay(1500);
        Destroy(fist, 1f);
        Destroy(hit, 1f);
        Destroy(par, 2f);
        Destroy(par1, 3f);

    }
}
