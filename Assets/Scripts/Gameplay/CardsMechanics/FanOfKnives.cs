using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Threading.Tasks;
public class FanOfKnives : Card
{
    private TextMeshPro textMeshPro;
    int defAttack;
    [SerializeField] private GameObject hit;
    [SerializeField] private GameObject blade;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] AnimationCurve anCurve;
    [SerializeField] private float height;

    private void Start()
    {
        desc = $"DealDeal <color=white>{attack.ToString()}</color> to enemy three times";
        defAttack = Mathf.RoundToInt(defaultattack);
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> to enemy three times";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> to enemy three times";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> to enemy three times";
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
        for (int i = -1; i < 2; i++)
        {
            if (en == null)
                return;
            var knif = Instantiate(blade, gameplayManager.player.transform.position + spawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            knif.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -25));
            var getRect = knif.GetComponent<RectTransform>();
            var enPos = en.transform.parent.GetComponent<RectTransform>().anchoredPosition;
            getRect.DOAnchorPos(new Vector3(enPos.x, enPos.y + (height * i), 1), 0.7f).SetEase(anCurve).OnComplete(() =>
            {
                var hitBlast = Instantiate(hit, knif.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
                if (en != null)
                    en.RecieveDamage(attack, this);
                else
                {
                    Destroy(knif);
                    return;
                }

                Destroy(knif, 0.6f);
                Destroy(hitBlast, 0.4f);
            });
            

            await Task.Delay(100);

        }
    }

}
