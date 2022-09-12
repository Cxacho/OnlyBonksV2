using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class ThinkingAhead : Card
{
    public int armor;
    [SerializeField] private GameObject dogeVFX, lightBulbVFX, handVFX;
    [SerializeField] private Vector3 dogeSpawnOffset, handSpawnOffset,moveTo,particleSpawnOffset;
    List<GameObject> vfxses = new List<GameObject>();

    public override async void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            player.GetArmor(armor);



            player.manaText.text = player.mana.ToString();

            base.OnDrop();
            await Doanim();
            //na poczatku tury dobierz 2 karty
            gameplayManager.DrawCards(2);
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }

    async Task Doanim()
    {
        var dogeSpriteVFX = Instantiate(dogeVFX,dogeSpawnOffset , Quaternion.identity, gameplayManager.vfxCanvas.transform);
        dogeSpriteVFX.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
        dogeSpriteVFX.GetComponent<SpriteRenderer>().material.DOFade(1, 0.5f);
        var handSpriteVFX = Instantiate(handVFX, handSpawnOffset+dogeSpriteVFX.transform.position, handVFX.transform.rotation, gameplayManager.vfxCanvas.transform);
        handSpriteVFX.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
        handSpriteVFX.GetComponent<SpriteRenderer>().material.DOFade(1, 0.5f);
        var lightBulb= Instantiate(lightBulbVFX, dogeSpriteVFX.transform.position+particleSpawnOffset, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        await Task.Delay(1000);
        handSpriteVFX.GetComponent<RectTransform>().DOAnchorPos(moveTo, 0.5f).SetLoops(-1, LoopType.Yoyo);
        handSpriteVFX.GetComponent<RectTransform>().DORotate(new Vector3(0,0,15), 0.5f).SetLoops(-1, LoopType.Yoyo);
        //dofade
        await Task.Delay(1000);
        handSpriteVFX.GetComponent<SpriteRenderer>().material.DOFade(0, 1f);
        dogeSpriteVFX.GetComponent<SpriteRenderer>().material.DOFade(0, 1f);
        Destroy(dogeSpriteVFX,3);
        Destroy(handSpriteVFX,3);
        Destroy(lightBulb,1);



    }
}
