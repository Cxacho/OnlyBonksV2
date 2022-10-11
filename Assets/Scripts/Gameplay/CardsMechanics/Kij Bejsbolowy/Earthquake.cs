using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Threading.Tasks;
public class Earthquake : Card
{
    private TextMeshPro textMeshPro;
    [SerializeField] Vector3 offset,offsetUp,spawnOffset,offsetDown;
    [SerializeField] private GameObject bat;
    [SerializeField] private GameObject eruptions;
    [SerializeField] private GameObject rocks;
    [SerializeField] private float vfxScale;

    private void Start()
    {
        desc = $"Apply 3 strength this battle, if your total strength is 5 or higher, deal <color=white>{(attack+3).ToString()}</color> damage to all enemies";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }
    private void FixedUpdate()
    {
        
        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);

        if (attack == defaultattack)
            desc = $"Apply 3 strength this battle, if your total strength is 5 or higher, deal <color=white>{(attack + 3).ToString()}</color> damage to all enemies";
        else if (attack < defaultattack)
            desc = $"Apply 3 strength this battle, if your total strength is 5 or higher, deal <color=red>{(attack + 3).ToString()}</color> damage to all enemies";
        else
            desc = $"Apply 3 strength this battle, if your total strength is 5 or higher, deal <color=green>{(attack + 3).ToString()}</color> damage to all enemies";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    public override async void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();
            player.setStatusIndicator(3, 3, player.buffIndicators[3]);
            gameplayManager.state = BattleState.INANIM;
            await DoAnim();
            gameplayManager.state = BattleState.PLAYERTURN;
            
            StartCoroutine(ExecuteAfterTime(1f));

            

            
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

        player.manaText.text = player.mana.ToString();

    }
    async Task DoAnim()
    {
        var batObj = Instantiate(bat, gameplayManager.player.transform.position+ spawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        var batRect =batObj.GetComponent<RectTransform>();
        batObj.transform.DOMove(offsetUp, 0.6f);
        batObj.transform.DORotate(new Vector3(0,0,180), 0.6f);
        await Task.Delay(300);
        batObj.transform.DOMove(offset, 0.6f);
        await Task.Delay(300);
        var eruptionObj= Instantiate(eruptions, batObj.transform.position - offsetDown, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        await Task.Delay(300);
        if (player.strenght >= 5)
        {
            var rocksObj = Instantiate(rocks, gameplayManager.vfxCanvas.transform);
            await Task.Delay(300);

            foreach (Enemy en in _enemies)
            {
                Debug.Log("dosta³em " + attack);
                await Task.Delay(2000);
                en.RecieveDamage(attack, this);

            }

            Destroy(rocksObj, 4);
        }
        Destroy(batObj,2);
        Destroy(eruptionObj,2);
        
    }

    
}
