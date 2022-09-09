using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class EnVulnerableIndi : EnemyIndicator
{
    string description;

    public override void checkIfIExist(Enemy.statuses stat, int val,Enemy enemy)
    {
        base.checkIfIExist(stat, val,enemy);
        myEnemyScript.vurneable += statusValue;
    }
    //tickdown
    public override void Awake()
    {
        base.Awake();
        gm.OnTurnEnd += onTurnEnd;
    }
    void onTurnEnd(object sender, EventArgs e)
    {
        myEnemyScript.vurneable += -1;
        //wymiana na healthdamage
        statusValue = myEnemyScript.vurneable;
        UpdateNum(statusValue);
        if (myEnemyScript.vurneable <= 0)
            Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        gm.OnTurnEnd -= onTurnEnd;
    }
}
