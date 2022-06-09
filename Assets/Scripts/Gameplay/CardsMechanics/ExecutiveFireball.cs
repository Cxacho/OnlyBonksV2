using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ExecutiveFireball : Card
{
    //If enemy has less than 40% hp execute him

    //Else deal 20 dmg

    private void Start()
    {

        desc = $"If enemy has less than 40$ hp execute him <br><br>Else deal <color=white>{attack.ToString()}</color> damage";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }



    private void FixedUpdate()
    {

        PlayerDamageCalculation();

        if (fm.en != null)
        {

            if (fm.en.vurneable > 0)
            {
                kalkulacjaPrzeciwnik = Mathf.RoundToInt(kalkulacja * 1.25f);
                if (kalkulacjaPrzeciwnik > defaultattack)
                    desc = $"If enemy has less than 40$ hp execute him <br><br>Else deal <color=green>{kalkulacjaPrzeciwnik.ToString()}</color> damage";
                else if (kalkulacjaPrzeciwnik == defaultattack)
                    desc = $"If enemy has less than 40$ hp execute him <br><br>Else deal <color=white>{kalkulacjaPrzeciwnik.ToString()}</color> damage";
                else if (kalkulacjaPrzeciwnik < defaultattack)
                    desc = $"If enemy has less than 40$ hp execute him <br><br>Else deal <color=red>{kalkulacjaPrzeciwnik.ToString()}</color> damage";
            }
            else
            {
                kalkulacjaPrzeciwnik = kalkulacja;
                if (kalkulacjaPrzeciwnik > defaultattack)
                    desc = $"If enemy has less than 40$ hp execute him <br><br>Else deal <color=green>{kalkulacjaPrzeciwnik.ToString()}</color> damage";
                else if (kalkulacjaPrzeciwnik == defaultattack)
                    desc = $"If enemy has less than 40$ hp execute him <br><br>Else deal <color=white>{kalkulacjaPrzeciwnik.ToString()}</color> damage";
                else if (kalkulacjaPrzeciwnik < defaultattack)
                    desc = $"If enemy has less than 40$ hp execute him <br><br>Else deal <color=red>{kalkulacjaPrzeciwnik.ToString()}</color> damage";
            }
        }
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }

    public void PlayerDamageCalculation()
    {
        if (pl.strenght > 0) //gracz ma strenght wiekszy od 0
        {
            if (pl.frail > 0) //gracz ma strenght i frail
            {
                kalkulacja = Mathf.RoundToInt(((attack + pl.strenght) * 0.75f));
                if (kalkulacja > defaultattack) // dmg jest wiekszy niz podstawowy 
                    desc = $"If enemy has less than 40$ hp execute him <br><br>Else deal <color=green>{kalkulacja.ToString()}</color> damage";

                else if (kalkulacja == defaultattack) //dmg jest taki sam jak podstawowy
                    desc = $"If enemy has less than 40$ hp execute him <br><br>Else deal <color=white>{kalkulacja.ToString()}</color> damage";

                else if (kalkulacja < defaultattack) //dmg jest mniejszy niz podstawowy 
                    desc = $"If enemy has less than 40$ hp execute him <br><br>Else deal <color=red>{kalkulacja.ToString()}</color> damage";
            }
            else //gracz ma strenght ale nie ma fraila 
            {
                kalkulacja = (attack + pl.strenght);
                desc = $"If enemy has less than 40$ hp execute him <br><br>Else deal <color=green>{kalkulacja.ToString()}</color> damage";
            }
        }
        else if (pl.strenght == 0) //gracz ma strenght równy 0
        {

            if (pl.frail > 0) //gracz ma fraila
            {
                kalkulacja = Mathf.RoundToInt((attack * 0.75f));
                desc = $"If enemy has less than 40$ hp execute him <br><br>Else deal <color=red>{kalkulacja.ToString()}</color> damage";
            }
            else // gracz nie ma fraila 
            {
                kalkulacja = attack;
                desc = $"If enemy has less than 40$ hp execute him <br><br>Else deal <color=white>{kalkulacja.ToString()}</color> damage";
            }
        }
        else if (pl.strenght < 0) //gracz ma strenght mniejszy od 0
        {

            if (pl.frail > 0)    //gracz ma fraila
            {
                kalkulacja = Mathf.RoundToInt(((attack + pl.strenght) * 0.75f));
                desc = $"If enemy has less than 40$ hp execute him <br><br>Else deal <color=red>{kalkulacja.ToString()}</color> damage";
            }
            else // gracz nie ma fraila 
            {
                kalkulacja = (attack + pl.strenght);
                desc = $"If enemy has less than 40$ hp execute him <br><br>Else deal <color=red>{kalkulacja.ToString()}</color> damage";
            }
        }
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }

    public override void OnDrop()
    {
        gm.checkPlayerMana(cost);
        if (gm.canPlayCards == true)
        {
            base.OnDrop();

            StartCoroutine(ExecuteAfterTime(1f));
            foreach (Enemy en in _enemies)
            {
                if (en.targeted == true)
                {
                    Debug.Log("jba");
                    if (en._currentHealth<(float)(en.maxHealth*0.4))
                    {
                        en.ReceiveDamage(en.maxHealth);
                    }
                    else 
                    {
                        en.ReceiveDamage(attack+pl.strenght);
                    }
                    en.targeted = false;
                }
            }
        }
        else
        {
            Debug.Log("fajnie dzia³a");
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);


        //enemy.ReceiveDamage(attack * pl.strenght);


        pl.manaText.text = pl.mana.ToString();


        /* else
         {
             // enemy.currentHealth = 0;
             gm.state = BattleState.WON;
             StartCoroutine(gm.OnBattleWin());

         }*/





    }

}
