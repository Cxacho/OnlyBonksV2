using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Homerun :  Card
{

    public GameObject bonk;
    public int drugiAtak;
    private void Start()
    {
        drugiAtak = Mathf.RoundToInt(attack * 0.3f);

        desc = $"Deal <color=white>{attack.ToString()}</color> damage to first enemy and <color=white>{drugiAtak.ToString()}</color> to second enemy";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }



    private void FixedUpdate()
    {

        PlayerDamageCalculation();

        if (followMouse.en != null)
        {

            if (followMouse.en.vurneable > 0)
            {
                kalkulacjaPrzeciwnik = Mathf.RoundToInt(kalkulacja * 1.25f);
                if (kalkulacjaPrzeciwnik > defaultattack)
                    desc = $"Deal <color=green>{kalkulacjaPrzeciwnik.ToString()}</color> damage to first enemy and <color=green>{drugiAtak.ToString()}</color> to second enemy";
                else if (kalkulacjaPrzeciwnik == defaultattack)
                    desc = $"Deal <color=white>{kalkulacjaPrzeciwnik.ToString()}</color> damage to first enemy and <color=white>{drugiAtak.ToString()}</color> to second enemy";
                else if (kalkulacjaPrzeciwnik < defaultattack)
                    desc = $"Deal <color=red>{kalkulacjaPrzeciwnik.ToString()}</color> damage to first enemy and <color=red>{drugiAtak.ToString()}</color> to second enemy";
            }
            else
            {
                kalkulacjaPrzeciwnik = kalkulacja;
                if (kalkulacjaPrzeciwnik > defaultattack)
                    desc = $"Deal <color=green>{kalkulacjaPrzeciwnik.ToString()}</color> damage to first enemy and <color=green>{drugiAtak.ToString()}</color> to second enemy";
                else if (kalkulacjaPrzeciwnik == defaultattack)
                    desc = $"Deal <color=white>{kalkulacjaPrzeciwnik.ToString()}</color> damage to first enemy and <color=white>{drugiAtak.ToString()}</color> to second enemy";
                else if (kalkulacjaPrzeciwnik < defaultattack)
                    desc = $"Deal <color=red>{kalkulacjaPrzeciwnik.ToString()}</color> damage to first enemy and <color=red>{drugiAtak.ToString()}</color> to second enemy";
            }
        }
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }

    public void PlayerDamageCalculation()
    {
        if (player.strenght > 0) //gracz ma strenght wiekszy od 0
        {
            if (player.frail > 0) //gracz ma strenght i frail
            {
                kalkulacja = Mathf.RoundToInt(((attack + player.strenght) * 0.75f));
                drugiAtak = Mathf.RoundToInt(kalkulacja * 0.3f);
                if (kalkulacja > defaultattack) // dmg jest wiekszy niz podstawowy 
                    desc = $"Deal <color=green>{kalkulacja.ToString()}</color> damage to first enemy and <color=green>{drugiAtak.ToString()}</color> to second enemy";

                else if (kalkulacja == defaultattack) //dmg jest taki sam jak podstawowy
                    desc = $"Deal <color=white>{kalkulacja.ToString()}</color> damage to first enemy and <color=white>{drugiAtak.ToString()}</color> to second enemy";

                else if (kalkulacja < defaultattack) //dmg jest mniejszy niz podstawowy 
                    desc = $"Deal <color=red>{kalkulacja.ToString()}</color> damage to first enemy and <color=red>{drugiAtak.ToString()}</color> to second enemy";
            }
            else //gracz ma strenght ale nie ma fraila 
            {
                kalkulacja = (attack + player.strenght);
                drugiAtak = Mathf.RoundToInt(kalkulacja * 0.3f);
                desc = $"Deal <color=green>{kalkulacja.ToString()}</color> damage to first enemy and <color=green>{drugiAtak.ToString()}</color> to second enemy";
            }
        }
        else if (player.strenght == 0) //gracz ma strenght r�wny 0
        {

            if (player.frail > 0) //gracz ma fraila
            {
                kalkulacja = Mathf.RoundToInt((attack * 0.75f));
                drugiAtak = Mathf.RoundToInt(kalkulacja * 0.3f);
                desc = $"Deal <color=red>{kalkulacja.ToString()}</color> damage to first enemy and <color=red>{drugiAtak.ToString()}</color> to second enemy";
            }
            else // gracz nie ma fraila 
            {
                kalkulacja = attack;
                drugiAtak = Mathf.RoundToInt(kalkulacja * 0.3f);
                desc = $"Deal <color=white>{kalkulacja.ToString()}</color> damage to first enemy and <color=white>{drugiAtak.ToString()}</color> to second enemy";
            }
        }
        else if (player.strenght < 0) //gracz ma strenght mniejszy od 0
        {

            if (player.frail > 0)    //gracz ma fraila
            {
                kalkulacja = Mathf.RoundToInt(((attack + player.strenght) * 0.75f));
                drugiAtak = Mathf.RoundToInt(kalkulacja * 0.3f);
                desc = $"Deal <color=red>{kalkulacja.ToString()}</color> damage to first enemy and <color=red>{drugiAtak.ToString()}</color> to second enemy";
            }
            else // gracz nie ma fraila 
            {
                kalkulacja = (attack + player.strenght);
                drugiAtak = Mathf.RoundToInt(kalkulacja * 0.3f);
                desc = $"Deal <color=red>{kalkulacja.ToString()}</color> damage to first enemy and <color=red>{drugiAtak.ToString()}</color> to second enemy";
            }
        }
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }

    public override void OnDrop()
    {
        gameplayManager.checkPlayerMana(cost);
        if (gameplayManager.canPlayCards == true)
        {
            base.OnDrop();

            StartCoroutine(ExecuteAfterTime(1f));
            foreach (Enemy en in _enemies)
            {
                if (en.isFirstTarget == true)
                {
                    en.ReceiveDamage(attack + player.strenght);
                    en.targeted = false;
                    en.isFirstTarget = false;
                    Debug.Log("jebanyskryptv2");
                }
                if (en.isSecondTarget == true)
                {
                    en.ReceiveDamage((attack + player.strenght)*0.3f); // do zmiany po demie
                    en.targeted = false;
                    en.isSecondTarget = false;
                    Debug.Log("jebanyskrypt");
                }
               // resetTargetting();
            }
        }
        else
        {
            Debug.Log("fajnie dzia�a");
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);


        //enemy.ReceiveDamage(attack * pl.strenght);


        player.manaText.text = player.mana.ToString();


        /* else
         {
             // enemy.currentHealth = 0;
             gm.state = BattleState.WON;
             StartCoroutine(gm.OnBattleWin());

         }*/





    }

}


