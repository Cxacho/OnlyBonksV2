using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Inventory  
{
public override void  AddStats()
    {
        base.AddStats();
    }
    public override void RemoveStats()
    {
        //pl.armor -= 2;
    }
}
