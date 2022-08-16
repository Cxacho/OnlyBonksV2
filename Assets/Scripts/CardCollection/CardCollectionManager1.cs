using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class CardCollectionManager1 : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> cardCollection = new List<GameObject>();
    public List<GameObject> cardCollectionTemp = new List<GameObject>();
    public GameObject contentPanel;
    private void Awake()
    {
        
        SortByMana();

        for (int i=0; i < cardCollection.Count; i++)
        {

            Instantiate(cardCollectionTemp[i], contentPanel.transform);
            
        }
    }

    public void SortByMana()
    {
        cardCollectionTemp = cardCollection.OrderBy(t => t.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text).ThenBy(t => t.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text).ToList();
        Debug.Log(cardCollection[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);
    }
}
