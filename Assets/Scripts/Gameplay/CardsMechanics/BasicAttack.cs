using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class BasicAttack : Card
{
    public int attack = 3;

    private int cost = 1;
    public GameObject bonk;

    string desc;
    private TextMeshPro textMeshPro;

    private void Start()
    {
        desc = $"Deal <color=blue>{attack.ToString()}</color> damage";

        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
    }
    public void UpdateAttack()
    {
        attack += pl.strenght;
        if (attack > 3)
        {
            desc = $"Deal <color=green>{attack.ToString()}</color> damage";
            this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
        }
        else if (attack < 3)
        {
            desc = $"Deal <color=red>{attack.ToString()}</color> damage";
            this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = desc;
        }
    }
    
    public override void OnDrop()
    {
        gm.checkPlayerMana(cost);
        if (gm.canPlayCards == true)
        {
            base.OnDrop();

            Instantiate(bonk, new Vector3(0, -10, 0), Quaternion.identity, GameObject.Find("Player").transform);
            StartCoroutine(ExecuteAfterTime(1f));
            foreach (Enemy en in _enemies)
            {
                if (en.targeted == true)
                {
                    en.ReceiveDamage(attack + pl.strenght);

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

        pl.manaText.text = pl.mana.ToString();

    }


}
