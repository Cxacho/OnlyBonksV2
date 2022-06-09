using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject fiveCardsArea;
    [HideInInspector]public GameplayManager gameplayManager;

    //lista kart pojawiaj¹cych siê w sklepie w górnym panelu kart
    public List<GameObject> cards = new List<GameObject>();

    private void Awake()
    {
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        cards = gameplayManager.cards;
    }
    private void Start()
    {
        SpawnShuffledCards();
        //CheckIfCanBuy();
    }


    //shuffle danej listy
    void Shuffle<GameObject>(List<GameObject> inputList)
    {
        for (int i = 0; i < inputList.Count - 1; i++)
        {
            GameObject temp = inputList[i];
            int rand = Random.Range(i, inputList.Count);
            inputList[i] = inputList[rand];
            inputList[rand] = temp;
        }
    }

    //shuffle listy "cards", a póŸniej spawn kart z listy "cards"
    void SpawnShuffledCards()
    {
        fiveCardsArea.GetComponent<GridLayoutGroup>().enabled = true;
        Shuffle(cards);
        for (int i = 0; i < 5; i++)
        {
            Instantiate(cards[i], fiveCardsArea.transform);
        }
        Invoke("GridGroupDisable", 0.01f);
    }

    //wyl¹cza grid layout group, aby po kupieniu karty pozosta³e karty zosta³y na swoich miejscach
    void GridGroupDisable()
    {
        fiveCardsArea.GetComponent<GridLayoutGroup>().enabled = false;
    }
}


