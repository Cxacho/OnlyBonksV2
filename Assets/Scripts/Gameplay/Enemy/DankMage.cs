using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DankMage : Enemy, ITakeTurn
{


    private void Start()
    {
        //indicatorStrings.AddRange(new string[] {"10", (damage * 3).ToString(), " ", damage.ToString(), damage.ToString()});
        indicatorStringsBool.AddRange(new bool[] { false, true, false, false });
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
                //Dankening
                //naklada indicator frail , o wartosci 3
                player.setStatus(Player.playerStatusses.frail, 3);
                
                SetIndicator();
                actionsInt++;
                break;
            case 1:
                //FeelsDankMan
                //nalozyc indicator buffa na przeciwnikow
                foreach (Enemy en in FindObjectsOfType<Enemy>())
                    en.baseDamage += 3;
                ChangeIndicatorTexts("atak");
                SetIndicator();
                actionsInt++;

                
                break;
            case 2:
                //DankWave
                player.TakeDamage(Mathf.RoundToInt(damage * numberOfAttacks));
                SetIndicator();
                actionsInt++;
                ChangeIndicatorTexts("inny");
                otherIndicatortxt.text = 7.ToString();
                break;
            case 3:
                //Dankness protect you !
                foreach (Enemy en in FindObjectsOfType<Enemy>())
                    en.GetArmor(7);
                SetIndicator();
                actionsInt++;
                otherIndicatortxt.enabled = false;
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
