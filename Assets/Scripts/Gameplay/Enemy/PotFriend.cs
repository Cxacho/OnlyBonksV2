using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotFriend : Enemy, ITakeTurn
{


    private void Start()
    {
        indicatorStringsBool.AddRange(new bool[] { true, true, true, false});
        indicatorStrings.AddRange(new string[] { (damage).ToString(), "12", (damage * 1.5).ToString(), " "});
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
                //corrosive spit
                //naklada indicator poision , o wartosci 4
                player.setDebuffIndicator(4, 2, player.buffIndicators[2]);
                NextCaseAttack(1);
                break;
            case 1:
                //light Jab
                player.TakeDamage(numberOfAttacks * damage);
                NextCaseOther("12");
                break;
            case 2:
                //harden
                armor = armor + 12;
                NextCaseAttack(1.5f);
                break;
            case 3:
                //slam
                player.TakeDamage(damage * numberOfAttacks);
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
