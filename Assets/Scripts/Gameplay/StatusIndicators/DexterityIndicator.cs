using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DexterityIndicator : Indicator
{
    string description;
    
    public override void checkIfIExist(Player.playerStatusses ps, int val)
    {
        base.checkIfIExist(ps,val);
        player.dexterity += statusValue;
    }
    public override void Awake()
    {
        base.Awake();
        gm.OnTurnEnd += onTurnEnd;
    }
    void onTurnEnd(object sender, EventArgs e)
    {
        player.dexterity -= 1;
        statusValue = player.dexterity;
        UpdateNum(statusValue);
        if (player.dexterity == 0)
            Destroy(this.gameObject);
    }
    private void OnDestroy()
    {
        gm.OnTurnEnd -= onTurnEnd;
    }
}
