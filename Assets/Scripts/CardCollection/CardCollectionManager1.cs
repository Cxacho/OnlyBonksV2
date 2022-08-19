using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using DG.Tweening;

public class CardCollectionManager1 : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> cardCollection = new List<GameObject>();
    public List<GameObject> cardCollectionTemp = new List<GameObject>();
    public GameObject contentPanel;
    public GameObject weaponsPanel;
    public GameObject openWeaponsPanelButton;
    public GameObject closeWeaponsPanelButton;
    [HideInInspector]public string currentWeapon;
    private void Awake()
    {

        cardCollectionTemp = cardCollection.OrderBy(t => t.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text).ThenBy(t => t.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text).ToList();

        for (int i=0; i < cardCollection.Count; i++)
        {

            Instantiate(cardCollectionTemp[i], contentPanel.transform);
            
        }
    }

    public void ChooseWeaponCollection()
    {
        Debug.Log(currentWeapon);

        switch (currentWeapon)
        {
            case "Palka":
                
                for (int i = 0; i < cardCollectionTemp.Count; i++)
                {

                    Destroy(contentPanel.transform.GetChild(i).gameObject);

                }

                cardCollectionTemp.RemoveRange(0, cardCollectionTemp.Count);
                
                foreach (GameObject card in cardCollection)
                {
                    Debug.Log(card.GetComponent<CardInCollection>().cardCollectionWeapon.ToString());
                    if(card.GetComponent<CardInCollection>().cardCollectionWeapon.ToString() == "Palka")
                    {

                        cardCollectionTemp.Add(card);

                    }

                }
                
                
                
                for (int i = 0; i < cardCollectionTemp.Count; i++)
                {

                    Instantiate(cardCollectionTemp[i], contentPanel.transform);
                
                }
                break;
            case "Kostur":
                for (int i = 0; i < cardCollectionTemp.Count; i++)
                {

                    Destroy(contentPanel.transform.GetChild(i).gameObject);

                }

                cardCollectionTemp.RemoveRange(0, cardCollectionTemp.Count);

                foreach (GameObject card in cardCollection)
                {
                    Debug.Log(card.GetComponent<CardInCollection>().cardCollectionWeapon.ToString());
                    if (card.GetComponent<CardInCollection>().cardCollectionWeapon.ToString() == "Kostur")
                    {

                        cardCollectionTemp.Add(card);

                    }

                }

                

                for (int i = 0; i < cardCollectionTemp.Count; i++)
                {

                    Instantiate(cardCollectionTemp[i], contentPanel.transform);

                }
                break;
            case "Ksiazka":
                for (int i = 0; i < cardCollectionTemp.Count; i++)
                {

                    Destroy(contentPanel.transform.GetChild(i).gameObject);

                }

                cardCollectionTemp.RemoveRange(0, cardCollectionTemp.Count);

                foreach (GameObject card in cardCollection)
                {
                    Debug.Log(card.GetComponent<CardInCollection>().cardCollectionWeapon.ToString());
                    if (card.GetComponent<CardInCollection>().cardCollectionWeapon.ToString() == "Ksiazka")
                    {

                        cardCollectionTemp.Add(card);

                    }

                }

                

                for (int i = 0; i < cardCollectionTemp.Count; i++)
                {

                    Instantiate(cardCollectionTemp[i], contentPanel.transform);

                }
                break;
            case "Alchemia":
                for (int i = 0; i < cardCollectionTemp.Count; i++)
                {

                    Destroy(contentPanel.transform.GetChild(i).gameObject);

                }

                cardCollectionTemp.RemoveRange(0, cardCollectionTemp.Count);

                foreach (GameObject card in cardCollection)
                {
                    Debug.Log(card.GetComponent<CardInCollection>().cardCollectionWeapon.ToString());
                    if (card.GetComponent<CardInCollection>().cardCollectionWeapon.ToString() == "Alchemia")
                    {

                        cardCollectionTemp.Add(card);

                    }

                }

                

                for (int i = 0; i < cardCollectionTemp.Count; i++)
                {

                    Instantiate(cardCollectionTemp[i], contentPanel.transform);

                }
                break;
            case "Topor":
                for (int i = 0; i < cardCollectionTemp.Count; i++)
                {

                    Destroy(contentPanel.transform.GetChild(i).gameObject);

                }

                cardCollectionTemp.RemoveRange(0, cardCollectionTemp.Count);

                foreach (GameObject card in cardCollection)
                {
                    Debug.Log(card.GetComponent<CardInCollection>().cardCollectionWeapon.ToString());
                    if (card.GetComponent<CardInCollection>().cardCollectionWeapon.ToString() == "Topor")
                    {

                        cardCollectionTemp.Add(card);

                    }

                }

                

                for (int i = 0; i < cardCollectionTemp.Count; i++)
                {

                    Instantiate(cardCollectionTemp[i], contentPanel.transform);

                }
                break;
            case "Katana":
                for (int i = 0; i < cardCollectionTemp.Count; i++)
                {

                    Destroy(contentPanel.transform.GetChild(i).gameObject);

                }

                cardCollectionTemp.RemoveRange(0, cardCollectionTemp.Count);

                foreach (GameObject card in cardCollection)
                {
                    Debug.Log(card.GetComponent<CardInCollection>().cardCollectionWeapon.ToString());
                    if (card.GetComponent<CardInCollection>().cardCollectionWeapon.ToString() == "Katana")
                    {

                        cardCollectionTemp.Add(card);

                    }

                }

                

                for (int i = 0; i < cardCollectionTemp.Count; i++)
                {

                    Instantiate(cardCollectionTemp[i], contentPanel.transform);

                }
                break;
            case "Brak":
                for (int i = 0; i < cardCollectionTemp.Count; i++)
                {

                    Destroy(contentPanel.transform.GetChild(i).gameObject);

                }

                cardCollectionTemp.RemoveRange(0, cardCollectionTemp.Count);

                foreach (GameObject card in cardCollection)
                {
                    Debug.Log(card.GetComponent<CardInCollection>().cardCollectionWeapon.ToString());
                    if (card.GetComponent<CardInCollection>().cardCollectionWeapon.ToString() == "Brak")
                    {

                        cardCollectionTemp.Add(card);

                    }

                }

                

                for (int i = 0; i < cardCollectionTemp.Count; i++)
                {

                    Instantiate(cardCollectionTemp[i], contentPanel.transform);

                }
                break;
            case "Wszystkie":
                for (int i = 0; i < cardCollectionTemp.Count; i++)
                {

                    Destroy(contentPanel.transform.GetChild(i).gameObject);

                }

                cardCollectionTemp.RemoveRange(0, cardCollectionTemp.Count);

                cardCollectionTemp = cardCollection;
                

                for (int i = 0; i < cardCollectionTemp.Count; i++)
                {

                    Instantiate(cardCollectionTemp[i], contentPanel.transform);

                }

                break;
            default:
                break;
        }
    }

    public void SortByMana()
    {
        for (int i = 0; i < cardCollectionTemp.Count; i++)
        {

            Destroy(contentPanel.transform.GetChild(i).gameObject);

        }
        cardCollectionTemp = cardCollectionTemp.OrderBy(t => t.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text).ThenBy(t => t.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text).ToList();
        for (int i = 0; i < cardCollectionTemp.Count; i++)
        {

            Instantiate(cardCollectionTemp[i], contentPanel.transform);

        }
    }

    public void SortByName()
    {
        for (int i = 0; i < cardCollectionTemp.Count; i++)
        {

            Destroy(contentPanel.transform.GetChild(i).gameObject);

        }
        cardCollectionTemp = cardCollectionTemp.OrderBy(t => t.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text).ThenBy(t => t.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text).ToList();
        for (int i = 0; i < cardCollectionTemp.Count; i++)
        {

            Instantiate(cardCollectionTemp[i], contentPanel.transform);

        }
    }
    public void SortByType()
    {
        for (int i = 0; i < cardCollectionTemp.Count; i++)
        {

            Destroy(contentPanel.transform.GetChild(i).gameObject);

        }
        cardCollectionTemp = cardCollectionTemp.OrderBy(t => t.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text).ThenBy(t => t.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text).ThenBy(t => t.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text).ToList();
        for (int i = 0; i < cardCollectionTemp.Count; i++)
        {

            Instantiate(cardCollectionTemp[i], contentPanel.transform);

        }
    }

    public void OpenWeaponPanel()
    {
        weaponsPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-839.26f,0), 1f);
        openWeaponsPanelButton.SetActive(false);
        closeWeaponsPanelButton.SetActive(true);
    }
    public void CloseWeaponPanel()
    {
        weaponsPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-1079.7f, 0), 1f);
        openWeaponsPanelButton.SetActive(true);
        closeWeaponsPanelButton.SetActive(false);
    }
}
