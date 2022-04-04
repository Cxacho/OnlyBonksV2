using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[System.Serializable]
public class CardCollection : MonoBehaviour
{

    public string cardName;
    public int cardMana;

    public string cardType;
    public string cardDesc;
    public string cardAction;
    public Sprite cardSprite;

    private void Awake()
    {
        
        cardType = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        int.TryParse(gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text, out cardMana);
        cardName = gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
        cardAction = gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text;
        cardDesc = gameObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text;

        

    }

}
