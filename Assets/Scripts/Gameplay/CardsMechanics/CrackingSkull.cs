using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Threading.Tasks;
public class CrackingSkull : Card
{
    [SerializeField] private GameObject bonkComicVFX,particleVFX,crackVFX;
    [SerializeField] private GameObject batVFX;
    [SerializeField] private Vector3 spawnOffset, hitOffset, spriteSpawnOffset,crackSpawnOffset;
    [SerializeField] private Vector3 rot;
    [SerializeField] private AnimationCurve anCurve;
    [SerializeField] private float animTime, animMultiplier;
    int selectedTarget;
    List<GameObject> ens = new List<GameObject>();

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage";
        else if (attack < defaultattack)
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
            foreach (Enemy en in _enemies)
            {
                if (en.targeted == true)
                {
                    await DoAnim(en);
                    bool isInvoked = false;
                    if (en.armor < attack && en.armor > 0)
                    {
                        en.OnDamageRecieved += InvokeEvent;
                        isInvoked = true;
                    }
                    selectedTarget = en.transform.parent.GetSiblingIndex();
                    en.RecieveDamage(attack, this);
                    if(isInvoked==true)
                    en.OnDamageRecieved -= InvokeEvent;
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
    private void OnEnable()
    {
        ens = gameplayManager.enemies;
    }
    void InvokeEvent(Card card,float dam,Enemy en)
    {
        player.mana += 4;
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        player.manaText.text = player.mana.ToString();

    }

    async Task DoAnim(Enemy en)
    {
        var bat = Instantiate(batVFX, en.transform.parent.transform.position + spawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        bat.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
        bat.GetComponent<SpriteRenderer>().material.DOFade(1, 0.5f);
        await Task.Delay(500);
        
        var getRect = bat.GetComponent<RectTransform>();
        getRect.DOAnchorPos3D(hitOffset + en.transform.parent.GetComponent<RectTransform>().anchoredPosition3D, animTime).SetEase(anCurve);
        await Task.Delay(150);
        getRect.DORotate(rot, animTime * animMultiplier).SetEase(anCurve);
        await Task.Delay(100);
        
        var particles = Instantiate(particleVFX, en.transform.parent.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var skull = Instantiate(bonkComicVFX, getRect.transform.position + spriteSpawnOffset, Quaternion.Euler(new Vector3(0,0,25)), gameplayManager.vfxCanvas.transform);
        skull.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 25));
        await Task.Delay(1000);
        bat.GetComponent<SpriteRenderer>().material.DOFade(0, 1.5f);
        skull.GetComponent<SpriteRenderer>().material.DOFade(0, 1.5f);
        Destroy(bat,2);
        Destroy(skull,2);
        Destroy(particles, 3);


    }

}
