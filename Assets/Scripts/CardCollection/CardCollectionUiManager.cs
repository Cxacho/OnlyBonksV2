using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardCollectionUiManager : MonoBehaviour
{

    public CardCollectionManager cardCollectionManager;
    public GameObject[] cardSlots;

    public int page;
    public TextMeshProUGUI pageText;

    private void Start()
    {
        DisplayCards();
    }

    private void Update()
    {
        UpdatePage();

        if (Input.GetKeyDown(KeyCode.D))
        {
            if(page >= Mathf.Floor((cardCollectionManager.cards.Count - 1) / 18))
            {
                page = 0;
            }
            else
            {
                page++;
            }
            DisplayCards();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (page <= 0)
            {
                page = Mathf.FloorToInt((cardCollectionManager.cards.Count - 1) / 18);
            }
            else
            {
                page--;
            }
            DisplayCards();
        }
    }

    private void UpdatePage()
    {
        
        pageText.text = (page + 1) + "/" + (Mathf.Ceil(cardSlots.Length / 18) +1).ToString();
    }

    private void DisplayCards()
    {
        for(int i=0; i < cardCollectionManager.cards.Count; i++)
        {

            if(i >= page * 18 && i < (page+1)* 18)
            {
                cardSlots[i].gameObject.SetActive(true);
                cardSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cardCollectionManager.cards[i].cardType;
                cardSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cardCollectionManager.cards[i].cardMana.ToString();
                cardSlots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = cardCollectionManager.cards[i].cardName;
            }
            else
            {
                cardSlots[i].gameObject.SetActive(false);
            }
            
        }
    }

}
