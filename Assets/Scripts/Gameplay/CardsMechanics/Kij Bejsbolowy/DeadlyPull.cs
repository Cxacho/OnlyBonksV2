using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Threading.Tasks;
public class DeadlyPull : Card
{
    [SerializeField] private GameObject woundVFX,trail;
    [SerializeField] private Vector3 spawnOffset, jumpPos, hitOffset;
    [SerializeField] private float jumpPower, animTime,returnTime,fallingPower,supTime;
    [SerializeField] private AnimationCurve anCurve,retCurve,riseCurve,fallCurve;
    [SerializeField] Vector2 suplexPos;
    private Enemy firstEnemy, secondEnemy;
    private TextMeshPro textMeshPro;
    private RectTransform trail1, trail2;

    private void Start()
    {
        desc = $"Pull up to you 2 enemies and deal <color=white>{attack.ToString()}</color> damage to them";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;

    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Pull up to you 2 enemies and deal <color=white>{attack.ToString()}</color> damage to them";
        else if (attack < defaultattack)
            desc = $"Pull up to you 2 enemies and deal <color=red>{attack.ToString()}</color> damage to them";
        else
            desc = $"Pull up to you 2 enemies and deal <color=green>{attack.ToString()}</color> damage to them";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
        if(firstEnemy!=null&& gameplayManager.enemies.Count > 1)
        trail1.anchoredPosition = firstEnemy.transform.parent.GetComponent<RectTransform>().anchoredPosition;
        if (secondEnemy != null && gameplayManager.enemies.Count>1)
            trail2.anchoredPosition = secondEnemy.transform.parent.GetComponent<RectTransform>().anchoredPosition;
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
                if (en.isFirstTarget == true)
                {

                    firstEnemy = en;

                }
                if (en.isSecondTarget == true)
                {
                    secondEnemy = en;


                }
            }
            await DoAnim();
            resetTargetting();
        }

        else
        {
            Debug.Log("fajnie dzia³a");
        }

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
    async Task DoAnim()
    {
        //trail
        var fEnOldPos = firstEnemy.transform.parent.GetComponent<RectTransform>().anchoredPosition;

        
        
        if (gameplayManager.enemies.Count>1)
        {
            var sEnOldPos = secondEnemy.transform.parent.GetComponent<RectTransform>().anchoredPosition;
            var getFEnemyRect = firstEnemy.transform.parent.GetComponent<RectTransform>();
            var getSEnemyRect = secondEnemy.transform.parent.GetComponent<RectTransform>();
            float center = 0;
            if (getFEnemyRect.anchoredPosition.x > getSEnemyRect.anchoredPosition.x)
            {
                firstEnemy.gameObject.transform.DORotate(new Vector3(0, 180, 0), 0.2f, RotateMode.Fast);
                center = (getFEnemyRect.anchoredPosition.x + getSEnemyRect.anchoredPosition.x) * 0.5f;
            }
            else
            {
                secondEnemy.gameObject.transform.DORotate(new Vector3(0, 180, 0), 0.2f, RotateMode.Fast);
                center = (getFEnemyRect.anchoredPosition.x + getSEnemyRect.anchoredPosition.x) * 0.5f;
            }
            trail1=Instantiate(trail, firstEnemy.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform).GetComponent<RectTransform>();
            trail2=Instantiate(trail, secondEnemy.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform).GetComponent<RectTransform>();
            getFEnemyRect.DOJumpAnchorPos(new Vector3(center + jumpPos.x, jumpPos.y, jumpPos.z), jumpPower, 1, animTime).SetEase(anCurve);
            getSEnemyRect.DOJumpAnchorPos(new Vector3(center + jumpPos.x, jumpPos.y, jumpPos.z), jumpPower, 1, animTime).SetEase(anCurve);
            await Task.Delay(Mathf.RoundToInt(animTime * 1000));
            var hit = Instantiate(woundVFX, getFEnemyRect.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            getFEnemyRect.DOJumpAnchorPos(fEnOldPos, fallingPower, 1, returnTime).SetEase(retCurve);
            getSEnemyRect.DOJumpAnchorPos(sEnOldPos, fallingPower, 1, returnTime).SetEase(retCurve);
            await Task.Delay(1000);
            Destroy(trail1.gameObject);
            Destroy(trail2.gameObject);
            firstEnemy.gameObject.transform.DORotate(Vector3.zero, 0.2f, RotateMode.Fast);
            secondEnemy.gameObject.transform.DORotate(Vector3.zero, 0.2f, RotateMode.Fast);
            firstEnemy.RecieveDamage(attack, this);
            secondEnemy.RecieveDamage(attack, this);

            Destroy(hit, 1f);
        }
        else
        {
            var getFEnemyRect = firstEnemy.transform.parent.GetComponent<RectTransform>();
            var trailSup = Instantiate(trail, firstEnemy.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform).GetComponent<RectTransform>();
            var getEnPos = getFEnemyRect.anchoredPosition;
            trailSup.DOAnchorPos(suplexPos + getFEnemyRect.anchoredPosition, supTime);
            getFEnemyRect.DOAnchorPos(suplexPos + getFEnemyRect.anchoredPosition,supTime);
            await Task.Delay(Mathf.RoundToInt(supTime * 1000));
            var oldRot = firstEnemy.transform.rotation.eulerAngles;
            firstEnemy.gameObject.transform.DORotate(new Vector3(0, 0, 180), 0.2f, RotateMode.Fast);
            await Task.Delay(Mathf.RoundToInt(200));
            trailSup.DOAnchorPos(getEnPos, supTime).SetEase(fallCurve);
            getFEnemyRect.DOAnchorPos(getEnPos, supTime).SetEase(fallCurve);
            await Task.Delay(Mathf.RoundToInt(supTime * 1000));
            var hit = Instantiate(woundVFX, getFEnemyRect.transform.position+spawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            await Task.Delay(Mathf.RoundToInt(200));
            firstEnemy.gameObject.transform.DORotate(oldRot, 0.2f, RotateMode.Fast).SetEase(riseCurve);
            Destroy(hit, 1f);
            Destroy(trailSup.gameObject, 1f);
            firstEnemy.RecieveDamage(attack * 2, this);
        }
    }

}
