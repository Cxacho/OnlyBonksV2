using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Threading.Tasks;
public class Slide : Card
{
    [SerializeField] private GameObject trailVFX,sparksVFX;
    [SerializeField] private Vector3 startRot;
    [SerializeField] private AnimationCurve anCurve;
    [SerializeField] private float animTime, swayMoveTime,rotTime,jumpPower,jumpTime,returnTime,jumpPower2;
    [SerializeField] private int armorValue=3;

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
            await DoAnim();
            gameplayManager.DrawCards(gameplayManager.enemies.Count);
            player.GetArmor(gameplayManager.enemies.Count * armorValue);

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

    async Task DoAnim()
    {
        List<GameObject> temp = new List<GameObject>();
        //trail
        var oldRot = player.transform.rotation.eulerAngles;
        foreach (Transform obj in gameplayManager.characterCanvas.transform)
            temp.Add(obj.gameObject);
        var playerRect=player.gameObject.GetComponent<RectTransform>();

        //
        playerRect.DOAnchorPos(temp[temp.Count - 2].GetComponent<RectTransform>().anchoredPosition + new Vector2(20, -120), animTime);
        var sparks = Instantiate(sparksVFX, player.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var trail = Instantiate(trailVFX, player.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var getSparkRect = sparks.GetComponent<RectTransform>();
        var getTrailRect = trail.GetComponent<RectTransform>();
        getSparkRect.DOAnchorPos(temp[temp.Count - 2].GetComponent<RectTransform>().anchoredPosition + new Vector2(20, -120), animTime);
        getTrailRect.DOAnchorPos(temp[temp.Count - 2].GetComponent<RectTransform>().anchoredPosition + new Vector2(20, -120), animTime);
        sparks.SetActive(false);
        await Task.Delay(Mathf.RoundToInt(swayMoveTime*1000));
        sparks.SetActive(true);

        playerRect.DORotate(startRot, swayMoveTime, RotateMode.Fast).SetEase(anCurve);
        await Task.Delay(Mathf.RoundToInt(rotTime * 1000));
        Destroy(sparks);
        playerRect.DORotate(oldRot, 0.3f, RotateMode.Fast);
        playerRect.DOJumpAnchorPos(playerRect.anchoredPosition + new Vector2(250, 40),jumpPower,1, jumpTime);
        await Task.Delay(Mathf.RoundToInt(jumpTime * 1000));
        player.transform.position = new Vector3(-150, player.transform.position.y, player.transform.position.z);
        playerRect.DOJumpAnchorPos(player.myOriginalPosition, jumpPower2, 4, returnTime);
        Destroy(trail,1);


    }

}
