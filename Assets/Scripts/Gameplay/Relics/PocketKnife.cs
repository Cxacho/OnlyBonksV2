using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PocketKnife : Relic, IPointerEnterHandler,IPointerExitHandler
{
    public List<GameObject> temp = new List<GameObject>();
    GameObject cardToCreate;
    public override void Awake()
    {
        
        description = "At start of your turn, add Pocket knife to your hand, if you dont have one.";
        base.Awake();
        gm.OnTurnStart += AddCardToHand;
    }


    void AddCardToHand(object sender,EventArgs e)
    {
        temp.Clear();
        temp.AddRange(gm.drawDeck);
        temp.AddRange(gm.discardDeck);
        if (temp.Contains(gm.allCardsCreatable[0]) == false)
        gm.CreateCard(gm.allCardsCreatable[0], -1, 66);
    }
    private void OnDestroy()
    {
        gm.OnTurnStart -= AddCardToHand;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cardToCreate == null)
        {
            cardToCreate = Instantiate(gm.allCardsCreatable[0], gm.canvas.transform);
            cardToCreate.GetComponent<Card>().enabled = false;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardToCreate != null)
        {
            Destroy(cardToCreate);
            cardToCreate = null;
        }
    }
}
