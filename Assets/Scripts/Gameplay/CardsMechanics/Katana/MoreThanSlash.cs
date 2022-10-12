using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Threading.Tasks;

public class MoreThanSlash : Card
{
    [SerializeField] private GameObject cutVFX;
    [SerializeField] private GameObject katVFX, woundVFX;
    [SerializeField] private Vector3 spawnOffset, hitOffset, spriteSpawnOffset;
    [SerializeField] private Vector3 rot, startingRot;
    [SerializeField] private AnimationCurve anCurve;
    [SerializeField] private float animTime, animMultiplier;
    private TextMeshPro textMeshPro;

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage, add 1 mana and draw 1 card";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage, add 1 mana and draw 1 card";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage, add 1 mana and draw 1 card";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage, add 1 mana and draw 1 card";
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
                    //gameplayManager.OnEnemyKilled += AddMeSomeMana;
                    en.RecieveDamage(attack, this);
                    player.mana += 1;
                    player.manaText.text = player.mana.ToString();
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
    void AddMeSomeMana(object sender, EventArgs e)
    {
        player.mana += 4;
    }
    private void OnDestroy()
    {
        // gameplayManager.OnEnemyKilled -= AddMeSomeMana;   
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        player.manaText.text = player.mana.ToString();

    }

    async Task DoAnim(Enemy en)
    {
        var bat = Instantiate(katVFX, en.transform.parent.transform.position + spawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        bat.transform.rotation = Quaternion.Euler(startingRot);
        var getRect = bat.GetComponent<RectTransform>();
        getRect.DOAnchorPos3D(hitOffset + en.transform.parent.GetComponent<RectTransform>().anchoredPosition3D, animTime).SetEase(anCurve);
        await Task.Delay(150);
        getRect.DORotate(rot, animTime * animMultiplier).SetEase(anCurve);
        await Task.Delay(100);
        var comicSprite = Instantiate(cutVFX, en.transform.parent.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var wound = Instantiate(woundVFX, en.transform.parent.transform.position + spriteSpawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        comicSprite.transform.DOScale(new Vector3(comicSprite.transform.localScale.x * 1.25f, comicSprite.transform.localScale.y * 1.25f, comicSprite.transform.localScale.z * 1.25f), 0.3f);
        await Task.Delay(300);
        bat.GetComponent<SpriteRenderer>().material.DOFade(0, 1);
        comicSprite.transform.DOScale(Vector3.zero, 0.6f).OnComplete(() =>
        {
            Destroy(bat);
            Destroy(comicSprite, 2f);
            Destroy(wound);
        });

    }
}
