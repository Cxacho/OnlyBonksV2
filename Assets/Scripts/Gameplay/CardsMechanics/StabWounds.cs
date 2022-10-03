using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Threading.Tasks;
public class StabWounds : Card
{
    private TextMeshPro textMeshPro;
    [SerializeField] private Vector3 secondRot, startRot, knifSpawnOffset, moveOffset, stabOffset, firstStop,vfxSpawnOffset;
    [SerializeField] private GameObject knifVFX, woundVFX;
    [SerializeField] private float stabTime,moveTime,rotTime,travelTime;
    [SerializeField] private Vector3  secondMovePos;
    [SerializeField] private AnimationCurve anCurve;
    List<GameObject> objs = new List<GameObject>(); 

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage, if enemy has their back exposed, deal double damage";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;


    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage, if enemy has their back exposed, deal double damage";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage, if enemy has their back exposed, deal double damage";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage, if enemy has their back exposed, deal double damage";
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
                    //gameplayManager.OnEnemyKilled += AddMeSomeMana;
                    //dociagnonc mechanike "back exposed " np gdy zabijemy przeciwnika na srodku, zrobic to porprzz sprawdzenie czy istnieje nastepny enemy spawner pos, i czy ma dzieci
                    var getEnemyIndex = en.transform.parent.GetSiblingIndex();
                    /*
                if (en.gameObject == gameplayManager.enemies[gameplayManager.enemies.Count - 1])
                {
                    await DoAnim(en);
                    en.RecieveDamage(attack * 2, this);
                }
                */

                    gameplayManager.state = BattleState.INANIM;
                    await DoAnim(en);
                    gameplayManager.state = BattleState.PLAYERTURN;
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


    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        player.manaText.text = player.mana.ToString();

    }


    async Task DoAnim(Enemy _enemy)
    {
        var enPos=_enemy.transform.parent.GetComponent<RectTransform>().anchoredPosition3D;
        var knif = Instantiate(knifVFX, _enemy.transform.position + knifSpawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        knif.transform.rotation = Quaternion.Euler(startRot);
        var stabRect=knif.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        var knifRect = knif.gameObject.GetComponent<RectTransform>();
        knifRect.DOAnchorPos(firstStop+enPos, travelTime).SetEase(Ease.Linear);
        await Task.Delay(Mathf.RoundToInt(travelTime * 1000));
        knif.transform.DORotate(secondRot, rotTime);
        await Task.Delay(Mathf.RoundToInt(rotTime * 1000));
        var tween=stabRect.DOAnchorPos(stabOffset, stabTime).SetLoops(-1,LoopType.Yoyo).SetEase(anCurve);
        knifRect.DOAnchorPos(moveOffset + enPos, moveTime).SetEase(Ease.Linear);
        AnimHelper(knif);
        await Task.Delay(Mathf.RoundToInt(moveTime * 1000));
        tween.Kill();
        knif.transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(0, 1).OnComplete(()=>
        {
            foreach (GameObject obj in objs)
                Destroy(obj);
            Destroy(knif);
        });
        //kill tween

    }
    async Task AnimHelper(GameObject knif)
    {
        for(int i =0;i<28;i++)
        {
            objs.Add(Instantiate(woundVFX, knif.transform.position + vfxSpawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform));
            await Task.Delay(Mathf.RoundToInt(stabTime * 1000*2));
        }
    }

}
