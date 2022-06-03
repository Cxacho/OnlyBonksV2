using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoobySnax : Relic
{
public override void OnEndTurn()
    {
        pl.setDebuffIndicator(2, 3, pl.buffIndicators[3]);
        base.OnEndTurn();
        
        //pl.strenght += 
        
    }
}
