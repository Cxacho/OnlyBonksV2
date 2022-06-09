using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coonsomer : Enemy, ITakeTurn
{


    private void Start()
    {
        indicatorStringsBool.AddRange(new bool[] { true, true, false, true });
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
                //light jab
                player.TakeDamage(damage * numberOfAttacks);
                NextCaseAttack(1.5f);
                break;
            case 1:
                //slam
                player.TakeDamage(numberOfAttacks * damage);
                NextCaseAttack(4);
                break;
            case 2:
                //CON - SOOOOOOOM
                player.TakeDamage(damage * numberOfAttacks);
                NextCaseOther("");
                break;
            case 3:
                //well earned rest
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
