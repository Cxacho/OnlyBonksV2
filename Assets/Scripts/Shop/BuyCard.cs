using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyCard : MonoBehaviour
{
    private GameplayManager gameplayManager;
    public int cardCost;
    [SerializeField]
    private int ID;
    //trzeba ustawiaæ ID w prefabach kart sklepowych
    private void Awake()
    {
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        int.TryParse(this.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text, out cardCost);

    }

    private void Update()
    {
        CheckIfCanBuy();
    }

    public void Buy()
    {
        
        if (gameplayManager.gold >= cardCost)
        {
            gameplayManager.gold -= cardCost;
            gameplayManager.startingDeck.Add(gameplayManager.allCards[ID]);
            
            GameObject.Destroy(gameObject);
            
        }
        else
        {
            Debug.Log("Nie masz tyle kasy");
        }
        
    }

    private void CheckIfCanBuy()
    {
        if (gameplayManager.gold < cardCost)
        {
            gameObject.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else
        {

            gameObject.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;

        }
    }
}


