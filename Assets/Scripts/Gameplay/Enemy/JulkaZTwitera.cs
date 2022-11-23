using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JulkaZTwitera : Enemy, ITakeTurn
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
                //leftist take
                player.setStatus(Player.playerStatusses.frail, 2);
                NextCaseAttack(1);
                break;
            case 1:
                //furious bite
                player.TakeDamage(numberOfAttacks * damage);
                NextCaseOther("3");
                break;
            case 2:
                //Love for all
                foreach (Enemy enemy in gm.enemyType)
                    GetArmor(3);
                NextCaseOther("5");
                break;
            case 3:
                //Artistic screetch
                GetArmor(5);
                NextCaseOther("");
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
