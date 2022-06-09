using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBird : Enemy, ITakeTurn
{
    // Start is called before the first frame update
    void Start()
    {
        indicatorStringsBool.AddRange(new bool[] { true, true, false, true});

    }

    public void takeTurn(Player player)
    {
        switch (actionsInt)
        {
            case 0:
                player.TakeDamage(Mathf.RoundToInt(damage * numberOfAttacks));
                NextCaseAttack(1.5f);
                break;
            case 1:
                player.TakeDamage(Mathf.RoundToInt(damage * numberOfAttacks));
                NextCaseAttack(2f);
                break;
            case 2:
                player.TakeDamage(Mathf.RoundToInt(damage * numberOfAttacks));
                NextCaseOther(" ");
                break;
            case 3:
                
                NextCaseAttack(1f);
                actionsInt = 0;
                break;
        }
    }
}