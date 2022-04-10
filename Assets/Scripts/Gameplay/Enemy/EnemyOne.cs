using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : Enemy, ITakeTurn
{
    private int i = 0;

    public void takeTurn(Player player)
    {
        switch (i)
        {
            case 0:
                player.TakeDamage(damage);
                i++;
                break;
            case 1:
                armor = 10;
                i++;
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
                i++;
                break;
            case 3:
                player.Charmed();
                i++;
                break;
            case 4:
                player.TakeDamage(damage);
                i = 0;
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

        damage = (float)(damage * 0.7);
    }
}
