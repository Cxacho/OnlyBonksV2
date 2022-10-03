using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Threading.Tasks;
public class DeadliestCultery : Card
{
    private TextMeshPro textMeshPro;
    int defAttack;
    [SerializeField] private GameObject hit;
    [SerializeField] private GameObject blade;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField]private Vector2 retreatOffset;
    [SerializeField] private float knifeJumpPower,fallingPower;
    [SerializeField] AnimationCurve anCurve;
    [SerializeField] private float animTime,animTime2;

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage and draw a card";
        defAttack = Mathf.RoundToInt(defaultattack);
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage and draw a card";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage and draw a card";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage and draw a card";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    public override async void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();
            StartCoroutine(ExecuteAfterTime(1f));
            foreach (Enemy en in _enemies)
            {
                if (en.targeted == true)
                {
                    gameplayManager.state = BattleState.INANIM;
                    await DoAnim(en);
                    gameplayManager.state = BattleState.PLAYERTURN;
                    en.RecieveDamage(attack, this);
                    gameplayManager.DrawCards(1);
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

        player.manaText.text = player.mana.ToString();

    }


    async Task DoAnim(Enemy en)
    {
        var noz = Instantiate(blade, player.transform.position + spawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var getRect = noz.GetComponent<RectTransform>();
        getRect.DOJumpAnchorPos(en.transform.parent.GetComponent<RectTransform>().anchoredPosition, knifeJumpPower, 1, animTime).SetEase(anCurve);
        getRect.DORotate(new Vector3(0, 0, -360), animTime, RotateMode.FastBeyond360);
        await Task.Delay(Mathf.RoundToInt(animTime * 1000));
        var hitAnim=Instantiate(hit, noz.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        Destroy(noz.transform.GetChild(0).gameObject);
        getRect.DOJumpAnchorPos(getRect.anchoredPosition + retreatOffset,fallingPower, 1, animTime2);
        getRect.DORotate(new Vector3(0, 0, -360), animTime2, RotateMode.FastBeyond360);
        noz.GetComponent<SpriteRenderer>().material.DOFade(0, animTime2);
        Destroy(noz,2);
        Destroy(hitAnim,2);
    }

}
