using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketKnife : Relic
{
    public List<GameObject> temp = new List<GameObject>();
    public override void OnEndTurn()
    {
        temp.Clear();
        temp.AddRange(gm.drawDeck);
        temp.AddRange(gm.discardDeck);
        if(temp.Contains(gm.allCards[0]) == false)
        gm.CreateCard(gm.allCards[0],-1,66);
        base.OnEndTurn();

        //pl.strenght += 

    }

}
