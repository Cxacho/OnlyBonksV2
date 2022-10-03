using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class Testudo : Card
{
    public int armor;
    [SerializeField] private GameObject shieldGO;
    [SerializeField] private Vector3 horizontalOffset, vecticalOffset;
    List<GameObject> vfxses = new List<GameObject>();
    public override async void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            player.GetArmor(armor);

            

            player.manaText.text = player.mana.ToString();

            base.OnDrop();
            gameplayManager.state = BattleState.INANIM;
            await DoAnim();
            gameplayManager.state = BattleState.PLAYERTURN;
            gameplayManager.DrawCards(2);
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }

    }

    async Task DoAnim()
    {
        var shieldOne = Instantiate(shieldGO, gameplayManager.player.transform.position, Quaternion.identity, gameplayManager.vfxCanvas.transform);
        shieldOne.transform.localScale = new Vector3(8, 8, 1);
        shieldOne.transform.DOScale(new Vector3(30, 30, 1),0.3f);
        vfxses.Add(shieldOne);
        //shieldOne.transform.localScale = new Vector3(,,);
        //doscale
        await Task.Delay(500);
        var shieldTwo = Instantiate(shieldGO, gameplayManager.player.transform.position +(horizontalOffset*3), Quaternion.identity, gameplayManager.vfxCanvas.transform);
        shieldTwo.transform.DOMove(gameplayManager.player.transform.position + horizontalOffset, 0.5f);
        vfxses.Add(shieldTwo);
        //skala?
        //ruchdosrodka
        var shieldThree = Instantiate(shieldGO, gameplayManager.player.transform.position-(horizontalOffset*3), Quaternion.identity, gameplayManager.vfxCanvas.transform);
        shieldThree.transform.DOMove(gameplayManager.player.transform.position - horizontalOffset, 0.5f);
        vfxses.Add(shieldThree);
        //skala?
        //ruch do srodka
        await Task.Delay(500);
        var shieldFour = Instantiate(shieldGO, (gameplayManager.player.transform.position +vecticalOffset*3), Quaternion.identity, gameplayManager.vfxCanvas.transform);
        shieldFour.transform.DOMove(gameplayManager.player.transform.position + vecticalOffset, 0.5f);
        var newRot = shieldFour.transform.rotation.eulerAngles + new Vector3(60,0,90);
        shieldFour.transform.rotation=Quaternion.Euler(newRot);
        shieldFour.transform.localScale += new Vector3(0, 20, 0);
        vfxses.Add(shieldFour);
        await Task.Delay(500);
        foreach (GameObject obj in vfxses)
            obj.transform.DOScale(Vector3.zero, 0.5f);
        //skala?
        //domove
        await Task.Delay(1000);
        foreach (GameObject obj in vfxses)
            Destroy(obj);

    }   
}
