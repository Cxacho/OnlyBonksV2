using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using DG.Tweening;

public class FlurryOfBonks : Card
{

    public GameObject batVFX, hitblastVFX;
    [SerializeField] private Vector3 rot1, rot2,rot3, batSpawn1, batSpawn2,batSpawn3, move1, move2,move3, startRot1, startRot2, startRot3;
    [SerializeField] private AnimationCurve anCurve;
    [SerializeField] private float animTime, moveTime,spawnDelay,moveDelay;

    private void Start()
    {
        desc = $"Unleash flurry of attacks dealing <color=white>{attack.ToString()}</color> damage three times.";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Unleash flurry of attacks dealing <color=white>{attack.ToString()}</color> damage three times.";
        else if (attack < defaultattack)
            desc = $"Unleash flurry of attacks dealing <color=red>{attack.ToString()}</color> damage three times.";
        else
            desc = $"Unleash flurry of attacks dealing <color=green>{attack.ToString()}</color> damage three times.";
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

                    //InvokeRepeating(en.ReceiveDamage(attack + pl.strenght), 0.1f, 0.3f);
                    gameplayManager.state = BattleState.INANIM;
                    await DoAnim(en);
                    gameplayManager.state = BattleState.PLAYERTURN;

                    en.targeted = false;
                    en.isFirstTarget = false;
                }
                

            }
            resetTargetting();
        }
        



        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }

    async Task DoAnim(Enemy en)
    {
        var enemyRect = en.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition3D;
        //1
        
        var bat1 = Instantiate(batVFX, en.transform.parent.transform.position + batSpawn1, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        bat1.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
        bat1.GetComponent<SpriteRenderer>().material.DOFade(1, 1);
        bat1.transform.rotation = Quaternion.Euler(startRot1);
        //2
        await Task.Delay((int)spawnDelay);
        var bat2 = Instantiate(batVFX, en.transform.parent.transform.position + batSpawn2, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        bat2.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
        bat2.GetComponent<SpriteRenderer>().material.DOFade(1, 1);
        bat2.transform.rotation = Quaternion.Euler(startRot2);
        //3
        await Task.Delay((int)spawnDelay);
        var bat3 = Instantiate(batVFX, en.transform.parent.transform.position + batSpawn3, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        bat3.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
        bat3.GetComponent<SpriteRenderer>().material.DOFade(1, 1);
        bat3.transform.rotation = Quaternion.Euler(startRot3);
        await Task.Delay(1000);
        //move1
        var getRect1 = bat1.GetComponent<RectTransform>();
        bat1.transform.DORotate(rot1, animTime, RotateMode.Fast).SetEase(anCurve);
        getRect1.DOAnchorPos(move1 + enemyRect, animTime).SetEase(anCurve);
        var enPos = en.transform.parent.transform.position;

        await Task.Delay((int)moveDelay);
        en.RecieveDamage(attack, this);
        var hit1 = Instantiate(hitblastVFX, enPos, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        //move2
        var getRect2 = bat2.GetComponent<RectTransform>();
        bat2.transform.DORotate(rot2, moveTime, RotateMode.Fast).SetEase(anCurve);
        getRect2.DOAnchorPos(move2 + enemyRect, moveTime).SetEase(anCurve);


        await Task.Delay((int)moveDelay);
        if (en != null)
            en.RecieveDamage(attack, this);
        var hit2 = Instantiate(hitblastVFX, enPos, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        //move3
        var getRect3 = bat3.GetComponent<RectTransform>();
        bat3.transform.DORotate(rot3, moveTime, RotateMode.Fast).SetEase(anCurve);
        getRect3.DOAnchorPos(move3 + enemyRect, moveTime).SetEase(anCurve);

        await Task.Delay(400);
        if (en != null)
        en.RecieveDamage(attack, this);
        var hit3 = Instantiate(hitblastVFX, enPos, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        bat3.GetComponent<SpriteRenderer>().material.DOFade(0, 1);
        bat1.GetComponent<SpriteRenderer>().material.DOFade(0, 1);
        bat2.GetComponent<SpriteRenderer>().material.DOFade(0, 1);
        //recieve damage
        Destroy(bat1, 1f);
        Destroy(bat2, 1f);
        Destroy(hit1, 1);
        Destroy(hit2, 1);
        Destroy(bat3, 1f);
        Destroy(hit3, 1);
    }


    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);


        //enemy.ReceiveDamage(attack * pl.strenght);


        player.manaText.text = player.mana.ToString();


        /* else
         {
             // enemy.currentHealth = 0;
             gameplayManager.state = BattleState.WON;
             StartCoroutine(gameplayManager.OnBattleWin());

         }*/





    }

}