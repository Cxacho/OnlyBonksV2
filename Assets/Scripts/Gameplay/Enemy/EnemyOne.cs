using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : Enemy, ITakeTurn
{
    

    private void Start()
    {
        indicatortxt.text = damage.ToString();
        
    }

    public void takeTurn(Player player)
    {
        switch (actionsInt)
        {
            case 0:              
                player.TakeDamage(damage);
                actionsInt++;
                //indicatorSpriteRenderer.sprite = indicatorImages[1];
                SetIndicator(10.ToString(), 1, false);
                //indicatortxt.text = 10.ToString();
                
                //indicatortxt.enabled = false;
                break;
            case 1:
                armor = 10;
                actionsInt++;
                
                indicatortxt.text = (damage * 3).ToString();
                indicatorSpriteRenderer.sprite = indicatorImages[0];
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
                actionsInt++;
                indicatortxt.enabled = false;
                indicatorSpriteRenderer.sprite = indicatorImages[2];
                break;
            case 3:
                player.Charmed();
                actionsInt++;
                indicatortxt.enabled = true;
                indicatortxt.text = damage.ToString();
                indicatorSpriteRenderer.sprite = indicatorImages[0];
                break;
            case 4:
                player.TakeDamage(damage);
                actionsInt = 0;
                indicatortxt.text = damage.ToString();
                indicatorSpriteRenderer.sprite = indicatorImages[0];
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
