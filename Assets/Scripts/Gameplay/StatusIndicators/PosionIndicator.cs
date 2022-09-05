using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PosionIndicator : Indicator
{
    string description;

    public override void checkIfIExist(Player.playerStatusses ps, int val)
    {
        base.checkIfIExist(ps, val);
        player.poison += statusValue;
    }
    //tickdown
    public override void Awake()
    {
        base.Awake();
        gm.OnTurnEnd += onTurnEnd;
    }
    void onTurnEnd(object sender, EventArgs e)
    {
        player.poison += -1;
        player.TakeHealthDamage(player.poison);
        statusValue = player.poison;
        UpdateNum(statusValue);
        if (player.poison <= 0)
            Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        gm.OnTurnEnd -= onTurnEnd;
    }
}
