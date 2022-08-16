using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollectionManager1 : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> cardCollection = new List<GameObject>();
    public GameObject contentPanel;
    private void Start()
    {
        for(int i=0; i < cardCollection.Count; i++)
        {
            
            cardCollection[i].GetComponent<Card>().enabled = false;
            Instantiate(cardCollection[i], contentPanel.transform);
            
        }
    }
}
