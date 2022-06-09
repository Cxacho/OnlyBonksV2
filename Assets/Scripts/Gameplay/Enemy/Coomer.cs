using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coomer : Enemy, ITakeTurn
{

    private void Start()
    {
        indicatorStringsBool.AddRange(new bool[] { true, true, true, false });
        indicatorStrings.AddRange(new string[] { (damage).ToString(), "12", (damage * 1.5).ToString(), " " });
        numberOfAttacks = 1;
        //tutaj wpisujemy Indicator na start (czy jest true/false) i jaki ma napis
        attackIndicatortxt.enabled = false;

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
                //Disgusting tendencies
                player.setDebuffIndicator(3, 2, player.buffIndicators[2]);
                player.setDebuffIndicator(2, 1, player.buffIndicators[1]);
                player.setDebuffIndicator(2, 0, player.buffIndicators[0]);
                NextCaseAttack(1);
                break;
            case 1:
                //my right arm, is much stronger than left arm
                player.TakeDamage(numberOfAttacks * damage);
                NextCaseOther("8");
                break;
            case 2:
                //white knighting
                GetArmor(8);
                NextCaseAttack(1.4f);
                break;
            case 3:
                //heavy punch
                if (player.armor > damage * numberOfAttacks)
                {
                    player.TakeDamage(damage * numberOfAttacks);
                    ReceiveDamage(player.armor);
                }
                else
                {
                    player.TakeDamage(damage * numberOfAttacks);
                }
                NextCaseOther(" ");
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
