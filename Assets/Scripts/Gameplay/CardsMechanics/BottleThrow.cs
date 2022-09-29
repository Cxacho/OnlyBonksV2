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

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color>damage and apply 3 bleed";
    }

    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);
        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color>damage and apply 3 bleed";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color>damage and apply 3 bleed";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color>damage and apply 3 bleed";
    }

    public override void OnDrop()
    {
        
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {

            //naloz bleed, dodaj armor???
            player.manaText.text = player.mana.ToString();

            base.OnDrop();
            foreach (Enemy en in _enemies)
            {
                if (en.targeted == true)
                {

                    en.RecieveDamage(attack, this);
                    DoAnim(en);
                }
                en.setStatus(Enemy.statuses.bleeding, 3,en);
                en.targeted = false;
            }
            resetTargetting();
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }

    void DoAnim(Enemy en)
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
            {
                var obj1 = Instantiate(shardsVFXRight, bottleObj.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
                Destroy(obj1, 3.5f);
            }
            else if (enIndex == _enemies.Count - 1)
            {
                var obj2 = Instantiate(shardsVFXLeft, bottleObj.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
                Destroy(obj2, 3.5f);
            }
            else
            {
                var obj3 = Instantiate(shardsVFX, bottleObj.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
                Destroy(obj3, 3.5f);
            }

        });

    }
}
