using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiActive : MonoBehaviour
{
    public GameObject deckScreen;
    public GameObject drawPile;
    public GameObject discardPile;
    public int panelIndex = 0;
    public GameplayManager gm;
    public GameObject fill;
    [SerializeField] GameObject mapScreen;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject eqPanel;
    //public Vector3 mousePosition,secPos;
    //RaycastHit hit;


    // Update is called once per frame

    public void ClosePanel()
    {
        panelIndex = 0;

    }
    public void OnDeckScreen()
    {

        deckScreen.SetActive(true);
        if (deckScreen.activeSelf == true)
        {
            OnDeckLayoutDestroy();
        }
        if (panelIndex == 1)
        {
            panelIndex = 0;

        }
        else
        {
            panelIndex = 1;
        }
        //Debug.Log(panelIndex);



    }
    public void OnDrawPile()
    {

        drawPile.SetActive(true);
        if (drawPile.activeSelf == true)
        {
            OnDrawPileDestroy();
        }
        if (panelIndex == 2)
        {
            panelIndex = 0;
        }
        else
        {
            panelIndex = 2;
        }
        //Debug.Log(panelIndex);
    }
    public void OnDiscardPile()
    {
        discardPile.SetActive(true);
        if (discardPile.activeSelf == true)
        {
            OnDiscardPileDestroy();
        }
        if (panelIndex == 3)
        {
            panelIndex = 0;
            //OnDiscardPileDestroy();
        }
        else
        {
            panelIndex = 3;
        }
        //Debug.Log(panelIndex);
    }

    void Update()
    {
        Check();
        //secPos = Input.mousePosition;
        //secPos.z = Camera.main.nearClipPlane;
        //mousePosition = Camera.main.ScreenToWorldPoint(secPos);
        
    }
    public void Check()
    {
        switch (panelIndex)
        {
            case 0:
                //wylaczenie wszystkich panelow
                deckScreen.SetActive(false);
                drawPile.SetActive(false);
                discardPile.SetActive(false);
                settings.SetActive(false);
                mapScreen.SetActive(false);
                if(GameObject.Find("Player") != null)
                fill.SetActive(true);
                eqPanel.SetActive(false);
                break;
            case 1:
                //wlaczenie panelu decklayoutu
                deckScreen.SetActive(true);
                drawPile.SetActive(false);
                discardPile.SetActive(false);
                settings.SetActive(false);
                mapScreen.SetActive(false);
                eqPanel.SetActive(false);
                break;
            case 2:
                //wlaczenie panelu drawpile'u
                deckScreen.SetActive(false);
                drawPile.SetActive(true);
                discardPile.SetActive(false);
                settings.SetActive(false);
                mapScreen.SetActive(false);
                eqPanel.SetActive(false);

                break;
            case 3:
                //wlaczenie panelu discardu
                deckScreen.SetActive(false);
                drawPile.SetActive(false);
                discardPile.SetActive(true);
                settings.SetActive(false);
                mapScreen.SetActive(false);
                eqPanel.SetActive(false);

                break;
            case 4:
                //wlaczenie panelu ustawien
                deckScreen.SetActive(false);
                drawPile.SetActive(false);
                discardPile.SetActive(false);
                settings.SetActive(true);
                mapScreen.SetActive(false);
                eqPanel.SetActive(false);

                break;
            case 5:
                //wlaczenie panelu mapy
                deckScreen.SetActive(false);
                drawPile.SetActive(false);
                discardPile.SetActive(false);
                settings.SetActive(false);
                mapScreen.SetActive(true);
                eqPanel.SetActive(false);

                break;
                //wlaczenie panelu eq
            case 6:
                deckScreen.SetActive(false);
                drawPile.SetActive(false);
                discardPile.SetActive(false);
                settings.SetActive(false);
                mapScreen.SetActive(false);
                eqPanel.SetActive(true);
                break;

        }

    }
    public void OnDeckLayoutClick()
    {
        if (deckScreen.activeSelf)
        {
            DisplayDeck();
            //Debug.Log("faken");
        }
        /*
        else
        {
            var clones = GameObject.FindGameObjectsWithTag("Card");
            foreach (var clone in clones)
            {
                if (clone.transform.IsChildOf(deckScreen.transform))
                {
                    Destroy(clone);
                }
            }
            //deckPanel.SetActive(false);

        }
        */
    }
    void OnDeckLayoutDestroy()
    {
        if (panelIndex != 1)
        {
            var clones = GameObject.FindGameObjectsWithTag("Card");
            foreach (var clone in clones)
            {
                if (clone.transform.IsChildOf(deckScreen.transform))
                {
                    Destroy(clone);
                }
            }
        }
    }
    void DisplayDeck()
    {

        //spawn kart do decku
        for (int i = 0; i < gm.startingDeck.Count; i++)
        {
            if (deckScreen.activeSelf)
                Instantiate(gm.startingDeck[i], GameObject.FindGameObjectWithTag("Panel").transform);
        }
    }

    public void OnDrawPileClick()
    {
        if (drawPile.activeSelf)
        {
            DisplayDrawPile();
        }
    }
    void OnDrawPileDestroy()
    {
        var clones = GameObject.FindGameObjectsWithTag("Card");
        if (panelIndex != 2)
        {
            foreach (var clone in clones)
            {
                if (clone.transform.IsChildOf(drawPile.transform))
                {
                    Destroy(clone);
                }
            }
        }
    }
    public void DisplayDrawPile()   //Pokazanie DrawPile
    {

        for (int i = 0; i < gm.drawDeck.Count; i++)
        {
            if (drawPile.activeSelf)
                Instantiate(gm.drawDeck[i], GameObject.FindGameObjectWithTag("DrawPanel").transform);
        }

    }
    public void OnDiscardPileClick()
    {
        if (discardPile.activeSelf)
            DisplayDiscardPile();
    }
    void OnDiscardPileDestroy()
    {
        var clones = GameObject.FindGameObjectsWithTag("Card");

        foreach (var clone in clones)
        {

            if (clone.transform.IsChildOf(discardPile.transform))
            {
                Destroy(clone);
            }
        }

    }
    void DisplayDiscardPile()
    {
        for (int i = 0; i < gm.discardDeck.Count; i++)
        {
            if (discardPile.activeSelf)
                Instantiate(gm.discardDeck[i], GameObject.FindGameObjectWithTag("DiscardDeck").transform);
        }
    }
    public void OnSettingsClick()
    {
        Debug.Log("???");
        settings.SetActive(true);
        if (panelIndex == 4)
        {
            panelIndex = 0;
            //OnDiscardPileDestroy();
        }
        else
        {
            panelIndex = 4;
        }
        //Debug.Log(panelIndex);
    }
    public void OnMapClick()
    {
        Debug.Log("////");
        mapScreen.SetActive(true);
        fill.SetActive(false);
        
        if (panelIndex == 5)
        {
            gm.battleUI.SetActive(true);
            for (int i = 0; i < gm.enemyBattleStation.Length; i++)
            {
                gm.enemyBattleStation[i].gameObject.SetActive(true);
            }
            panelIndex = 0;
            //OnDiscardPileDestroy();
        }
        else
        {
            gm.battleUI.SetActive(false);
            for (int i = 0; i < gm.enemyBattleStation.Length; i++)
            {
                gm.enemyBattleStation[i].gameObject.SetActive(false);
            }
            panelIndex = 5;
        }
        //Debug.Log(panelIndex);
    }
    public void OnEqipmentClick()
    {
        eqPanel.SetActive(true);
        if (panelIndex == 6)
        {
            panelIndex = 0;
            //OnDiscardPileDestroy();
        }
        else
        {
            panelIndex = 6;
        }
        //Debug.Log(panelIndex);
    }
}

