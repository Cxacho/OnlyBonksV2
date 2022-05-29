using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : Enemy, ITakeTurn
{
    

    private void Start()
    {
        //indicatorStrings.AddRange(new string[] {"10", (damage * 3).ToString(), " ", damage.ToString(), damage.ToString()});
        indicatorStringsBool.AddRange(new bool[] {true,true,false,true,true});
        numberOfAttacks = 1;
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
                player.TakeDamage(damage);
                //naklada indicator frail, o wartosci 2 
                player.setDebuffIndicator(2,0,player.buffIndicators[0]);
                SetIndicator();
                actionsInt++;
                ChangeIndicatorTexts("inny");
                otherIndicatortxt.text = 10.ToString();
                break;
            case 1:
                ChangeIndicatorTexts("atak");
                //naklada indicator vurneable, o wartosci 3 
                player.setDebuffIndicator(3, 1, player.buffIndicators[1]);
                armor = 10;
                
                SetIndicator();
                actionsInt++;
                numberOfAttacks = 3;
                break;
            case 2:
                //naklada indicator poision , o wartosci 4
                player.setDebuffIndicator(4, 2, player.buffIndicators[2]);
                if (armor > 0)
                {
                    player.TakeDamage(damage * 3);
                }
                else
                {
                    crippled();
                }
                SetIndicator();
                actionsInt++;
                break;
            case 3:
                player.setDebuffIndicator(3, 1, player.buffIndicators[1]);
                player.Charmed();
                
                SetIndicator();
                actionsInt++;
                numberOfAttacks = 1;
                break;
            case 4:
                player.setDebuffIndicator(3, 1, player.buffIndicators[1]);
                player.TakeDamage(damage);
                
                SetIndicator();
                actionsInt = 0;
                numberOfAttacks = 1;
                break;
            default:
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
