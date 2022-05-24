using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : Enemy, ITakeTurn
{
    

    private void Start()
    {
        indicatorStrings.AddRange(new string[] { "10", (damage * 3).ToString(), " ", damage.ToString(), damage.ToString() });
        indicatorStringsBool.AddRange(new bool[] {true,true,false,true,true});

        indicatortxt.text = damage.ToString();
        
    }

    public void takeTurn(Player player)
    {
        switch (actionsInt)
        {
            case 0:              
                player.TakeDamage(damage);
                

                SetIndicator();
                actionsInt++;
                break;
            case 1:
                armor = 10;
                SetIndicator();
                actionsInt++;
                break;
            case 2:

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
                player.Charmed();
                SetIndicator();
                actionsInt++;
                break;
            case 4:
                player.TakeDamage(damage);
                SetIndicator();
                actionsInt = 0;
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
