using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardDeck : MonoBehaviour
{

    public Transform discardDeckTransform;
    public GameObject discardPanel;

    public GameplayManager gameplayManager;


    public void DisplayDiscardPile()   //Pokazanie DrawPile
    {
        
            for (int i = 0; i < gameplayManager.discardDeck.Count; i++)
            {
                if (discardPanel.activeSelf)
                    Instantiate(gameplayManager.discardDeck[i], GameObject.FindGameObjectWithTag("DiscardDeck").transform);
            }
        
    }
}
