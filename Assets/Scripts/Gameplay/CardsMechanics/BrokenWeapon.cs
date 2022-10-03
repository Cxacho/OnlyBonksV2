using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Threading.Tasks;
public class BrokenWeapon : Card
{
    [SerializeField] private GameObject crackVFX;
    [SerializeField] private GameObject batVFX,batHandle,batRest;
    [SerializeField] private Vector3 spawnOffset, crackOffset, restSpawnOffset,handleSpawnOffset;
    [SerializeField] private Vector3 rot,restRot,handleRot;
    [SerializeField] private Vector2 moveTo,handleMoveTo,restMoveTo;
    [SerializeField] private AnimationCurve anCurve;
    [SerializeField] private float animTime,jumpPower,returnTime,rotTime;

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage and swap weapon";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage and swap weapon";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage and swap weapon";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage and swap weapon";
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
                    //switchWeapon
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

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        player.manaText.text = player.mana.ToString();

    }


    async Task DoAnim(Enemy en)
    {
        var bat = Instantiate(this.batVFX, en.transform.parent.transform.position + spawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        bat.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 25));
        var getRect = bat.GetComponent<RectTransform>();
        getRect.DOAnchorPos(moveTo + en.transform.parent.GetComponent<RectTransform>().anchoredPosition, animTime).SetEase(anCurve);
        getRect.DORotate(rot, animTime).SetEase(anCurve);
        await Task.Delay(Mathf.RoundToInt(animTime * 1000));
        var crack = Instantiate(crackVFX, bat.transform.position+crackOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var handle = Instantiate(batHandle, bat.transform.position+handleSpawnOffset,getRect.transform.rotation, gameplayManager.vfxCanvas.transform);
        var batVFX = Instantiate(batRest, bat.transform.position+restSpawnOffset, getRect.transform.rotation, gameplayManager.vfxCanvas.transform);
        Destroy(bat);
        var getHandleRect = handle.GetComponent<RectTransform>();
        var getRestRect = batVFX.GetComponent<RectTransform>();
        getHandleRect.DOAnchorPos(handleMoveTo + getHandleRect.anchoredPosition, returnTime);
        getRestRect.DOJumpAnchorPos(restMoveTo + getRestRect.anchoredPosition,jumpPower ,1,returnTime);
        handle.transform.DORotate(handleRot,0.4f,RotateMode.Fast);
        batVFX.transform.DORotate(restRot, rotTime, RotateMode.Fast);
        await Task.Delay(1000);
        handle.GetComponent<SpriteRenderer>().material.DOFade(0, 1.5f);
        batVFX.GetComponent<SpriteRenderer>().material.DOFade(0, 1.5f);
        Destroy(crack, 1.5f);
        Destroy(handle, 1.5f);
        Destroy(batVFX, 1.5f);

    }

}
