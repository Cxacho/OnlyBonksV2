using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class BuyCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameplayManager gameplayManager;
    
    public int cardCost;
    
    [SerializeField]
    private int ID;

    private float PosY;
    
    //trzeba ustawiaæ ID w prefabach kart sklepowych
    private void Awake()
    {
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        //int.TryParse(this.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text, out cardCost);
        this.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = cardCost.ToString();
        PosY = gameObject.GetComponent<RectTransform>().anchoredPosition.y;
        
    }

    private void Update()
    {
        CheckIfCanBuy();
    }

    //czynnoœci po klikniêciu w kartê, aby j¹ kupiæ
    public void Buy()
    {
        
        if (gameplayManager.gold >= cardCost)
        {
            gameplayManager.gold -= cardCost;
            gameplayManager.drawDeck.Add(gameplayManager.allCardsSHOP[ID]);
            
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


