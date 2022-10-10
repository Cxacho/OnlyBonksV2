using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class EnergizeIndicator : Indicator
{
    string description;

    public override void checkIfIExist(Player.playerStatusses ps, int val)
    {
        Debug.Log("CHUJOW STO" + val) ;
        base.checkIfIExist(ps, val);
        player.energize += statusValue;

    }
    //tickdown
    public override void Awake()
    {
        base.Awake();
        gm.OnTurnEnd += onTurnEnd;
        /*gm.OnCardPlayed += cardPlay;
        gm.OnCardExhausted += cardPlay;
        foreach (Card obj in FindObjectsOfType<Card>())
            obj.cost = obj.baseCost-1;
        */
    }
    void onTurnEnd(object sender, EventArgs e)
    {
        /*statusValue = player.energize;
        UpdateNum(statusValue);
        player.energize = 0;*/
        /*player.dexterity += -1;
        statusValue = player.dexterity;
        UpdateNum(statusValue);
        */
        /*if (player.dexterity == 0)
            Destroy(this.gameObject);
        */
    }
    /*void cardPlay(object sender, EventArgs e)
    {
        player.energize += -1;
        statusValue = player.energize;
        UpdateNum(statusValue);
        if (player.energize == 0)
        {
            Destroy(this.gameObject);
        }

    }
    */
    private void OnDestroy()
    {
        gm.OnTurnEnd -= onTurnEnd;
        /*gm.OnCardPlayed -= cardPlay;
        gm.OnCardExhausted -= cardPlay;
        /*foreach (Card obj in FindObjectsOfType<Card>())
            obj.cost = obj.baseCost;
        player.energize = 0;*/
    }
}
