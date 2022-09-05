using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class VurneableIndicator : Indicator
{
    string description;

    public override void checkIfIExist(Player.playerStatusses ps, int val)
    {
        base.checkIfIExist(ps, val);
        player.vurneable += statusValue;
    }
    //tickdown
    public override void Awake()
    {
        base.Awake();
        gm.OnTurnEnd += onTurnEnd;
    }
    void onTurnEnd(object sender, EventArgs e)
    {
        player.vurneable += -1;
        statusValue = player.vurneable;
        UpdateNum(statusValue);
        if (player.vurneable == 0)
            Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        gm.OnTurnEnd -= onTurnEnd;
    }
}
