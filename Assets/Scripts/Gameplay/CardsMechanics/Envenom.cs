using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;

public class Envenom : Card
{
    public int armor;
    [SerializeField] private GameObject flaskVFX, swordVFX;
    [SerializeField] private Vector3 flaskOffset, spawnOffset, swordRot,moveOffset;
    [SerializeField] private float strength,despawnHeight;
    List<GameObject> vfxses = new List<GameObject>();

    private void Start()
    {
        desc = $"Enchant your bat dealing <color=white>{attack.ToString()}</color> damage and applying 3 bleed. Then double the bleed of enemy";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }


    private void FixedUpdate()
    {

        calc(Mathf.RoundToInt(attack), cardScalingtype, secondaryScalingType);
        if (attack == defaultattack)
            desc = $"Enchant your bat dealing <color=white>{attack.ToString()}</color> damage and applying 3 bleed. Then double the bleed of enemy";
        else if (attack < defaultattack)
            desc = $"Enchant your bat dealing <color=red>{attack.ToString()}</color> damage and applying 3 bleed. Then double the bleed of enemy";
        else
            desc = $"Enchant your bat dealing <color=green>{attack.ToString()}</color> damage and applying 3 bleed. Then double the bleed of enemy";
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }

    public override async void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            //player.GetArmor(armor);



            player.manaText.text = player.mana.ToString();

            base.OnDrop();
            gameplayManager.state = BattleState.INANIM;
            await DoAnim();
            gameplayManager.state = BattleState.PLAYERTURN;

            foreach (Enemy en in _enemies)
            {
                if (en.targeted == true)
                {
                    await Task.Delay(2000);
                    en.RecieveDamage(attack, this);
                    en.setStatus(Enemy.statuses.bleeding, 3, en);
                    en.setStatus(Enemy.statuses.bleeding, (en.bleed), en);
                    

                }
                en.targeted = false;
            }
            resetTargetting();
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }


    async Task DoAnim()
    {
        var sword = Instantiate(swordVFX, gameplayManager.player.transform.position + spawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        sword.transform.rotation = Quaternion.Euler(swordRot);
        var flask  = Instantiate(flaskVFX, gameplayManager.player.transform.position + flaskOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        flask.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        var getRect = flask.GetComponent<RectTransform>();
        var getRectPos = getRect.anchoredPosition;
        getRect.DOAnchorPos(moveOffset, 2f).OnComplete(() =>
        {
            getRect.DOAnchorPos(new Vector3(getRectPos.x, getRectPos.y + despawnHeight, 1), 2f);
        });
        getRect.transform.DOShakeRotation(4, strength).SetEase(Ease.Linear);
        Destroy(flask, 3.5f);
        Destroy(sword, 3.5f);
    }
}
