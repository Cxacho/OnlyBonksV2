using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDeck : MonoBehaviour
{
    public Transform drawDeckPanelTransform;
    public GameplayManager gameplayManager;
    public GameObject drawDeckPanel;
    

    private void Start()
    {
        DrawPile();
    }

        

        void DrawPile()
        {
        //wykona sie tylko dla pierwszej rundy
        if(gameplayManager.firstRound == true)
        {


            
            gameplayManager.drawDeck = new List<GameObject>(gameplayManager.startingDeck);
            
            
            
            gameplayManager.firstRound = false;
        }
    }


}
