using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;

public class Ram : Card
{   //Deal 15 damage to first enemy, then deal damage to rest of enemies. Each consecutive attack is halved
    [SerializeField] private Vector3 smokeSpawnOffset,woundSpawnOffset;
    [SerializeField] private GameObject smokeVFX,chargeVFX,woundVFX;
    List<RectTransform> enRects = new List<RectTransform>();
    List<Vector2> enPositions = new List<Vector2>();
    [SerializeField] private AnimationCurve anCurve,secAnCurve;
    [SerializeField] private float animTime, knockBackTime, returnTime, jumpPower;
    [SerializeField] private int numOfJumps; 
    [SerializeField]Vector2 moveOffset,ramOffset;

    private void Start()
    {
        desc = $"Deal <color=white>{attack.ToString()}</color> damage to first enemy, then deal damage to rest of enemies. Each consecutive attack will be halved";
    }

    private void FixedUpdate()
    {

        
        if (attack == defaultattack)
            desc = $"Deal <color=white>{attack.ToString()}</color> damage to first enemy, then deal damage to rest of enemies. Each consecutive attack will be halved";
        else if (attack < defaultattack)
            desc = $"Deal <color=red>{attack.ToString()}</color> damage to first enemy, then deal damage to rest of enemies. Each consecutive attack will be halved";
        else
            desc = $"Deal <color=green>{attack.ToString()}</color> damage to first enemy, then deal damage to rest of enemies. Each consecutive attack will be halved";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
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
            var multiplier = 0.5f;
            foreach (Enemy en in _enemies)
            {
                en.RecieveDamage(Mathf.RoundToInt(attack), this);
                attack *= multiplier;

            }

            
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
        //trail ??
        //doshake pos, nie zazdziala tu/ beda sie skrpty drzec
        enRects.Clear();
        enPositions.Clear();
        foreach (GameObject obj in gameplayManager.enemies)
        {
            enRects.Add(obj.transform.parent.GetComponent<RectTransform>());
            enPositions.Add(obj.transform.parent.GetComponent<RectTransform>().anchoredPosition);
        }
        var getPlayerRect = player.gameObject.GetComponent<RectTransform>();
        var ram=Instantiate(chargeVFX, chargeVFX.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var getRamRect=ram.GetComponent<RectTransform>();
        var smoke = Instantiate(smokeVFX, player.transform.position + smokeSpawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        getPlayerRect.DOAnchorPos(enRects[0].anchoredPosition-moveOffset,animTime).SetEase(anCurve);
        getRamRect.DOAnchorPos(enRects[0].anchoredPosition +ramOffset, animTime).SetEase(anCurve).OnComplete(()=>
        {
            Destroy(ram);
        });
        await Task.Delay(Mathf.RoundToInt(animTime * 1000));
        for (int i = 1; i < enRects.Count+1; i++)
        {
            if (i < enRects.Count)
                if (enRects[i-1]!=null)
                enRects[i - 1].DOAnchorPos(enRects[i].anchoredPosition, knockBackTime * i).SetEase(secAnCurve);
                var wound = Instantiate(woundVFX, enRects[i - 1].transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);

            Destroy(wound,0.5f);
            await Task.Delay(Mathf.RoundToInt(knockBackTime * i*1000));
        }
        await Task.Delay(200);
        for (int j = 0; j < enRects.Count; j++)
        {

                if (enRects[j] != null)
                    enRects[j].DOAnchorPos(enPositions[j], 0.5f).SetEase(Ease.OutQuart);
            
        }
        getPlayerRect.DOJumpAnchorPos(player.myOriginalPosition, jumpPower, numOfJumps, returnTime);
        //getPlayerRect.DOAnchorPos(, returnTime);
        Destroy(smoke);
        

    }
}
