using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardDeck : MonoBehaviour
{

    public Transform discardDeckScreen;
    public GameObject discardPanel;

    public GameplayManager gm;


    public void DisplayDiscardPile()   //Pokazanie DrawPile
    {
        
            for (int i = 0; i < gm.discardDeck.Count; i++)
            {
                if (discardPanel.activeSelf)
                    Instantiate(gm.discardDeck[i], GameObject.FindGameObjectWithTag("DiscardDeck").transform);
            }
        
    }
}
