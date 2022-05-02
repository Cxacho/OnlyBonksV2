using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : Enemy, ITakeTurn
{
    private int i = 0;

    private void Start()
    {
        indicatortxt.text = damage.ToString();
    }

    public void takeTurn(Player player)
    {
        switch (i)
        {
            case 0:              
                player.TakeDamage(damage);
                i++;
                indicatorSpriteRenderer.sprite = indicatorImages[1];
                indicatortxt.text = 10.ToString();
                indicatorSpriteRenderer.enabled = false;
                indicatortxt.enabled = false;
                break;
            case 1:
                armor = 10;
                i++;
                
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
                i++;
                indicatortxt.enabled = false;
                indicatorSpriteRenderer.sprite = indicatorImages[2];
                break;
            case 3:
                player.Charmed();
                i++;
                indicatortxt.enabled = true;
                indicatortxt.text = damage.ToString();
                indicatorSpriteRenderer.sprite = indicatorImages[0];
                break;
            case 4:
                player.TakeDamage(damage);
                i = 0;
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
