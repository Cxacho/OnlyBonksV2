using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class Envenom : Card
{
    public int armor;
    [SerializeField] private GameObject flaskVFX, swordVFX;
    [SerializeField] private Vector3 flaskOffset, spawnOffset, swordRot,moveOffset;
    [SerializeField] private float strength,despawnHeight;
    List<GameObject> vfxses = new List<GameObject>();

    public override void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            player.GetArmor(armor);



            player.manaText.text = player.mana.ToString();

            base.OnDrop();
            DoAnim();
            gameplayManager.DrawCards(2);
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }


    void DoAnim()
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
