using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BasicDefend : Card
{
    public int armor;
    public GameObject shield;

    private void Start()
    {
        desc = $"Gain <color=white>{armor.ToString()}</color> armor";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }

    public override void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            player.GetArmor(armor);

            
            player.manaText.text = player.mana.ToString();
            base.OnDrop();
            ui.DisableButtons(0);
            /*
            foreach (Enemy en in _enemies)
            {
                en.setStatusIndicator(3, 0, gameplayManager.enemiesIndicators[0]);
                en.setStatusIndicator(3, 1, gameplayManager.enemiesIndicators[1]);
                    en.setStatusIndicator(3, 2, gameplayManager.enemiesIndicators[2]);
            }
            */
            //gameplayManager.CreateCard(0);

        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }
        
    }
    public override void CastOnPlay()
    {

        
        base.CastOnPlay();
        ApplyEffectToCard(3, 1, cardAlign.helpingGO.GetComponent<Card>());
    }


}