using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Threading.Tasks;
public class StackingBlades : Card
{
    [SerializeField]int cardsDrawn = 0;
    private TextMeshPro textMeshPro;
    int defAttack;
    [SerializeField] private GameObject hit;
    [SerializeField] private GameObject blade;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float animTime;
    [SerializeField] AnimationCurve anCurve;

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage";
        defAttack = Mathf.RoundToInt(defaultattack);
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
        gameplayManager.OnDraw += ApplyOnDraw;
        gameplayManager.OnTurnEnd += ResetValues;
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
                    await Doanim(en);

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
    void ResetValues(object sender, EventArgs e)
    {
        cardsDrawn = 0;
        this.defaultattack = defAttack;
    }
    void ApplyOnDraw(object sender,EventArgs e)
    {
        cardsDrawn++;
        this.defaultattack += 3;
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        player.manaText.text = player.mana.ToString();

    }
    private void OnDestroy()
    {
        gameplayManager.OnTurnEnd -= ResetValues;
        gameplayManager.OnDraw -= ApplyOnDraw;
    }
    async Task Doanim(Enemy en)
    {
        
        for (int i = 0; i < cardsDrawn; i++)
        {
            //random.range dla wysoko?ci rzutu
            var knif = Instantiate(blade, gameplayManager.player.transform.position + offset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            knif.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -25));
            var getRect = knif.GetComponent<RectTransform>();
            getRect.DOAnchorPos(en.transform.parent.GetComponent<RectTransform>().anchoredPosition, animTime / cardsDrawn).SetEase(anCurve);
            var calc = Mathf.RoundToInt(animTime / cardsDrawn*1000);
            await Task.Delay(calc);
            var hitBlast = Instantiate(hit, knif.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            //jakis efekt uderzenia przeciwnika
            //dodac damage
            Destroy(knif);
            Destroy(hitBlast, 0.4f);

        }
    }

}
