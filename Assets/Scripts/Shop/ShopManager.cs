using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject fiveCardsArea;
    public GameplayManager gameplayManager;

    //lista kart pojawiaj¹cych siê w sklepie w górnym panelu kart
    public List<GameObject> cards = new List<GameObject>();

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

    //shuffle, a póŸniej spawn kart z listy "cards"
    void SpawnShuffledCards()
    {
        Shuffle(cards);
        for (int i = 0; i < 3; i++)
        {
            Instantiate(cards[i], fiveCardsArea.transform);
        }
    }


    }


