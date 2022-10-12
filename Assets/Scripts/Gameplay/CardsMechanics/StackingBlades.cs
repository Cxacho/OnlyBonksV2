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
        Debug.Log("Stacking blades attack : " + attack);

        desc = $"Deal <color=white>{attack.ToString()}</color>, <color=white>{(attack*2).ToString()}</color>, <color=white>{(attack * 4).ToString()}</color> damage to the same enemy";
        defAttack = Mathf.RoundToInt(defaultattack);
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;

        gameplayManager.OnTurnEnd += ResetValues;
        gameplayManager.OnDraw += ApplyOnDraw;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        var secondAttack = Mathf.RoundToInt(attack * 2);
        var thirdAttack = Mathf.RoundToInt(attack * 4);

        Debug.Log("Stacking blades attack : " + attack);

        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color>, <color=white>{secondAttack.ToString()}</color>, <color=white>{thirdAttack.ToString()}</color> damage to the same enemy";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color>, <color=red>{secondAttack.ToString()}</color>, <color=red>{thirdAttack.ToString()}</color> damage to the same enemy";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color>, <color=green>{secondAttack.ToString()}</color>, <color=green>{thirdAttack.ToString()}</color> damage to the same enemy";
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
                    attack = 2; // Workaround, przywraca attack do poczatkowej wartosci
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
    async Task DoAnim(Enemy en)
    {
        
        for (int i = 0; i < 3; i++)
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
            {
                en.RecieveDamage(attack, this);
                attack *= 2;
            }
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
