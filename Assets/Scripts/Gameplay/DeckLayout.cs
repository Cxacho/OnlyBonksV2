using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckLayout : MonoBehaviour
{
    public Transform deckPanelTransform;

    public GameplayManager gameplayManager;

    public GameObject deckPanel;
    public GameObject drawDeckPanel;
    
    void Start()
    {
       
    }
    void DisplayDeck()
    {

        //spawn kart do decku
        for (int i = 0; i < gameplayManager.startingDeck.Count; i++)
        {
            if(deckPanel.activeSelf)
            Instantiate(gameplayManager.startingDeck[i], GameObject.FindGameObjectWithTag("Panel").transform);
            
            
        }
    }
    


}
