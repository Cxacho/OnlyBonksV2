using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wartownik : Enemy, ITakeTurn
{


    private void Start()
    {
        indicatorStringsBool.AddRange(new bool[] { false, true, true, true});
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
                //Impale
                //po demie dodac tu zadawanie obrazen typu pierce zamiast take damage, take health damage
                player.TakeDamage(damage * numberOfAttacks);
                NextCaseOther("");
                break;
            case 1:
                //For the queen <3
                foreach (Enemy en in gm.enType)
                {
                    en.setStatusIndicator(4, 0, gm.enemiesIndicators[0]);
                    en.baseDamage += 4;
                }
                NextCaseAttack(1.5f);
                break;
            case 2:
                //Charge !!!
                player.TakeDamage(numberOfAttacks * damage);
                NextCaseOther("8");
                break;
            case 3:
                //Raise The Banner!
                setStatusIndicator(2, 0, gm.enemiesIndicators[0]);
                baseDamage += 2;
                GetArmor(8);
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
