using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopCat : Enemy, ITakeTurn
{
    int charge;

    private void Start()
    {
        actionsInt = 0;
        damage = baseDamage;
        indicatorStringsBool.AddRange(new bool[] { true, true, true, false,true });
        numberOfAttacks = 1;
        attackIndicatortxt.enabled = true;

        /*
        if (pl.vurneable > 0)
        {
            damage = damage * 1.25f;
        }
        */
    }

    public void takeTurn(Player player)
    {
        switch (actionsInt)
        {
            case 0:
                //scratch
                
                charge++;
                player.TakeDamage(Mathf.RoundToInt(damage * numberOfAttacks));
                NextCaseOther("8");
                break;
            case 1:
                //proteccc
                charge++;
                armor += 8;
                NextCaseAttack(0.8f);


                break;
            case 2:
                //proteccc/ataccc
                
                charge++;
                player.TakeDamage(Mathf.RoundToInt(damage * numberOfAttacks));
                armor += 5;
                NextCaseAttack(charge *1);
                break;
            case 3:
                //laser ????
                
                player.TakeHealthDamage(charge *damage);
                NextCaseOther("");


                break;
                
            case 4:
                //rest/nap
                //attackIndicatortxt.enabled = true;
                NextCaseAttack(1);
                actionsInt = 0;
                break;
                
        }

    }
    public void Phase2(Player player)
    {
        // dwoch ziomkow
    }

    private void crippled()
    {

        damage = damage - 3;
    }

}
