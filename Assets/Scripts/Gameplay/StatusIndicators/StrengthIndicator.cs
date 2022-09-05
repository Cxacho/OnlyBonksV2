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

}
