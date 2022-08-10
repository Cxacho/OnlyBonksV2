using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.VFX;
public class BottleThrow : Card
{
    [SerializeField] private GameObject bottleVFX;
    [SerializeField] private GameObject shardsVFX, shardsVFXRight, shardsVFXLeft;
    [SerializeField] private Vector3 offset, anim, smokOffset;
    [SerializeField] float oneTimeValue;
    [SerializeField] AnimationCurve anCurve;
    GameObject objToDestroy;


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
            foreach (Enemy en in _enemies)
            {
                if (en.targeted == true)
                {

                    en.RecieveDamage(attack, this);
                    await DoAnim(en);
                }
                en.setStatusIndicator(3, 2, gameplayManager.enemiesIndicators[2]);
                en.targeted = false;
            }
            resetTargetting();
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }
    async Task DoAnim(Enemy en)
    {
        var bottleObj = Instantiate(bottleVFX, gameplayManager.player.transform.position + offset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var getRect= bottleObj.GetComponent<RectTransform>();
        var enIndex = en.transform.parent.GetSiblingIndex();
        //zrobic liste ze wzsystkich enemy battlestations
        getRect.DORotate(new Vector3(0, 0, -360), 1f,RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        getRect.DOAnchorPos(en.transform.parent.GetComponent<RectTransform>().anchoredPosition, 0.7f).SetEase(anCurve).OnComplete(() =>
        {
            Destroy(bottleObj,0.3f);
            if (enIndex == 0)
                objToDestroy = Instantiate(shardsVFXRight, bottleObj.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            
            else if (enIndex == _enemies.Count-1)
                objToDestroy= Instantiate(shardsVFXLeft, bottleObj.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
                    else
                objToDestroy = Instantiate(shardsVFX, bottleObj.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);

        });
        Destroy(objToDestroy,3.5f);

    }
}
