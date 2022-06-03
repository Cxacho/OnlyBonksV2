using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketKnife : Relic
{
    public override void OnEndTurn()
    {
        gm.CreateCard(0);
        base.OnEndTurn();

        //pl.strenght += 

    }
}
