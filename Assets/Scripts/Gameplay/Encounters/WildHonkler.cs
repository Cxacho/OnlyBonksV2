using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WildHonkler : MonoBehaviour
{
    GameplayManager gm;
    EncountersHandler eh;
    Player pl;
    TextMeshProUGUI text;
    [SerializeField]GameObject thirdButton;
    private void Awake()
    {
        gm = FindObjectOfType<GameplayManager>();
        eh = FindObjectOfType<EncountersHandler>();
        pl = FindObjectOfType<Player>();
        text = gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }
    public void Run()
    {
        StartCoroutine(eh.DestroyMe());
        gm.gogo();
    }
    public void Trust(int badJudgement)
    {
        thirdButton.SetActive(true);
        var but1 = gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
            var but2= gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();
        but1.interactable = false;
        but2.interactable = false;
        pl.TakeHealthDamage(badJudgement);
        pl.setHP();
        text.text = "Creature swiftly pulled out knife from other hand, stabbing you twice in your paw. Mom always said never trust strangers.";
    }

}
