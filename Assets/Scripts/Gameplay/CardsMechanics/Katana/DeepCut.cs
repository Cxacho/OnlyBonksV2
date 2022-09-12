using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Threading.Tasks;
public class DeepCut : Card
{
    [SerializeField] private GameObject katanaHandle,katanaBlade,katana,shaderBlood,bloodBurst;
    [SerializeField] private Vector3 spawnOffset, jumpPos, hitOffset;
    [SerializeField] private Vector3 rot,originalRot;
    [SerializeField] private float jumpPower,animTime;
    [SerializeField] private AnimationCurve anCurve;
    private TextMeshPro textMeshPro;

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
                    en.RecieveDamage(attack, this);
                    en.setStatus(Enemy.statuses.bleeding, 2, en);
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

        var kat = Instantiate(katana, spawnOffset + en.transform.parent.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var getKatRect = kat.GetComponent<RectTransform>();
        getKatRect.transform.rotation = Quaternion.Euler(originalRot);
        getKatRect.DOJumpAnchorPos(en.transform.parent.GetComponent<RectTransform>().anchoredPosition, jumpPower,1,animTime).SetEase(anCurve);
        getKatRect.DORotate(rot, animTime);
        await Task.Delay(Mathf.RoundToInt(animTime*1000));

        var burst = Instantiate(bloodBurst, kat.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var shader = Instantiate(shaderBlood, kat.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var getRenderer = shader.GetComponent<SpriteRenderer>();
        getRenderer.DOFade(0, 2.2f);
        Destroy(kat,0.7f);
        Destroy(burst,1.5f);
        Destroy(shader,2.2f);
    }

}
