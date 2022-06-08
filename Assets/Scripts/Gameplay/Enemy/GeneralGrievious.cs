using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GeneralGrievious : Enemy, ITakeTurn
{
    
    private void Start()
    {

        indicatorStringsBool.AddRange(new bool[] { true, true, true, true, true });
        
    }
    // pierwsza faza ataki x2
    // druga faza ataki x4
    // deflect niektórych obra¿eñ
    private void Update()
    {
        if (_currentHealth < maxHealth / 2)
        {
            attackIndicatortxt.text = ("4x" + baseDamage).ToString();
        }
        else
        {
            attackIndicatortxt.text = ("2x" + baseDamage).ToString();
        }
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
                if (_currentHealth < maxHealth / 2) await attackPhaseTwo(player);
                else await attackPhaseOne(player);
                actionsInt++;
                
                break;
            case 2:
                if (_currentHealth < maxHealth / 2) await attackPhaseTwo(player);
                else await attackPhaseOne(player);
                actionsInt++;
                break;
            case 3:
                if (_currentHealth < maxHealth / 2) await attackPhaseTwo(player);
                else await attackPhaseOne(player);
                actionsInt++;
                
                break;
            case 4:

                if (_currentHealth < maxHealth / 2) await attackPhaseTwo(player);
                else await attackPhaseOne(player);
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
        for (int i = 0; i < 2; i++)
        {
            var checkPlayerHp = player.currentHealth;
            player.TakeDamage(baseDamage);
            if(checkPlayerHp > player.currentHealth)
            {
                baseDamage++;
            }
            await Task.Delay(150);
        }
        
    }
    private async Task attackPhaseTwo(Player player)
    {
        for (int i = 0; i < 4; i++)
        {
            var checkPlayerHp = player.currentHealth;
            player.TakeDamage(baseDamage);
            if (checkPlayerHp > player.currentHealth)
            {
                baseDamage++;
            }
            await Task.Delay(150);
        }

    }
    private void ShieldUp()
    {
        GetArmor(20);
    }
}

