using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotFriend : Enemy, ITakeTurn
{


    private void Start()
    {
        //indicatorStrings.AddRange(new string[] {"10", (damage * 3).ToString(), " ", damage.ToString(), damage.ToString()});
        indicatorStringsBool.AddRange(new bool[] { true, true, true, false});
        numberOfAttacks = 1;
        attackIndicatortxt.enabled = false;
        //SetAttackString(1);
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
                //corrosive spit
                //naklada indicator poision , o wartosci 4
                player.setDebuffIndicator(4, 2, player.buffIndicators[2]);
                SetIndicator();
                actionsInt++;
                numberOfAttacks = 1;
                break;
            case 1:
                //light Jab
                player.TakeDamage(damage);                                        
                SetIndicator();
                actionsInt++;
                ChangeIndicatorTexts("inny");
                otherIndicatortxt.text = 12.ToString();
                break;
            case 2:
                //harden
                armor = armor + 12;
                ChangeIndicatorTexts("atak");
                SetIndicator();
                actionsInt++;
                numberOfAttacks = 1.5f;
                break;
            case 3:
                //slam
                player.TakeDamage(damage * 1.5f);
                SetIndicator();
                actionsInt++;
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
