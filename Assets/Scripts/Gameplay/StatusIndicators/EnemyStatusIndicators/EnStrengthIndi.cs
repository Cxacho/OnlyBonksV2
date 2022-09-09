using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class EnStrengthIndi : EnemyIndicator
{
    string description;

    public override void checkIfIExist(Enemy.statuses stat, int val,Enemy enemy)
    {
        base.checkIfIExist(stat, val,enemy);
        myEnemyScript.strength += statusValue;
    }

}
