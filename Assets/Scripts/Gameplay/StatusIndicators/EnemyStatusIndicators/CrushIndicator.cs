using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CrushIndicator : EnemyIndicator
{
    string description;
    Enemy enHelper;
    public override void checkIfIExist(Enemy.statuses stat, int val, Enemy enemy)
    {
        base.checkIfIExist(stat, val, enemy);
        enHelper = myEnemyScript;
        enHelper.OnDamageRecieved += ampDamage;
    }
    void ampDamage(Card card, float dam, Enemy enemy)
    {
        
        if (dam >= 10)
        {
            enHelper.OnDamageRecieved -= ampDamage;
            enHelper.RecieveDamage(10, null);
            
            Destroy(this.gameObject);
        }


    }
    private void OnDestroy()
    {
        
    }
}