using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class StrengthIndicator : Indicator
{
    string description;
    
    public override void checkIfIExist(Player.playerStatusses ps, int val)
    {
        base.checkIfIExist(ps,val);
        player.strenght += statusValue;
    }
    public override void Awake()
    {
        base.Awake();
        gm.OnTurnEnd += onTurnEnd;
    }
    void onTurnEnd(object sender, EventArgs e)
    {
        player.strenght += -1;
        statusValue = player.strenght;
        UpdateNum(statusValue);
        if (player.strenght == 0)
            Destroy(this.gameObject);
    }
    private void OnDestroy()
    {
        gm.OnTurnEnd -= onTurnEnd;
    }
}
