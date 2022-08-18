using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Threading.Tasks;
public class SupaKicka : Card
{
    [SerializeField] private GameObject trail;
    [SerializeField] private GameObject smokeVFX, smokeVFX2,hitVFX;
    [SerializeField] private Vector2 jumpPos;
    [SerializeField] private Vector3 rot, smokeOffset;
    [SerializeField] private AnimationCurve anCurve, strikeCurve;
    [SerializeField] private float jumpAnimTime, jumpPower,fallSpeed;
    private TextMeshPro textMeshPro;
    [SerializeField]private RectTransform trailGORect,playerRect;

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
        playerRect = player.gameObject.GetComponent<RectTransform>();
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
        if (trailGORect != null)
            trailGORect.anchoredPosition = playerRect.anchoredPosition;
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
                    //gameplayManager.OnEnemyKilled += AddMeSomeMana;
                    en.RecieveDamage(attack, this);

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
        //instantiate trail
        trailGORect = Instantiate(trail, player.gameObject.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform).GetComponent<RectTransform>();
        //var getPlayerRect=player.gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        var getPlayerRect = player.gameObject.GetComponent<RectTransform>();
        //ruch lokalny?
        var oldRot = getPlayerRect.transform.rotation.eulerAngles;
        var smok = Instantiate(smokeVFX, player.gameObject.transform.position + smokeOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        getPlayerRect.DOLocalRotate(rot, jumpAnimTime,RotateMode.FastBeyond360).SetEase(anCurve);
        getPlayerRect.DOJumpAnchorPos(jumpPos, jumpPower, 1, jumpAnimTime);
        await Task.Delay(Mathf.RoundToInt((jumpAnimTime+0.1f) * 1000));
        getPlayerRect.DOAnchorPos(en.transform.parent.GetComponent<RectTransform>().anchoredPosition, fallSpeed).SetEase(strikeCurve);
        await Task.Delay(Mathf.RoundToInt(fallSpeed*1000));

        var secondSmok = Instantiate(smokeVFX2, player.gameObject.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var hit = Instantiate(hitVFX, player.gameObject.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        getPlayerRect.DOLocalRotate(new Vector3(oldRot.x,oldRot.y,oldRot.z-720), jumpAnimTime*1.5f, RotateMode.FastBeyond360).SetEase(anCurve);
        getPlayerRect.DOJumpAnchorPos(player.myOriginalPosition, jumpPower * 20, 1, jumpAnimTime * 1.5f).OnComplete(() =>
        {
            var vfx= Instantiate(smokeVFX, player.gameObject.transform.position+smokeOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            Destroy(vfx,1.5f);
        });
        Destroy(smok,1.5f);
        Destroy(hit, 1.5f);
        Destroy(secondSmok, 1.5f);
        Destroy(trailGORect.gameObject);
        trailGORect = null;


    }

}
