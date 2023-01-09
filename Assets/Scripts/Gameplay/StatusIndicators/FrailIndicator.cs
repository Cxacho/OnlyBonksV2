using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class FrailIndicator : Indicator
{
    string description;

    public override void checkIfIExist(Player.playerStatusses ps, int val)
    {
        base.checkIfIExist(ps, val);
        player.frail += statusValue;
    }
    //tickdown
    public override void Awake()
    {
        base.Awake();
        gm.OnTurnEnd += onTurnEnd;
    }
    void onTurnEnd(object sender, EventArgs e)
    {
        player.frail -= 1;
        statusValue = player.frail;
        UpdateNum(statusValue);
        if (player.frail == 0)
            Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        gm.OnTurnEnd -= onTurnEnd;
    }
}
