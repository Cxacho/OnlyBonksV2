using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheHeart : Relic
{
    public override void OnEndTurn()
    {
        if (pl.armor < 4)
            pl.GetArmor(5);
        base.OnEndTurn();

        //pl.strenght += 

    }
}
