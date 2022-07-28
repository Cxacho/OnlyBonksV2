using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TheHeart : Relic
{
    public List<GameObject> temp = new List<GameObject>();
    public override void Awake()
    {
        description = "If your health is lower than 30% of your maximum hp, at start of a turn gain " + value + "armor.";
        base.Awake();
        gm.OnTurnStart += ApplyBuff;
    }
    void ApplyBuff(object sender, EventArgs e)
    {
        if (pl.currentHealth < pl.maxHealth * 0.3f)
            pl.GetArmor(value);
    }
    private void OnDestroy()
    {
        gm.OnTurnStart -= ApplyBuff;
    }
}
