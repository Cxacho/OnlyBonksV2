using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class EnBleedIndi : EnemyIndicator
{
    string description;

    public override void checkIfIExist(Enemy.statuses stat, int val,Enemy enemy)
    {
        base.checkIfIExist(stat, val,enemy);
        myEnemyScript.bleed += statusValue;
    }
    //tickdown
    public override void Awake()
    {
        base.Awake();
        gm.OnTurnEnd += onTurnEnd;
    }
    void onTurnEnd(object sender, EventArgs e)
    {
        myEnemyScript.bleed += -1;
        //wymiana na healthdamage
        myEnemyScript.RecieveDamage(myEnemyScript.bleed, null);
        statusValue = myEnemyScript.bleed;
        UpdateNum(statusValue);
        if (myEnemyScript.bleed <= 0)
            Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        gm.OnTurnEnd -= onTurnEnd;
    }
}
