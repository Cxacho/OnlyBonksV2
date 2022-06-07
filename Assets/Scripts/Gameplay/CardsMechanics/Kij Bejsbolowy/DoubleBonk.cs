using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoubleBonk : Card
{
    public GameObject bonk;

    private void Start()
    {

        desc = $"Deal <color=white>{attack.ToString()}</color> damage twice";

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
                    desc = $"Deal <color=green>{kalkulacjaPrzeciwnik.ToString()}</color> damage twice";
                else if (kalkulacjaPrzeciwnik == defaultattack)
                    desc = $"Deal <color=white>{kalkulacjaPrzeciwnik.ToString()}</color> damage twice";
                else if (kalkulacjaPrzeciwnik < defaultattack)
                    desc = $"Deal <color=red>{kalkulacjaPrzeciwnik.ToString()}</color> damage twice";
            }
            else
            {
                kalkulacjaPrzeciwnik = kalkulacja;
                if (kalkulacjaPrzeciwnik > defaultattack)
                    desc = $"Deal <color=green>{kalkulacjaPrzeciwnik.ToString()}</color> damage twice";
                else if (kalkulacjaPrzeciwnik == defaultattack)
                    desc = $"Deal <color=white>{kalkulacjaPrzeciwnik.ToString()}</color> damage twice";
                else if (kalkulacjaPrzeciwnik < defaultattack)
                    desc = $"Deal <color=red>{kalkulacjaPrzeciwnik.ToString()}</color> damage twice";
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
                    desc = $"Deal <color=green>{kalkulacja.ToString()}</color> damage twice";

                else if (kalkulacja == defaultattack) //dmg jest taki sam jak podstawowy
                    desc = $"Deal <color=white>{kalkulacja.ToString()}</color> damage twice";

                else if (kalkulacja < defaultattack) //dmg jest mniejszy niz podstawowy 
                    desc = $"Deal <color=red>{kalkulacja.ToString()}</color> damage twice";
            }
            else //gracz ma strenght ale nie ma fraila 
            {
                kalkulacja = (attack + pl.strenght);
                desc = $"Deal <color=green>{kalkulacja.ToString()}</color> damage twice";
            }
        }
        else if (pl.strenght == 0) //gracz ma strenght równy 0
        {

            if (pl.frail > 0) //gracz ma fraila
            {
                kalkulacja = Mathf.RoundToInt((attack * 0.75f));
                desc = $"Deal <color=red>{kalkulacja.ToString()}</color> damage twice";
            }
            else // gracz nie ma fraila 
            {
                kalkulacja = attack;
                desc = $"Deal <color=white>{kalkulacja.ToString()}</color> damage twice";
            }
        }
        else if (pl.strenght < 0) //gracz ma strenght mniejszy od 0
        {

            if (pl.frail > 0)    //gracz ma fraila
            {
                kalkulacja = Mathf.RoundToInt(((attack + pl.strenght) * 0.75f));
                desc = $"Deal <color=red>{kalkulacja.ToString()}</color> damage twice";
            }
            else // gracz nie ma fraila 
            {
                kalkulacja = (attack + pl.strenght);
                desc = $"Deal <color=red>{kalkulacja.ToString()}</color> damage twice";
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
                    IEnumerator Bonking()
                    {
                        yield return new WaitForSeconds(0.15f);
                        //anim/playsfx
                        en.ReceiveDamage(attack + pl.strenght);

                        yield return new WaitForSeconds(0.2f);
                        //anim/playsfx
                        en.ReceiveDamage(attack + pl.strenght);
                    }
                    StartCoroutine(Bonking());
                    //InvokeRepeating(en.ReceiveDamage(attack + pl.strenght), 0.1f, 0.3f);

                    en.targeted = false;
                    en.isFirstTarget = false;
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