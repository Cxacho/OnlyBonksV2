using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC1 : Enemy, ITakeTurn
{


    private void Start()
    {
        indicatorStringsBool.AddRange(new bool[] { false, true, true,false });
        damage = baseDamage;
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
                //Anger Issues
                //naklada indicator poision , o wartosci 4
                //damage = 7
                player.setDebuffIndicator(3, 0, player.buffIndicators[0]);
                Debug.Log(Mathf.RoundToInt(damage * numberOfAttacks));
                NextCaseOther("4");
                break;
            case 1:
                //MassHysteria
                //getarmor
                foreach (Enemy en in gm.enType)
                {
                    en.GetArmor(4);
                }
                NextCaseAttack(1);
                break;
            case 2:
                //Remain PC
                player.TakeDamage(Mathf.RoundToInt(damage * numberOfAttacks));
                NextCaseAttack(1f);
                break;
            case 3:
                //FollowTheCrowd
                player.TakeDamage(Mathf.RoundToInt(damage * numberOfAttacks));
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
