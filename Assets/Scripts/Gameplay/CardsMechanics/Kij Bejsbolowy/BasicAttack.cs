using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Threading.Tasks;
public class BasicAttack : Card
{
    [SerializeField] private GameObject bonkComicVFX;
    [SerializeField] private GameObject batVFX;
    [SerializeField] private Vector3 spawnOffset, hitOffset,spriteSpawnOffset;
    [SerializeField] private Vector3 rot;
    [SerializeField] private AnimationCurve anCurve;
    private TextMeshPro textMeshPro;
    [SerializeField] private float animTime, animMultiplier;

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }
  

    private void FixedUpdate()
    {
        
         calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);
        
        if(attack == defaultattack)
        desc = $"Deal <color=white>{attack.ToString()}</color> damage";
        else if(attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    public override async void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();

            StartCoroutine(ExecuteAfterTime(1f));

            //foreach stosowany, gdy działanie karty ma interakcję z jakimkolwiek przeciwnikiem
            foreach (Enemy en in _enemies)
            {
                
                if (en.targeted == true)
                {
                    
                    gameplayManager.state = BattleState.INANIM;
                    await DoAnim(en);
                    gameplayManager.state = BattleState.PLAYERTURN;

                    //miejsce na działanie karty

                    en.RecieveDamage(attack,this);
                    
                    //

                    en.targeted = false;
                }
            }
            resetTargetting();
        }

    }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        player.manaText.text = player.mana.ToString();

    }

    async Task DoAnim(Enemy en)
    {
        var bat=Instantiate(batVFX, en.transform.parent.transform.position+spawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        bat.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 25));
        var getRect=bat.GetComponent<RectTransform>();
        getRect.DOAnchorPos3D(hitOffset+en.transform.parent.GetComponent<RectTransform>().anchoredPosition3D, animTime).SetEase(anCurve);
        await Task.Delay(150);
        getRect.DORotate(rot, animTime*animMultiplier).SetEase(anCurve);
        await Task.Delay(100);
        var comicSprite = Instantiate(bonkComicVFX, getRect.transform.position+spriteSpawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        comicSprite.transform.DOScale(new Vector3(comicSprite.transform.localScale.x * 1.25f, comicSprite.transform.localScale.y * 1.25f, comicSprite.transform.localScale.z * 1.25f),0.3f);
        await Task.Delay(300);
        bat.GetComponent<SpriteRenderer>().material.DOFade(0, 1);
        comicSprite.transform.DOScale(Vector3.zero, 0.6f).OnComplete(() =>
        {
            Destroy(bat);
            Destroy(comicSprite);
        });

    }

}
