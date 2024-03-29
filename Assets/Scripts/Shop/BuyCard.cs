using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class BuyCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameplayManager gameplayManager;

    public enum WeaponType {Baseball, Katana, Neutral };

    public WeaponType weaponType;
    public int cardCost;
    private bool rewardcard = false;
    [SerializeField]
    private int ID;

    private float PosY;
    
    //trzeba ustawia� ID w prefabach kart sklepowych
    private void Awake()
    {
        
    }
    private void Start()
    {
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        //int.TryParse(this.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text, out cardCost);
        if (gameplayManager.cardHolder.gameObject.activeInHierarchy == true)
        {
            rewardcard = true;
            cardCost = 0;
            this.transform.GetChild(4).gameObject.SetActive(false);
        }
        else
        {
            this.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = cardCost.ToString();
            PosY = gameObject.GetComponent<RectTransform>().anchoredPosition.y;
        }
    }
    private void Update()
    {
        CheckIfCanBuy();
    }

    //czynno�ci po klikni�ciu w kart�, aby j� kupi�
    public void Buy()
    {
        if(rewardcard == true)
        {
            Debug.Log(gameplayManager.allCardsSHOP[ID]);
            var cardPrefabClone = Instantiate(gameplayManager.allCardsSHOP[ID]) as GameObject;
            //cardPrefabClone.transform.SetParent(gameplayManager.canvas.transform);
            //cardPrefabClone.transform.localScale = Vector3.one;
            //cardPrefabClone.transform.position = new Vector2(-1000, 0);

            if (this.weaponType == WeaponType.Baseball)
                gameplayManager.cardsAcquiredDeckBaseball.Add(cardPrefabClone);
            else if(this.weaponType == WeaponType.Katana)
                gameplayManager.cardsAcquiredDeckKatana.Add(cardPrefabClone);
            else if (this.weaponType == WeaponType.Neutral)
                gameplayManager.cardsAcquiredDeckNeutral.Add(cardPrefabClone);

            //gameplayManager.startingDeck.Add(cardPrefabClone);
            //gameplayManager.drawDeck.Add(cardPrefabClone);
            
            for (int i = 0; i < GameObject.Find("cardHolder").transform.childCount; i++)
            {
                Destroy(GameObject.Find("cardHolder").transform.GetChild(i).gameObject);
            }
            
        }
        else if (gameplayManager.gold >= cardCost)
        {
            gameplayManager.gold -= cardCost;
            var cardPrefabClone = Instantiate(gameplayManager.allCardsSHOP[ID]);
            //cardPrefabClone.transform.SetParent(gameplayManager.canvas.transform);
            //cardPrefabClone.transform.localScale = Vector3.one;
            //cardPrefabClone.transform.position = new Vector2(-1000, 0);
            gameplayManager.startingDeck.Add(cardPrefabClone);
            gameplayManager.drawDeck.Add(cardPrefabClone);
            
            GameObject.Destroy(gameObject);
            
        }
        else
        {
            Debug.Log("Nie masz tyle kasy");
        }
        
    }

    //ustawia czerowny kolor tekstu jezeli nie stac nas na karte
    private void CheckIfCanBuy()
    {
        if (gameplayManager.gold < cardCost)
        {
            gameObject.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else
        {

            gameObject.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(PosY - 200,0.2f);
        gameObject.transform.DOScale(1.5f,0.2f);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(-150, 0.2f);
        gameObject.transform.DOScale(1, 0.2f);
        
    }
}


