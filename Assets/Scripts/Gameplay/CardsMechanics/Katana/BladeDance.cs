using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;
public class BladeDance : Card
{
    [SerializeField] private GameObject katanaVFX, bladeVFX, trailVFX, crackVFX, hitBlastVFX;
    //smoke?
    [SerializeField] private Vector3 katSpawnOffset,rot;
    [SerializeField] private Vector2  sword1LandOffset,jumpOffset,fallOffset,moveOffset,trailPos;
    [SerializeField] private float jumpPower, animTime, jumpTime,fallPower,fallTime,returnTime,skipPower;
    [SerializeField] private AnimationCurve lungeCurve, jumpCurve,fallCurve,rotEase;
    List<GameObject> stations = new List<GameObject>();
    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color>, <color=white>{Mathf.RoundToInt(attack * 0.7f).ToString()}</color>, <color=white>{Mathf.RoundToInt(attack * 0.4f).ToString()}</color> damage in sequence up to 3 enemies. Reduce cost by 1 if there are 3 enemies selected";
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);
        var secondAttack = Mathf.RoundToInt(attack * 0.7f);
        var thirdAttack = Mathf.RoundToInt(attack * 0.4f);
        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color>, <color=white>{secondAttack.ToString()}</color>, <color=white>{thirdAttack.ToString()}</color> damage in sequence up to 3 enemies. Reduce cost by 1 if there are 3 enemies selected";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color>, <color=white>{secondAttack.ToString()}</color>, <color=white>{thirdAttack.ToString()}</color> damage in sequence up to 3 enemies. Reduce cost by 1 if there are 3 enemies selected";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color>, <color=white>{secondAttack.ToString()}</color>, <color=white>{thirdAttack.ToString()}</color> damage in sequence up to 3 enemies. Reduce cost by 1 if there are 3 enemies selected";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;

    }

    public override async void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();


            //ui.DisableButtons(getPanel);
            await DoAnim();
            foreach (Enemy en in _enemies)
            {
                if (en.isFirstTarget == true)
                {
                    {
                        //recieve damage 100/70/40
                        en.targeted = false;
                        en.RecieveDamage(attack,this);
                    }

                }
                if (en.isSecondTarget == true)
                {
                    {
                        en.RecieveDamage(attack*0.7f, this);
                        en.targeted = false;
                    }

                }
                if (en.isThirdTarget == true)
                {
                    {
                        en.RecieveDamage(attack * 0.4f, this);
                        en.targeted = false;
                    }

                }
                
            }
            if (gameplayManager.enemies.Count > 2)
            {
                player.mana += 1;
                player.manaText.text = player.mana.ToString();
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


    async Task DoAnim()
    {
        foreach (Transform obj in gameplayManager.characterCanvas.transform)
            stations.Add(obj.gameObject);
        var trail= Instantiate(trailVFX, player.gameObject.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var trailRect=trail.GetComponent<RectTransform>();
        var kat = Instantiate(katanaVFX, player.gameObject.transform.position + katSpawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        kat.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        var getPlayerRect = player.gameObject.GetComponent<RectTransform>();
        var katRect = kat.GetComponent<RectTransform>();
        trailRect.DOAnchorPos(stations[0].GetComponent<RectTransform>().anchoredPosition + moveOffset+trailPos, animTime).SetEase(lungeCurve);
        getPlayerRect.DOAnchorPos(stations[0].GetComponent<RectTransform>().anchoredPosition+moveOffset, animTime).SetEase(lungeCurve);
        katRect.DOAnchorPos(stations[0].GetComponent<RectTransform>().anchoredPosition+sword1LandOffset+moveOffset, animTime).SetEase(lungeCurve);
        await Task.Delay(Mathf.RoundToInt(animTime * 1000));
        var hit1 = Instantiate(hitBlastVFX, stations[0].transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        //vfx
        await Task.Delay(Mathf.RoundToInt(200));
        trailRect.DOJumpAnchorPos(stations[1].GetComponent<RectTransform>().anchoredPosition + jumpOffset + trailPos, jumpPower, 1, jumpTime).SetEase(jumpCurve);
        getPlayerRect.DOJumpAnchorPos(stations[1].GetComponent<RectTransform>().anchoredPosition+jumpOffset, jumpPower, 1, jumpTime).SetEase(jumpCurve);
        katRect.DOJumpAnchorPos(stations[1].GetComponent<RectTransform>().anchoredPosition + jumpOffset+sword1LandOffset, jumpPower, 1, jumpTime).SetEase(jumpCurve);
        kat.transform.DORotate(new Vector3(0, 0, 90), jumpTime, RotateMode.Fast);
        await Task.Delay(Mathf.RoundToInt(jumpTime*0.3f * 1000));
        var hit2 = Instantiate(hitBlastVFX, stations[1].transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        await Task.Delay(Mathf.RoundToInt(jumpTime * 0.7f * 1000));
        //rot ostrza
        //vfx
        trailRect.DOAnchorPos(stations[2].GetComponent<RectTransform>().anchoredPosition + fallOffset + trailPos, fallTime).SetEase(fallCurve);
        getPlayerRect.DOAnchorPos(stations[2].GetComponent<RectTransform>().anchoredPosition + fallOffset, fallTime).SetEase(fallCurve);
        katRect.DOAnchorPos(stations[2].GetComponent<RectTransform>().anchoredPosition + fallOffset + sword1LandOffset, fallTime).SetEase(fallCurve);
        kat.transform.DORotate(rot, jumpTime, RotateMode.Fast).SetEase(rotEase);
        await Task.Delay(Mathf.RoundToInt(fallTime * 1000));
        var hit3 = Instantiate(hitBlastVFX, stations[2].transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        katRect.gameObject.GetComponent<SpriteRenderer>().material.DOFade(0, fallTime);
        getPlayerRect.DOAnchorPos(getPlayerRect.anchoredPosition + new Vector2(400, 0), 0.3f);
        await Task.Delay(300);
        getPlayerRect.anchoredPosition = new Vector2(-1050, getPlayerRect.anchoredPosition.y);
        getPlayerRect.DOJumpAnchorPos(player.myOriginalPosition, skipPower, 7, returnTime);
        Destroy(kat,1);
        Destroy(hit1);
        Destroy(hit2);
        Destroy(hit3,1f);
        Destroy(trailRect.gameObject);

        //playhitblast

    }
}


