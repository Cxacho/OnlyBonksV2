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

    //Gdy w nast�pnym case jest atak wpisujemy NextCaseAttack(1). Value w nawiasie to liczba atakow, ktora ma sie wywolac w next case!!!

    //Gdy w nastepnym case jest coc innego niz atak wpisujemy NextCaseOther("String").

    //W ostatnim case musi by� actionsInt = 0 . We wszystkich innych case actionsInt++ wykonuje si� w metodach NextCase

    public void takeTurn(Player player)
    {
        switch (actionsInt)
        {
            case 0:
                //Disgusting tendencies
                player.setStatus(Player.playerStatusses.frail, 5);
                NextCaseAttack(1);
                break;
            case 1:
                //my right arm, is much stronger than left arm
                player.TakeDamage(Mathf.RoundToInt(damage * numberOfAttacks));
                NextCaseOther("7");
                break;
            case 2:
                //white knighting
                GetArmor(7);
                NextCaseAttack(1.4f);
                break;
            case 3:
                //heavy punch
                if (player.armor > damage * numberOfAttacks)
                {
                    player.TakeDamage(Mathf.RoundToInt(damage * numberOfAttacks));
                    RecieveDamage(player.armor,null);
                }
                else
                {
                    player.TakeDamage(Mathf.RoundToInt(damage * numberOfAttacks));
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
