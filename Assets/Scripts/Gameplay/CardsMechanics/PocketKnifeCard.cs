using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class PocketKnifeCard : Card
{
    //Deals 3 damage and applies 3 bleed
    [SerializeField] private GameObject knifeAnimPrefab;
    [SerializeField] private GameObject bleedVfx;
    [SerializeField] private Vector3 offset;
    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage, and apply 3 bleed";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage and apply 3 bleed";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage and apply 3 bleed";
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

            var knifeObj = Instantiate(knifeAnimPrefab, gameplayManager.player.transform.position + offset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            knifeObj.transform.localScale = new Vector3(45, 45, 1);
            var getRect = knifeObj.GetComponent<RectTransform>();
            
            //Instantiate(bonk, new Vector3(0, -10, 0), Quaternion.identity, GameObject.Find("Player").transform);
            StartCoroutine(ExecuteAfterTime(1f));
            foreach (Enemy en in _enemies)
            {
                if (en.targeted == true)
                {
                    var enRect = en.GetComponent<RectTransform>();
                    getRect.DORotate(new Vector3(0, 0, 360), 0.6f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
                    getRect.DOAnchorPos(enRect.transform.parent.GetComponent<RectTransform>().anchoredPosition, 0.7f).OnComplete(() =>
                    {
                        var vfxToDestroy=Instantiate(bleedVfx, en.transform.parent.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
                        //rescale ??
                        en.RecieveDamage(attack, this);
                        en.setStatus(Enemy.statuses.bleeding, 2, en);
                        en.targeted = false;
                        Destroy(knifeObj,0.2f);
                        Destroy(vfxToDestroy,1.5f);
                    });

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


    }
}
