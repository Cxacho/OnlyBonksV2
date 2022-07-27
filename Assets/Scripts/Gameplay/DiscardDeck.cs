using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardDeck : MonoBehaviour
{

    public Transform discardDeckPanelTransform;
    public GameObject discardDeckPanel;

    public GameplayManager gameplayManager;


    public void DisplayDiscardPile()   //Pokazanie DrawPile
    {
        
            for (int i = 0; i < gameplayManager.discardDeck.Count; i++)
            {
                if (discardDeckPanel.activeSelf)
                    Instantiate(gameplayManager.discardDeck[i], GameObject.FindGameObjectWithTag("DiscardDeck").transform);
            }
        
    }
}
