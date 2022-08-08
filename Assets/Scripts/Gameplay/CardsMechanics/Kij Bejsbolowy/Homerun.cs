using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.VFX;
using DG.Tweening;
public class Homerun :  Card
{
    [SerializeField] private GameObject ball;
    GameObject Baseball;
    [SerializeField]AnimationCurve anCurve;
    [SerializeField] AnimationCurve secondaryCurve;
    [SerializeField]Vector3 offset;
    VisualEffect hitBlastEffect;
    [SerializeField] GameObject hitBlast;
    RectTransform ballRect;
    [SerializeField]Vector3 playerPos;
    private void Start()
    {
        var get03procent = defaultattack * 0.3f;
        playerPos = new Vector3(-40, -17, 0);
        desc = $"Deal <color=white>{attack.ToString()}</color> damage to first enemy and <color=white>{get03procent.ToString()}</color> to second enemy";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;

    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);
        var secondAttack = Mathf.RoundToInt(attack * 0.3f);
        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage to first enemy and <color=white>{secondAttack.ToString()}</color> to second enemy";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage to first enemy and <color=red>{secondAttack.ToString()}</color> to second enemy";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage to first enemy and <color=green>{secondAttack.ToString()}</color> to second enemy";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    public override void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();
            Baseball = Instantiate(ball, playerPos, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            Debug.Log(player.GetComponent<RectTransform>().anchoredPosition);
            ballRect = Baseball.GetComponent<RectTransform>();
            StartCoroutine(ExecuteAfterTime(1f));
            Enemy firstEnemy = null;
            Enemy secondEnemy = null;
            foreach (Enemy en in _enemies)
            {
                if (en.isFirstTarget) firstEnemy = en;
                if (en.isSecondTarget) secondEnemy = en;
            }

                ballRect.DOAnchorPos3D(firstEnemy.transform.parent.GetComponent<RectTransform>().anchoredPosition3D + offset, 0.7f).SetEase(anCurve).OnComplete(() =>
                {
                    //play vfx uderzenia
                    var blast = Instantiate(hitBlast, firstEnemy.transform.parent.GetComponent<RectTransform>().anchoredPosition3D, Quaternion.identity);
                    blast.transform.SetParent(gameplayManager.vfxCanvas.transform);
                    Destroy(blast, 0.3f);
                    firstEnemy.RecieveDamage(attack, this);
                    firstEnemy.targeted = false;
                    firstEnemy.isFirstTarget = false;
                    if (_enemies.Count < 2)
                    {
                        Destroy(Baseball);
                    }
                    else
                    {
                        var mean =Mathf.Abs(secondEnemy.transform.parent.GetComponent<RectTransform>().anchoredPosition.x - firstEnemy.transform.parent.GetComponent<RectTransform>().anchoredPosition.x);
                        Baseball.transform.DORotate(new Vector3(0, 0, 360), 0.6f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
                        ballRect.DOAnchorPos3D(new Vector3(mean,offset.y*4, secondEnemy.transform.parent.GetComponent<RectTransform>().anchoredPosition3D.z), 1.2f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            ballRect.DOAnchorPos(secondEnemy.transform.parent.GetComponent<RectTransform>().anchoredPosition3D + offset, 0.7f).SetEase(anCurve).OnComplete(() =>
                            {
                                //play vfx uderzenia

                                var blast2 = Instantiate(hitBlast, ballRect.anchoredPosition, Quaternion.identity);
                                blast2.transform.SetParent(gameplayManager.canvas.transform);
                                Destroy(blast2, 0.3f);
                                secondEnemy.RecieveDamage(Mathf.RoundToInt((attack) * 0.3f), this); // do zmiany po demie
                                secondEnemy.targeted = false;
                                secondEnemy.isSecondTarget = false;
                                Destroy(Baseball);
                            });
                        });
                    }
                });

            
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

}


