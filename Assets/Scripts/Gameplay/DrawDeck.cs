using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDeck : MonoBehaviour
{
    public Transform drawDeckScreen;
    public GameplayManager gm;
    public GameObject drawPanel;
    

    private void Start()
    {
        DrawPile();
    }

        

        void DrawPile()
        {
        //wykona sie tylko dla pierwszej rundy
        if(gm.firstRound == true)
        {


            
            gm.drawDeck = new List<GameObject>(gm.startingDeck);
            
            
            
            gm.firstRound = false;
        }
    }


}
