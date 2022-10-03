using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Threading.Tasks;
public class NinjaRun : Card
{
    [SerializeField] private GameObject trailVFX, landingSmoke, stepSmoke;
    [SerializeField] private Vector3 rot1,rot2,smokeOffset,smokeOffset2,smokeOffset3,smokeLandingOffset;
    [SerializeField] private Vector2 move1, move2,move3,landingPos1,landingPos2;
    [SerializeField] private float animTime, anim2Time, rotTime, jumpPower, jumpTime, returnTime, attachPower;
    [SerializeField] private int numOfJumps;
    [SerializeField] private AnimationCurve anCurve;
    GameObject trailToFollow;
    

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
        if (trailToFollow != null)
            trailToFollow.transform.position = player.transform.position;
    }


    public override async void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();

            StartCoroutine(ExecuteAfterTime(1f));

            gameplayManager.state = BattleState.INANIM;
            await DoAnim();
            gameplayManager.state = BattleState.PLAYERTURN;
            gameplayManager.DrawCards(gameplayManager.enemies.Count);

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
    async Task DoAnim()
    {
        //trail
        
        var playerRect = player.gameObject.GetComponent<RectTransform>();
        trailToFollow = Instantiate(trailVFX, player.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
       
        var oldRot = player.gameObject.transform.rotation.eulerAngles;
        
        //
        List<GameObject> temp = new List<GameObject>();
        playerRect.DOJumpAnchorPos(move1,jumpPower,numOfJumps, animTime).SetEase(Ease.Linear);
        for (int i = 0; i < numOfJumps; i++)
        {
            var smoke = Instantiate(stepSmoke, player.transform.position +smokeOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            temp.Add(smoke);
            await Task.Delay(Mathf.RoundToInt(animTime * 1000 /(numOfJumps-1)));
        }
        playerRect.DOAnchorPos(landingPos1, anim2Time).SetEase(Ease.Linear);
        player.transform.DORotate(rot1, rotTime);
        //dorot do sciany
        await Task.Delay(Mathf.RoundToInt(anim2Time * 1000));
        //prawa sciana
        playerRect.DOJumpAnchorPos(move2, jumpPower, numOfJumps-5, animTime).SetEase(Ease.Linear);
        for (int i = 0; i < numOfJumps-5; i++)
        {
            var smoke = Instantiate(stepSmoke, player.transform.position + smokeOffset2, Quaternion.Euler(new Vector3(0,0,90)), gameplayManager.vfxCanvas.transform);
            temp.Add(smoke);
            await Task.Delay(Mathf.RoundToInt(animTime * 1000 / (numOfJumps - 6)));
        }
        playerRect.DOJumpAnchorPos(landingPos2, attachPower, 1, anim2Time).SetEase(Ease.Linear);
        player.transform.DORotate(rot2, anim2Time);
        ///dorot do sufitu
        await Task.Delay(Mathf.RoundToInt(anim2Time * 1000));
        //sufit
        playerRect.DOJumpAnchorPos(move3, jumpPower, numOfJumps-2, animTime).SetEase(Ease.Linear);
        for (int i = 0; i < numOfJumps-2; i++)
        {
            var smoke = Instantiate(stepSmoke, player.transform.position + smokeOffset3, Quaternion.Euler(new Vector3(0, 0, 180)), gameplayManager.vfxCanvas.transform);
            temp.Add(smoke);
            await Task.Delay(Mathf.RoundToInt(animTime * 1000 / (numOfJumps - 3)));
        }
        player.transform.DORotate(oldRot+new Vector3(0,0,-360), returnTime,RotateMode.FastBeyond360);
        playerRect.DOAnchorPos(player.myOriginalPosition, returnTime).SetEase(anCurve).OnComplete(() =>
        {
            var landingSmokeVFX = Instantiate(landingSmoke, player.transform.position + smokeLandingOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            Destroy(landingSmokeVFX,1f);
            Destroy(trailToFollow.gameObject, 1f);
        });
        //usunac smoki
        foreach (GameObject obj in temp)
            Destroy(obj.gameObject,0.5f);
       




    }

}
