using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardCollectionClicked : MonoBehaviour
{
    public GameObject cardClickedPanel;
    private GameObject cardName;
    private GameObject cardClicked;
    public void DisplayClickedCard()
    {
        
        cardClickedPanel.SetActive(true);
        cardName = this.gameObject;
        Debug.Log(cardName);
        cardClicked = Instantiate(cardName,cardClickedPanel.transform);

        cardClicked.GetComponent<RectTransform>().sizeDelta = new Vector2(220*3f, 340*3f);
        cardClicked.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        cardClicked.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        cardClicked.GetComponent<Button>().interactable = false; //zmieniæ kolor button disabled w inspektorze unity!

    }
}
