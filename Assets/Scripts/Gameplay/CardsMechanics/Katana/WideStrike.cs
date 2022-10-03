using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Threading.Tasks;
public class WideStrike : Card
{
    [SerializeField] private GameObject katanaVFX,bladeVFX,handleVFX,crackVFX;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private Vector3 spawnOffset, hitOffset, jumpOffset,jump2Offset,bladeSpawnOffset,handleSpawnOffset;
    [SerializeField] private Vector3 rot,leftRotation,rightRotation,startingRot;
    [SerializeField] private AnimationCurve anCurve;
    [SerializeField] private float animTime,jumpTime,jumpPower,rotTime;
    [SerializeField]List<GameObject> temporary = new List<GameObject>();
    [SerializeField] List<GameObject> temp1 = new List<GameObject>();


    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
        foreach (Transform obj in gameplayManager.characterCanvas.transform)
                    temp1.Add(obj.gameObject);
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
                    gameplayManager.state = BattleState.INANIM;
                    await DoAnim(en);
                    gameplayManager.state = BattleState.PLAYERTURN;
                    //gameplayManager.OnEnemyKilled += AddMeSomeMana;

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
        var kat = Instantiate(katanaVFX, en.transform.position + spawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var getRect = kat.GetComponent<RectTransform>();
        kat.transform.rotation = Quaternion.Euler(startingRot);
        kat.transform.DORotate(rot, rotTime);
        getRect.DOAnchorPos(en.transform.parent.GetComponent<RectTransform>().anchoredPosition3D, animTime).SetEase(anCurve);
        await Task.Delay(Mathf.RoundToInt(animTime * 1000));
        Destroy(kat);
        var crack = Instantiate(crackVFX, kat.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        en.RecieveDamage(attack, this);
        //instantiate crack
        //if (gameplayManager.enemies[en.transform.parent.GetSiblingIndex() - 1]!=null)
        Destroy(crack, 1);

        foreach (Transform obj in gameplayManager.characterCanvas.transform)
            if (obj.childCount > 0)
                temporary.Add(obj.gameObject);
            else
                temporary.Add(null);
        //Debug.Log(temporary[en.transform.parent.GetSiblingIndex() + 1].name);
        //Debug.Log(temp1[en.transform.parent.GetSiblingIndex() + 1].name);

        if (en.transform.parent.GetSiblingIndex() + 1 <= temporary.Count-2)
        {
            if (temporary[en.transform.parent.GetSiblingIndex() + 1]!=null)
            {
                var blade = Instantiate(bladeVFX, kat.transform.position + bladeSpawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
                var bladeRect = blade.GetComponent<RectTransform>();
                bladeRect.DOJumpAnchorPos(temporary[en.transform.parent.GetSiblingIndex() + 1].GetComponent<RectTransform>().anchoredPosition3D + jumpOffset, jumpPower, 1, jumpTime);
                blade.transform.DORotate(rightRotation, jumpTime, RotateMode.FastBeyond360).OnComplete(() =>
                {
                    var hit1 = Instantiate(hitVFX, blade.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
                    Destroy(hit1, 1);
                    Destroy(blade, 1);
                    blade.GetComponent<SpriteRenderer>().material.DOFade(0, 1);
                    temporary[en.transform.parent.GetSiblingIndex() + 1].transform.GetChild(0).GetComponent<Enemy>().RecieveDamage(0.5f * attack, this);
                });
            }

        }
        //rot
        if (en.transform.parent.GetSiblingIndex() - 1 >= 0)
        {
            if (temporary[en.transform.parent.GetSiblingIndex() - 1]!=null)
            {

                var handle = Instantiate(handleVFX, kat.transform.position + handleSpawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
                var handleRect = handle.GetComponent<RectTransform>();
                handleRect.DOJumpAnchorPos(temporary[en.transform.parent.GetSiblingIndex() - 1].GetComponent<RectTransform>().anchoredPosition3D + jumpOffset, jumpPower, 1, jumpTime);
                handle.transform.DORotate(leftRotation, jumpTime, RotateMode.FastBeyond360).OnComplete(() =>
                {

                    var hit2 = Instantiate(hitVFX, handle.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
                    Destroy(hit2, 1);
                    Destroy(handle, 1);
                    handle.GetComponent<SpriteRenderer>().material.DOFade(0, 1);
                    temporary[en.transform.parent.GetSiblingIndex() - 1].transform.GetChild(0).GetComponent<Enemy>().RecieveDamage(0.5f * attack, this);
                });
            }
        }


    }

}
