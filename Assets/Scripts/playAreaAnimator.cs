using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class playAreaAnimator : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] float endValue;
    [SerializeField] float startValue;
    Tween one,two;
    SpriteRenderer sprite1, sprite2;
    // Start is called before the first frame update
    public void doUnfade()
    {
        sprite1 = transform.GetChild(0).GetComponent<SpriteRenderer>();
        sprite2 = transform.GetChild(1).GetComponent<SpriteRenderer>();
        sprite1.color = new Color(sprite1.color.r, sprite1.color.g, sprite1.color.b, startValue);
        sprite2.color = new Color(sprite1.color.r, sprite1.color.g, sprite1.color.b, startValue);
        one=sprite1.DOColor(new Color(sprite1.color.r, sprite1.color.g, sprite1.color.b, endValue), time).SetLoops(-1, LoopType.Yoyo);
        two=sprite2.DOColor(new Color(sprite2.color.r, sprite2.color.g, sprite2.color.b, endValue), time).SetLoops(-1, LoopType.Yoyo);
    }
    public void killTweens()
    {
        one.Kill();
        two.Kill();
        sprite1.DOColor(new Color(sprite1.color.r, sprite1.color.g, sprite1.color.b, 0), 1);
        sprite2.DOColor(new Color(sprite2.color.r, sprite2.color.g, sprite2.color.b, 0), 1);

    }
}
