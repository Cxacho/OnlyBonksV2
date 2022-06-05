using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GeneralGrievious : Enemy, ITakeTurn
{
    int bossDamage = 5;
    private void Start()
    {
        
    }
    // pierwsza faza ataki x2
    // druga faza ataki x4
    // deflect niektórych obra¿eñ
    private void Update()
    {
        //logika zmiany phase podmiana sprite
    }
    public async void takeTurn(Player player)
    {
        switch (actionsInt)
        {
            case 0:
                if(_currentHealth< maxHealth/2) await attackPhaseTwo(player);
                else await attackPhaseOne(player);
                actionsInt++;
                
                break;
            case 1:

                ShieldUp();
                
                actionsInt++;
                
                break;
            case 2:
                
                actionsInt++;
                break;
            case 3:
                
                actionsInt++;
                
                break;
            case 4:
                
                
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

    private async Task attackPhaseOne(Player player)
    {
        for (int i = 0; i < 1; i++)
        {
            var checkPlayerHp = player.currentHealth;
            player.TakeDamage(bossDamage);
            if(checkPlayerHp > player.currentHealth)
            {
                bossDamage++;
            }
            await Task.Yield();
        }
        
    }
    private async Task attackPhaseTwo(Player player)
    {
        for (int i = 0; i < 3; i++)
        {
            var checkPlayerHp = player.currentHealth;
            player.TakeDamage(bossDamage);
            if (checkPlayerHp > player.currentHealth)
            {
                bossDamage++;
            }
            await Task.Yield();
        }

    }
    private void ShieldUp()
    {
        armor = 20;
    }
}

