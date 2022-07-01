using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DefensiveAttack : Card
{
    public int armor = 3;

    public GameObject shield;

    private void Start()
    {
        Debug.Log("Massive" + attack);

        desc = $"Deal <color=white>{attack.ToString()}</color> damage,Gives 3 armor";

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
                    desc = $"Deal <color=green>{kalkulacjaPrzeciwnik.ToString()}</color> damage, Gives 3 armor";
                else if (kalkulacjaPrzeciwnik == defaultattack)
                    desc = $"Deal <color=white>{kalkulacjaPrzeciwnik.ToString()}</color> damage, Gives 3 armor";
                else if (kalkulacjaPrzeciwnik < defaultattack)
                    desc = $"Deal <color=red>{kalkulacjaPrzeciwnik.ToString()}</color> damage, Gives 3 armor";
            }
            else
            {
                kalkulacjaPrzeciwnik = kalkulacja;
                if (kalkulacjaPrzeciwnik > defaultattack)
                    desc = $"Deal <color=green>{kalkulacjaPrzeciwnik.ToString()}</color> damage, Gives 3 armor";
                else if (kalkulacjaPrzeciwnik == defaultattack)
                    desc = $"Deal <color=white>{kalkulacjaPrzeciwnik.ToString()}</color> damage, Gives 3 armor";
                else if (kalkulacjaPrzeciwnik < defaultattack)
                    desc = $"Deal <color=red>{kalkulacjaPrzeciwnik.ToString()}</color> damage, Gives 3 armor";
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
                if (kalkulacja > defaultattack) // dmg jest wiekszy niz podstawowy 
                    desc = $"Deal <color=green>{kalkulacja.ToString()}</color> damage, Gives 3 armor";

                else if (kalkulacja == defaultattack) //dmg jest taki sam jak podstawowy
                    desc = $"Deal <color=white>{kalkulacja.ToString()}</color> damage, Gives 3 armor";

                else if (kalkulacja < defaultattack) //dmg jest mniejszy niz podstawowy 
                    desc = $"Deal <color=red>{kalkulacja.ToString()}</color> damage, Gives 3 armor";
            }
            else //gracz ma strenght ale nie ma fraila 
            {
                kalkulacja = (attack + player.strenght);
                desc = $"Deal <color=green>{kalkulacja.ToString()}</color> damage, Gives 3 armor";
            }
        }
        else if (player.strenght == 0) //gracz ma strenght równy 0
        {

            if (player.frail > 0) //gracz ma fraila
            {
                kalkulacja = Mathf.RoundToInt((attack * 0.75f));
                desc = $"Deal <color=red>{kalkulacja.ToString()}</color> damage, Gives 3 armor";
            }
            else // gracz nie ma fraila 
            {
                kalkulacja = attack;
                desc = $"Deal <color=white>{kalkulacja.ToString()}</color> damage, Gives 3 armor";
            }
        }
        else if (player.strenght < 0) //gracz ma strenght mniejszy od 0
        {

            if (player.frail > 0)    //gracz ma fraila
            {
                kalkulacja = Mathf.RoundToInt(((attack + player.strenght) * 0.75f));
                desc = $"Deal <color=red>{kalkulacja.ToString()}</color> damage, Gives 3 armor";
            }
            else // gracz nie ma fraila 
            {
                kalkulacja = (attack + player.strenght);
                desc = $"Deal <color=red>{kalkulacja.ToString()}</color> damage, Gives 3 armor";
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
                if (en.targeted == true)
                {
                    en.ReceiveDamage(attack + player.strenght);

                    en.targeted = false;

                    if (player.armor == 0)
                    {
                        Instantiate(shield, new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, 0), Quaternion.identity, GameObject.Find("Player").transform);
                    }
                    player.armor += armor;
                }
            }
            resetTargetting();

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


        player.manaText.text = player.mana.ToString();


        /* else
         {
             // enemy.currentHealth = 0;
             gm.state = BattleState.WON;
             StartCoroutine(gm.OnBattleWin());

         }*/





    }
}
