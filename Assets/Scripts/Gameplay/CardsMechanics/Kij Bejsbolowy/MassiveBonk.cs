using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MassiveBonk : Card
{
    public GameObject bonk;
    [SerializeField] private GameObject eruption;
    [SerializeField] private GameObject bigBonk;
    [SerializeField] private AnimationCurve anCurve;
    [SerializeField]Vector3 offset, eruptionOffset;
    [SerializeField] Vector2  fallingOffset;
    private void Start()
    {
        desc = $"Summon a huge bat dealing <color=white>{attack.ToString()}</color> damage to the enemy";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Summon a huge bat dealing <color=white>{attack.ToString()}</color> damage to the enemy";
        else if (attack < defaultattack)
            desc = $"Summon a huge bat dealing <color=red>{attack.ToString()}</color> damage to the enemy";
        else
            desc = $"Summon a huge bat dealing <color=green>{attack.ToString()}</color> damage to the enemy";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }

    public override void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();

            /*
            gameplayManager.state = BattleState.INANIM;

            await DoAnim();
            gameplayManager.state = BattleState.PLAYERTURN;
            */

            StartCoroutine(ExecuteAfterTime(1f));
            
            foreach (Enemy en in _enemies)
            {

                if (en.targeted == true)
                {
                    var hit = Instantiate(bigBonk, en.transform.parent.transform.position + offset, Quaternion.identity);
                    hit.transform.SetParent(gameplayManager.vfxCanvas.transform);
                    hit.transform.localScale = Vector3.one;
                    var rect = hit.GetComponent<RectTransform>();
                    rect.DOAnchorPos(en.transform.parent.GetComponent<RectTransform>().anchoredPosition + fallingOffset, 1f).SetEase(anCurve).OnComplete(() =>
                    {
                        var blow = Instantiate(eruption, rect.transform.position + eruptionOffset, Quaternion.identity ,gameplayManager.vfxCanvas.transform);
                        en.RecieveDamage(attack, this);
                        Destroy(blow, 1f);
                        Destroy(hit, 1f);
                    });
                    
                    

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

