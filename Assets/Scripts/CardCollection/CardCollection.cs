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

    private void Awake()
    {
        cardType = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        int.TryParse(gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text, out cardMana);
        cardName = gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;

        

    }

}
