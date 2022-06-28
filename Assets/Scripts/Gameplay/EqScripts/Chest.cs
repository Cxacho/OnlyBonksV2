using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Inventory  
{
public override void  AddStats()
    {
        base.AddStats();
        Debug.Log("razdwatrzy");
        pl.armor += 2;
    }
    public override void RemoveStats()
    {
        //pl.armor -= 2;
    }
}
