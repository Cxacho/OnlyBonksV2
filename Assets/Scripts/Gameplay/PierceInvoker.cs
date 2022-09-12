using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PierceInvoker : Indicator
{
    List<Enemy> ens = new List<Enemy>();


    public override void Awake()
    {
        base.Awake();
        foreach (GameObject en in gm.enemies)
        {
            ens.Add(en.GetComponent<Enemy>());
        }
        for (int i =0;i<gm.enemies.Count;i++)
        {
            ens[i].OnDamageRecieved += ApplyEffect;
        }
        gm.OnTurnEnd += DestroySelf;
    }
    void ApplyEffect (Card card,float dam,Enemy en)
    {
        Debug.Log("I applied "+dam+" to " + en.gameObject.name);
        en.setStatus(Enemy.statuses.bleeding, Mathf.RoundToInt(dam), en);
    }
    private void OnDestroy()
    {
        for (int i = 0; i < gm.enemies.Count; i++)
        {
            ens[i].OnDamageRecieved -= ApplyEffect;
        }
        gm.OnTurnEnd -= DestroySelf;
    }
    void DestroySelf(object sender, EventArgs e)
    {
        Destroy(this.gameObject);
    }
}
