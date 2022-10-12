    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;
public class DeathFromAbove : Card
{
    [SerializeField] private GameObject katanaVFX,bladeVFX,trailVFX,crackVFX,hitBlastVFX;
    //smoke?
    [SerializeField] private Vector3 katSpawnOffset, crackSpawnOffset,bladeSpawnOffset;
    [SerializeField]private Vector2 swordOffset,landOffset;
    GameObject trailGO;
    [SerializeField] private float jumpPower,animTime,shakeStrength;
    [SerializeField] private Vector3 rot;
    [SerializeField]private AnimationCurve anCurve,secondaryCurve;
        private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage and add 2 dexterity";
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);
        var secondAttack = Mathf.RoundToInt(attack * 0.3f);
        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage and add 2 dexterity";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage and add 2 dexterity";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage and add 2 dexterity";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
        if (trailVFX != null)
            trailGO.transform.position = player.gameObject.transform.position;

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
                    {
                        gameplayManager.state = BattleState.INANIM;

                        await DoAnim(en);

                        gameplayManager.state = BattleState.PLAYERTURN;
                        en.targeted = false;
                    }

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

    async Task DoAnim(Enemy en)
    {
        //trail?? czy trail z katany ?
        //trailGO = Instantiate(trailVFX, player.gameObject.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var kat = Instantiate(katanaVFX, player.gameObject.transform.position+katSpawnOffset,Quaternion.identity,gameplayManager.vfxCanvas.transform);
        var getRect = kat.GetComponent<RectTransform>();
        getRect.transform.rotation = Quaternion.Euler(rot);
        var getPlayerRect = player.gameObject.GetComponent<RectTransform>();
        getRect.DOJumpAnchorPos(en.transform.parent.GetComponent<RectTransform>().anchoredPosition+swordOffset, jumpPower, 1, animTime).SetEase(anCurve);
        getPlayerRect.DOJumpAnchorPos(en.transform.parent.GetComponent<RectTransform>().anchoredPosition+landOffset, jumpPower, 1, animTime).SetEase(anCurve);
        await Task.Delay(Mathf.RoundToInt(animTime * 1000));


        en.RecieveDamage(attack, this);
        player.setStatusIndicator(2, 5, player.buffIndicators[6]);

        getRect.DOShakeAnchorPos(1, shakeStrength);
        getRect.DOShakeRotation(1, shakeStrength);
        var crack = Instantiate(crackVFX, kat.transform.position+crackSpawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        crack.transform.localScale = new Vector3(150, 150, 1);
        kat.GetComponent<SpriteRenderer>().material.DOFade(0, 2);
        var hit = Instantiate(hitBlastVFX, player.gameObject.transform.position + bladeSpawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        //ZROBIC FADE WZSTKICH ITEMOW
        getPlayerRect.DOJumpAnchorPos(player.myOriginalPosition, jumpPower, 1, animTime).SetEase(secondaryCurve);
        getPlayerRect.DORotate(new Vector3(0, 180, 360), animTime,RotateMode.FastBeyond360).SetEase(secondaryCurve);
        Destroy(crack,3);
        Destroy(hit, 3);
        Destroy(kat, 2);

    }
}


