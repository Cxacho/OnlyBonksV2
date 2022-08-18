using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class Howl : Card
{
    [SerializeField] GameObject howlVFX;
    [SerializeField] private Vector3 offset;


    public override  void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {



            player.manaText.text = player.mana.ToString();

            base.OnDrop();
            Doanim();
            player.setStatusIndicator(4, 3, player.buffIndicators[3]);
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }

    void  Doanim()
    {
        var prefab = Instantiate(howlVFX, player.gameObject.transform.position+offset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        Destroy(prefab, 5);


    }
}
