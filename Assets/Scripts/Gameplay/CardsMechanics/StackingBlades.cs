using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Threading.Tasks;
public class StackingBlades : Card
{
    [SerializeField]int cardsDrawn = 1;
    private TextMeshPro textMeshPro;
    int defAttack;
    [SerializeField] private GameObject hit;
    [SerializeField] private GameObject blade;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float animTime;
    [SerializeField] AnimationCurve anCurve;
    [SerializeField] Vector2 randomizeHeight;

    private void Start()
    {
        desc = $"For each card drawn deal <color=white>{attack.ToString()}</color> damage to enemy. Current :"+(cardsDrawn +1);
        defAttack = Mathf.RoundToInt(defaultattack);
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;

        gameplayManager.OnTurnEnd += ResetValues;
        gameplayManager.OnDraw += ApplyOnDraw;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"For each card drawn deal <color=white>{attack.ToString()}</color> damage to enemy. Current cards drawn :" + cardsDrawn;
        else if (attack < defaultattack)
            desc = $"For each card drawn deal <color=red>{attack.ToString()}</color> damage to enemy. Current cards drawn :" + cardsDrawn;
        else
            desc = $"For each card drawn deal <color=green>{attack.ToString()}</color> damage to enemy. Current cards drawn :" + cardsDrawn ;
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
        cardsDrawn = 1;
        this.defaultattack = defAttack;
    }
    void ApplyOnDraw(object sender,EventArgs e)
    {
        cardsDrawn++;
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
            if (en == null)
                return;
            //random.range dla wysoko?ci rzutu
            var randomHeight=UnityEngine.Random.Range(randomizeHeight.x, randomizeHeight.y);
            var knif = Instantiate(blade, gameplayManager.player.transform.position + offset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            knif.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -25));
            var getRect = knif.GetComponent<RectTransform>();
            var enPos = en.transform.parent.GetComponent<RectTransform>().anchoredPosition;
            getRect.DOAnchorPos(new Vector3(enPos.x,enPos.y+randomHeight,1), animTime / cardsDrawn).SetEase(anCurve);
            var calc = Mathf.RoundToInt(animTime / cardsDrawn*1000);
            await Task.Delay(calc);
            var hitBlast = Instantiate(hit, knif.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            if (en != null)
                en.RecieveDamage(attack, this);
            else
            {
                Destroy(knif);
                return;
            }
            
            //dodac damage
            
            Destroy(knif);
            Destroy(hitBlast, 0.4f);

        }
    }

}
