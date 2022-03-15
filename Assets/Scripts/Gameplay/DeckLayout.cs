using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckLayout : MonoBehaviour
{
    public Transform deckScreen;

    public GameplayManager gm;

    public GameObject deckPanel;
    public GameObject drawPanel;
    
    void Start()
    {
       
    }
    void DisplayDeck()
    {

        //spawn kart do decku
        for (int i = 0; i < gm.startingDeck.Count; i++)
        {
            if(deckPanel.activeSelf)
            Instantiate(gm.startingDeck[i], GameObject.FindGameObjectWithTag("Panel").transform);
            
            
        }
    }
    


}
