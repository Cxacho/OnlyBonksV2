 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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
    [SerializeField] private string currentSearchName;

    [SerializeField] private string searchName;
    public TMP_InputField searchInput;
    [SerializeField] private bool isSearchByInput;
    private int ilosc = 0;

    private void Start()
    {
        DisplayCards();
        UpdatePageUI();
    }

    private void Update()
    {
        

        if (!isSearch)
            totalNumbers = 0;
    }
    
    public void SearchByMana(int _mana)
    {
        isSearchByMana = true;
        isSearchByClass = false;
        isSearchByInput = false;

        isSearch = true;
        totalNumbers = 0;
        page = 0;
        currentSearchMana = _mana;
        _ = new List<CardCollection>();
        List<CardCollection> cards = ReturnCard(_mana);

        DisplayCardWhenPressButton(cards);
        UpdatePageUI();
    }
   
    public void SearchByClass(string _cardClass)
    {
        isSearchByMana = false;
        isSearchByClass = true;
        isSearchByInput = false;

        isSearch = true;
        totalNumbers = 0;
        page = 0;
        currentSearchClass = _cardClass;
        _ = new List<CardCollection>();
        List<CardCollection> cards = ReturnCard(_cardClass);

        DisplayCardWhenPressButton(cards);
        UpdatePageUI();
    }

    public void SearchByInput(string _cardName)
    {
        isSearchByInput = true;
        isSearchByMana = false;
        isSearchByClass = false;

        isSearch = true;
        totalNumbers = 0;
        page = 0;
        currentSearchName = _cardName;
        _ = new List<CardCollection>();
        List<CardCollection> cards = ReturnCards(_cardName);
        if (searchInput.text == "")
        {
            InitialCardsTab();
        }
        else
        {
            DisplayCardWhenPressButton(cards);
        }
        
        UpdatePageUI();
        //searchName = searchInput.text;


        /*if (searchName == "")
        {
            InitialCardsTab();
        }
        else
        {
            for (int i = 0; i < cardSlots.Length; i++)
            {
                cardSlots[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < cardCollectionManager.cards.Count; i++)
            {
                if (searchName.ToUpper() == cardCollectionManager.cards[i].cardName.ToUpper())
                {


                    cardSlots[i].gameObject.SetActive(true);

                    cardSlots[i].gameObject.GetComponent<Image>().sprite = cardCollectionManager.cards[i].cardSprite;
                    cardSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cardCollectionManager.cards[i].cardType;
                    cardSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cardCollectionManager.cards[i].cardMana.ToString();
                    cardSlots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = cardCollectionManager.cards[i].cardName;
                    cardSlots[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = cardCollectionManager.cards[i].cardAction;
                    cardSlots[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = cardCollectionManager.cards[i].cardDesc;

                }
            }

            pageText.text = "1/1";
        }*/
    }

    public void InitialCardsTab()
    {
        page = 0;
        isSearch = false;
        DisplayCards();

        isSearchByMana = false;
        isSearchByClass = false;
        isSearchByInput = false;

        UpdatePageUI();
    }

    private void UpdatePageUI()
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
        UpdatePageUI();
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
            UpdatePageUI();
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
                UpdatePageUI();
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
                UpdatePageUI();
            }
            if (isSearchByInput)
            {
                if (page >= (Mathf.FloorToInt(totalNumbers / 18)))
                {
                    page = 0;
                }
                else
                {
                    page++;
                }
                DisplayBySearchInput();
                UpdatePageUI();
            }
        }
        

    }

    public void PreviousPage()
    {
        UpdatePageUI();
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
            UpdatePageUI();
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
                UpdatePageUI();
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
                UpdatePageUI();
            }
            if (isSearchByInput)
            {
                if (page <= 0)
                {
                    page = (Mathf.FloorToInt(totalNumbers / 18));
                }
                else
                {
                    page--;
                }
                DisplayBySearchInput();
                UpdatePageUI();
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
    
    private void DisplayCardWhenPressButton(List<CardCollection> _cards)
    {

        for (int i = 0; i < cardSlots.Length; i++)
        {
            cardSlots[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _cards.Count; i++)
        {
            if (i >= page * 18 && i < (page + 1) * 18)
            {
                totalNumbers++;

                cardSlots[i].gameObject.SetActive(true);
                cardSlots[i].gameObject.GetComponent<Image>().sprite = _cards[i].cardSprite;
                cardSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _cards[i].cardType;
                cardSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _cards[i].cardMana.ToString();
                cardSlots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = _cards[i].cardName;
                cardSlots[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = _cards[i].cardAction;
                cardSlots[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = _cards[i].cardDesc;


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

        for (int i = 0; i < cardCollectionManager.cards.Count; i++)
        {
            CardCollection card;

            if (_mana < 5)
            {
                if (cardCollectionManager.cards[i].cardMana == _mana)
                {
                    card = cardCollectionManager.cards[i];
                    cards.Add(card);
                }
            }

        }
        return cards;
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
        return cards;
    }

    private List<CardCollection> ReturnCards(string _cardName)
    {
        List<CardCollection> cards = new List<CardCollection>();

        

        for (int i = 0; i < cardCollectionManager.cards.Count; i++)
        {
            CardCollection card;
            if(cardCollectionManager.cards[i].cardName.ToUpper() == searchInput.text.ToUpper())
            {
                card = cardCollectionManager.cards[i];
                cards.Add(card);
            }
        }

            return cards;
    }

    private void DisplayBySearchMana()
    {
        List<CardCollection> cards = new List<CardCollection>();
        cards = ReturnCard(currentSearchMana);
        DisplayCardBySearch(cards);
        UpdatePageUI();
    }
    private void DisplayBySearchClass()
    {
        List<CardCollection> cards = new List<CardCollection>();
        cards = ReturnCard(currentSearchClass);
        
        DisplayCardBySearch(cards);
        UpdatePageUI();
    }

    private void DisplayBySearchInput()
    {
        List<CardCollection> cards = new List<CardCollection>();
        cards = ReturnCards(searchInput.text);
        DisplayCardBySearch(cards);
        UpdatePageUI();
    }
    private void DisplayCardBySearch(List <CardCollection> _cards)
    {

        for (int i = 0; i < cardSlots.Length; i++)
        {
            cardSlots[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < _cards.Count; i++)
        {
            
            if (i >= page * 18 && i < (page + 1) * 18)
            {
                cardSlots[i].gameObject.SetActive(true);

                cardSlots[i].gameObject.GetComponent<Image>().sprite = _cards[i].cardSprite;
                cardSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _cards[i].cardType;
                cardSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _cards[i].cardMana.ToString();
                cardSlots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = _cards[i].cardName;
                cardSlots[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = _cards[i].cardAction;
                cardSlots[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = _cards[i].cardDesc;
            }
            else
            {
                cardSlots[i].gameObject.SetActive(false);
            }
        }

    }

    
}
