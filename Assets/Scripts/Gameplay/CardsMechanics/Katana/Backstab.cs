using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Threading.Tasks;
public class Backstab : Card
{
    private TextMeshPro textMeshPro;
    [SerializeField] private Vector3 offset,startRot,knifSpawnOffset;
    [SerializeField] private GameObject smoke;
    [SerializeField] private GameObject bleed,knifVFX,woundVFX;
    [SerializeField] private float returnTime,knifMoveTime;
    [SerializeField] private Vector2 moveOffset;
    [SerializeField] private AnimationCurve anCurve;
    List<Transform> characters = new List<Transform>();

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
                    if (characters[getEnemyIndex + 1].gameObject == gameplayManager.player.gameObject || characters[getEnemyIndex + 1].childCount == 0)
                    {
                        gameplayManager.state = BattleState.INANIM;
                        await DoAnim(en);
                        gameplayManager.state = BattleState.PLAYERTURN;
                        en.RecieveDamage(attack * 2, this);
                    }
                    else
                    {
                        gameplayManager.state = BattleState.INANIM;
                        await DoAnim(en);
                        gameplayManager.state = BattleState.PLAYERTURN;
                        en.RecieveDamage(attack, this);
                    }

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
    private void OnEnable()
    {
        foreach (Transform obj in gameplayManager.characterCanvas.transform)
        {
            characters.Add(obj);
        }
    }

    async Task DoAnim(Enemy _enemy)
    {
        var smokeVFX = Instantiate(smoke, gameplayManager.player.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        await Task.Delay(1000);
        var smokeVFX2 = Instantiate(smoke, gameplayManager.player.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        await Task.Delay(100);
        var oldPos = gameplayManager.player.transform.position;
        gameplayManager.player.transform.position = _enemy.transform.position + offset;
        var rect = gameplayManager.player.GetComponent<RectTransform>();
        var oldRot = rect.rotation.eulerAngles;
        rect.DORotate(Vector3.zero, 0.1f, RotateMode.Fast);
        var knif = Instantiate(knifVFX, player.gameObject.transform.position+knifSpawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        knif.transform.rotation = Quaternion.Euler(startRot);
        knif.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
        knif.GetComponent<SpriteRenderer>().material.DOFade(1, 0.5f);
        knif.GetComponent<RectTransform>().DOAnchorPos(player.gameObject.GetComponent<RectTransform>().anchoredPosition + moveOffset, knifMoveTime).SetEase(anCurve);
        await Task.Delay(Mathf.RoundToInt(knifMoveTime * 1000));
        var wound = Instantiate(woundVFX, knif.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        
        await Task.Delay(Mathf.RoundToInt(returnTime * 1000));
        knif.GetComponent<SpriteRenderer>().material.DOFade(0, 0.7f);
        gameplayManager.player.transform.position = oldPos;
        rect.DORotate(oldRot, 0.2f, RotateMode.Fast);
        Destroy(smokeVFX,1);
        Destroy(smokeVFX2, 1);
        Destroy(wound,1);
        Destroy(knif,1);

    }



}
