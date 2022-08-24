using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;

public class DoubleBonk : Card
{
    public GameObject batVFX,hitblastVFX,smokeVFX;
    [SerializeField] private Vector3 rot1, rot2, batSpawn1, batSpawn2, move1, move2,startRot1,startRot2;
    [SerializeField] private AnimationCurve anCurve;
    [SerializeField]private float animTime,moveTime;


    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage twice";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage twice";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage twice";
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
                if(en.targeted)
                 await Doanim(en);
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


        //enemy.ReceiveDamage(attack * pl.strenght);


        player.manaText.text = player.mana.ToString();


        /* else
         {
             // enemy.currentHealth = 0;
             gameplayManager.state = BattleState.WON;
             StartCoroutine(gameplayManager.OnBattleWin());

         }*/





    }
    private void OnEnable()
    {
        Doanim(null);
    }

    async  Task  Doanim(Enemy en)
    {
       var enemyRect  =en.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition3D;
        var smoke1=Instantiate(smokeVFX, en.transform.parent.transform.position + batSpawn1,Quaternion.identity, gameplayManager.vfxCanvas.transform);
        await Task.Delay(200);
        var bat1 = Instantiate(batVFX, en.transform.parent.transform.position+batSpawn1, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        bat1.transform.rotation = Quaternion.Euler(startRot1);
        var getRect1 = bat1.GetComponent<RectTransform>();
        bat1.transform.DORotate(rot1, animTime,RotateMode.Fast).SetEase(anCurve);
        getRect1.DOAnchorPos(move1+enemyRect, animTime).SetEase(anCurve);
        await Task.Delay(Mathf.RoundToInt(animTime * 1000));
        en.RecieveDamage(attack, this);
        var hit1 = Instantiate(hitblastVFX, en.transform.parent.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        bat1.GetComponent<SpriteRenderer>().material.DOFade(0, 1);
        //zrobic warunek dla zabicia przeciwnika
        await Task.Delay(200);
            var smoke2 = Instantiate(smokeVFX, en.transform.parent.transform.position + batSpawn2, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            var bat2 = Instantiate(batVFX, en.transform.parent.transform.position + batSpawn2, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            bat2.transform.rotation = Quaternion.Euler(startRot2);
            var getRect2 = bat2.GetComponent<RectTransform>();
            bat2.transform.DORotate(rot2, moveTime, RotateMode.Fast).SetEase(anCurve);
            getRect2.DOAnchorPos(move2 + enemyRect, moveTime).SetEase(anCurve);
            await Task.Delay(Mathf.RoundToInt(moveTime * 0.7f * 1000));
            en.transform.DORotate(new Vector3(0, 0, -360), moveTime, RotateMode.FastBeyond360);
            await Task.Delay(Mathf.RoundToInt(moveTime * 0.3f * 1000));
            en.RecieveDamage(attack, this);
            bat2.GetComponent<SpriteRenderer>().material.DOFade(0, 1);
            var hit2 = Instantiate(hitblastVFX, en.transform.parent.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            //recieve damage
            Destroy(bat1, 1f);
            Destroy(bat2, 1f);
            Destroy(smoke1, 1f);
            Destroy(smoke2, 1f);
            Destroy(hit1, 1);
            Destroy(hit2, 1);
    }

}