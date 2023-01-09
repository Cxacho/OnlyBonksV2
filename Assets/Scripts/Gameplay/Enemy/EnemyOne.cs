using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : Enemy, ITakeTurn
{
    

    private void Start()
    {
        numberOfAttacks = 1;
    }

    public void takeTurn(Player player)
    {
        switch (actionsInt)
        {
            case 0:
                setStatus(statuses.crush, 1, this);
                player.TakeDamage(Mathf.RoundToInt(damage * numberOfAttacks));
                //naklada indicator frail, o wartosci 2 
                SetIndicator();
                actionsInt++;
                ChangeIndicatorTexts("inny");
                otherIndicatortxt.text = 10.ToString();
                break;
            case 1:
                ChangeIndicatorTexts("atak");
                //naklada indicator vurneable, o wartosci 3 
                player.setStatus(Player.playerStatusses.vurneable, 2);
                armor = 10;
                
                SetIndicator();
                actionsInt++;
                numberOfAttacks = 3;
                break;
            case 2:
                //naklada indicator poision , o wartosci 4
                if (armor > 0)
                {
                    player.TakeDamage(Mathf.RoundToInt(damage * numberOfAttacks));
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
                numberOfAttacks = 1;
                break;
            case 4:
                player.TakeDamage(Mathf.RoundToInt(damage * numberOfAttacks));
                
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
