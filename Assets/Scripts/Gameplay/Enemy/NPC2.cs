using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC2 : Enemy, ITakeTurn
{


    private void Start()
    {
        indicatorStringsBool.AddRange(new bool[] { false, false, true, true });
        damage = baseDamage;
        indicatorStrings.AddRange(new string[] { (damage).ToString(), "12", (damage * 1.5).ToString(), " " });
        numberOfAttacks = 1;
        //tutaj wpisujemy Indicator na start (czy jest true/false) i jaki ma napis
        attackIndicatortxt.enabled = true;

        /*
        if (pl.vurneable > 0)
        {
            damage = damage * 1.25f;
        }
        */
    }

    //Gdy w nastêpnym case jest atak wpisujemy NextCaseAttack(1). Value w nawiasie to liczba atakow, ktora ma sie wywolac w next case!!!

    //Gdy w nastepnym case jest coc innego niz atak wpisujemy NextCaseOther("String").

    //W ostatnim case musi byæ actionsInt = 0 . We wszystkich innych case actionsInt++ wykonuje siê w metodach NextCase

    public void takeTurn(Player player)
    {
        switch (actionsInt)
        {
            case 0:
                //Anger Issues
                //naklada indicator poision , o wartosci 4
                //damage = 7
                player.TakeDamage(Mathf.RoundToInt(damage * numberOfAttacks));
                NextCaseOther("4");
                break;
            case 1:
                //MassHysteria
                //getarmor
                foreach (Enemy enemy in gm.enemyType)
                {
                    enemy.GetArmor(4);
                }
                NextCaseOther("");
                break;
            case 2:
                //Remain PC
                player.setStatusIndicator(2, 1, player.buffIndicators[1]);
                NextCaseAttack(1f);
                break;
            case 3:
                //FollowTheCrowd
                player.TakeDamage(Mathf.RoundToInt(damage * numberOfAttacks));
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
