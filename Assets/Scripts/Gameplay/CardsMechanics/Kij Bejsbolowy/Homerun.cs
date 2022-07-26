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
    private void Start()
    {
        var get03procent = defaultattack * 0.3f;

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
            Baseball = Instantiate(ball, player.transform.position + new Vector3(4,7,0), Quaternion.identity);
            Baseball.transform.SetParent(gameplayManager.canvas.transform);
            Baseball.transform.localScale = new Vector3(15,15,15);
            StartCoroutine(ExecuteAfterTime(1f));
            Enemy firstEnemy = null;
            Enemy secondEnemy = null;
            foreach (Enemy en in _enemies)
            {
                if (en.isFirstTarget) firstEnemy = en;
                if (en.isSecondTarget) secondEnemy = en;
            }

                Baseball.transform.DOMove(firstEnemy.transform.position+ offset, 0.5f).SetEase(anCurve).OnComplete(() =>
                {
                    //play vfx uderzenia
                    var blast = Instantiate(hitBlast, Baseball.transform.position, Quaternion.identity);
                    blast.transform.SetParent(gameplayManager.canvas.transform);
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
                        var mean =Mathf.Abs(secondEnemy.transform.position.x - firstEnemy.transform.position.x);
                        Debug.Log(secondEnemy.transform.position);
                        Debug.Log(firstEnemy.transform.position);
                        Baseball.transform.DORotate(new Vector3(0, 0, 360), 0.6f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
                        Baseball.transform.DOMove(new Vector3(mean,offset.y*4, secondEnemy.transform.position.z), 1.2f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            Baseball.transform.DOMove(secondEnemy.transform.position + offset, 0.7f).SetEase(anCurve).OnComplete(() =>
                            {
                                //play vfx uderzenia

                                var blast2 = Instantiate(hitBlast, Baseball.transform.position, Quaternion.identity);
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


