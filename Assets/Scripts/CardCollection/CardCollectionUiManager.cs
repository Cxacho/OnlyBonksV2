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

    [SerializeField] private bool isSearch;
    [SerializeField] private int totalNumbers;
    [SerializeField] private int currentSearchMana;

    [SerializeField] private bool isSearchByMana;
    [SerializeField] private bool isSearchByClass;
    [SerializeField] private string currentSearchClass;

    private void Start()
    {
        DisplayCards();
        
    }

    private void Update()
    {
        UpdatePage();

        if (!isSearch)
            totalNumbers = 0;
    }
    
    public void SearchByMana(int _mana)
    {
        isSearchByMana = true;
        isSearchByClass = false;

        isSearch = true;
        totalNumbers = 0;
        page = 0;
        currentSearchMana = _mana;
        _ = new List<CardCollection>();
        List<CardCollection> cards = ReturnCard(_mana);

        for (int i=0; i < cardSlots.Length; i++)
        {
            cardSlots[i].gameObject.SetActive(false);
        }
        for(int i=0; i < cards.Count; i++)
        {
            if(i >= page * 18 && i < (page + 1) * 18)
            {
                totalNumbers++;

                cardSlots[i].gameObject.SetActive(true);
                cardSlots[i].gameObject.GetComponent<Image>().sprite = cards[i].cardSprite;
                cardSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cards[i].cardType;
                cardSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cards[i].cardMana.ToString();
                cardSlots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = cards[i].cardName;
                cardSlots[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = cards[i].cardAction;
                cardSlots[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = cards[i].cardDesc;
            }
            else
            {
                cardSlots[i].gameObject.SetActive(false);
            }
        }
    }

    private List<CardCollection> ReturnCard(int _mana)
    {
        List<CardCollection> cards = new List<CardCollection>();

        for(int i=0; i< cardCollectionManager.cards.Count; i++)
        {
            CardCollection card;

            if(_mana < 5)
            {
                if(cardCollectionManager.cards[i].cardMana == _mana)
                {
                    card = cardCollectionManager.cards[i];
                    cards.Add(card);
                }
            }
           
        }
        Debug.Log("Cards with that mana : " + cards.Count);
        return cards;
    }

    public void SearchByClass(string _cardClass)
    {
        isSearchByMana = false;
        isSearchByClass = true;

        isSearch = true;
        totalNumbers = 0;
        page = 0;
        currentSearchClass = _cardClass;
        _ = new List<CardCollection>();
        List<CardCollection> cards = ReturnCard(_cardClass);

        for (int i = 0; i < cardSlots.Length; i++)
        {
            cardSlots[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < cards.Count; i++)
        {
            if (i >= page * 18 && i < (page + 1) * 18)
            {
                totalNumbers++;

                cardSlots[i].gameObject.SetActive(true);
                cardSlots[i].gameObject.GetComponent<Image>().sprite = cards[i].cardSprite;
                cardSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cards[i].cardType;
                cardSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cards[i].cardMana.ToString();
                cardSlots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = cards[i].cardName;
                cardSlots[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = cards[i].cardAction;
                cardSlots[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = cards[i].cardDesc;
            }
            else
            {
                cardSlots[i].gameObject.SetActive(false);
            }
        }
    }

    private List<CardCollection> ReturnCard(string _cardClass)
    {
        List<CardCollection> cards = new List<CardCollection>();

        for (int i = 0; i < cardCollectionManager.cards.Count; i++)
        {
            CardCollection card;
           
            if (cardCollectionManager.cards[i].cardType.ToString() == _cardClass)
                {
                    card = cardCollectionManager.cards[i];
                    cards.Add(card);
                }

        }
        Debug.Log("Cards with that mana : " + cards.Count);
        return cards;
    }

    public void InitialCardsTab()
    {
        page = 0;
        isSearch = false;
        DisplayCards();
    }

    private void UpdatePage()
    {
        if (!isSearch)
        {
            pageText.text = (page + 1) + "/" + (Mathf.Ceil(cardSlots.Length / 18) + 1).ToString();
        }
        else
        {
            pageText.text = (page + 1) + "/" + (Mathf.Ceil(totalNumbers / 18) + 1).ToString();
        }
        
    }

    public void NextPage()
    {
        UpdatePage();
        if (!isSearch)
        {
            if (page >= Mathf.FloorToInt((cardCollectionManager.cards.Count - 1) / 18))
            {
                page = 0;
            }
            else
            {
                page++;
            }

            DisplayCards();
        }
        else
        {
            if (isSearchByMana)
            {
                if(page >= (Mathf.FloorToInt(totalNumbers / 18)))
                {
                    page = 0;
                }
                else
                {
                    page++;
                }
                DisplayBySearchMana();
            }
            if (isSearchByClass)
            {
                if (page >= (Mathf.FloorToInt(totalNumbers / 18)))
                {
                    page = 0;
                }
                else
                {
                    page++;
                }
                DisplayBySearchClass();
            }
        }
        

    }

    public void PreviousPage()
    {
        UpdatePage();
        if (!isSearch)
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
        else
        {
            if (isSearchByMana)
            {
                if (page <=0)
                {
                    page = (Mathf.FloorToInt(totalNumbers / 18 ));
                }
                else
                {
                    page--;
                }
                DisplayBySearchMana();
            }
            if (isSearchByClass)
            {
                if (page <= 0)
                {
                    page = (Mathf.FloorToInt(totalNumbers / 18));
                }
                else
                {
                    page--;
                }
                DisplayBySearchClass();
            }
        }

    }
   
    private void DisplayCards()
    {
        for(int i=0; i < cardCollectionManager.cards.Count; i++)
        {

            if(i >= page * 18 && i < (page + 1)* 18)
            {
                DisplaySingleCard(i);
            }
            else
            {
                cardSlots[i].gameObject.SetActive(false);
            }
            
        }
    }

    private void DisplaySingleCard(int i)
    {
        totalNumbers++;

        cardSlots[i].gameObject.SetActive(true);
        cardSlots[i].gameObject.GetComponent<Image>().sprite = cardCollectionManager.cards[i].cardSprite;
        cardSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cardCollectionManager.cards[i].cardType;
        cardSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cardCollectionManager.cards[i].cardMana.ToString();
        cardSlots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = cardCollectionManager.cards[i].cardName;
        cardSlots[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = cardCollectionManager.cards[i].cardAction;
        cardSlots[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = cardCollectionManager.cards[i].cardDesc;
    }
    
    private void DisplayBySearchMana()
    {
        List<CardCollection> cards = new List<CardCollection>();
        cards = ReturnCard(currentSearchMana);

        for(int i=0; i < cardSlots.Length; i++)
        {
            cardSlots[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < cards.Count; i++)
        {
            if(i >= page * 18 && i < (page + 1) * 18)
            {
                cardSlots[i].gameObject.SetActive(true);

                cardSlots[i].gameObject.GetComponent<Image>().sprite = cards[i].cardSprite;
                cardSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cards[i].cardType;
                cardSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cards[i].cardMana.ToString();
                cardSlots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = cards[i].cardName;
                cardSlots[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = cards[i].cardAction;
                cardSlots[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = cards[i].cardDesc;
            }
            else
            {
                cardSlots[i].gameObject.SetActive(false);
            }
        }
    }
    private void DisplayBySearchClass()
    {
        List<CardCollection> cards = new List<CardCollection>();
        cards = ReturnCard(currentSearchClass);

        for (int i = 0; i < cardSlots.Length; i++)
        {
            cardSlots[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < cards.Count; i++)
        {
            if (i >= page * 18 && i < (page + 1) * 18)
            {
                cardSlots[i].gameObject.SetActive(true);

                cardSlots[i].gameObject.GetComponent<Image>().sprite = cards[i].cardSprite;
                cardSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cards[i].cardType;
                cardSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cards[i].cardMana.ToString();
                cardSlots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = cards[i].cardName;
                cardSlots[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = cards[i].cardAction;
                cardSlots[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = cards[i].cardDesc;
            }
            else
            {
                cardSlots[i].gameObject.SetActive(false);
            }
        }
    }
}
