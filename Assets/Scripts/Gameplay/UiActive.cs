using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UiActive : MonoBehaviour
{
    
    public GameObject deckPanel;
    public GameObject drawDeckPanel;
    public GameObject discardDeckPanel;
    
    public GameplayManager gameplayManager;
    public GameObject playerHealthBar;
    [SerializeField] GameObject mapPanel;
    [SerializeField] GameObject settingsPanel;
    public GameObject eqPanel;
    public GameObject eqSlotsPanel;
    public GameObject eqBackpackPanel;
    
    
    List<TextMeshProUGUI> values = new List<TextMeshProUGUI>();
    [SerializeField] GameObject eqValuesPanel;
    [SerializeField] private List<Button> buttons = new List<Button>();
    //public Vector3 mousePosition,secPos;
    //RaycastHit hit;
    [HideInInspector] public GameObject cardToInspect;
    [HideInInspector] public int panelIndex = 0;
    // Update is called once per frame

    public void ClosePanel()
    {
        panelIndex = 0;

    }
    public void OnDeckPanelClick()
    {

        deckPanel.SetActive(true);
        if (deckPanel.activeSelf == true)
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
    public void OnDrawDeckClick()
    {

        drawDeckPanel.SetActive(true);
        if (drawDeckPanel.activeSelf == true)
        {
            OnDrawDeckDestroy();
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
    public void OnDiscardDeckClick()
    {
        discardDeckPanel.SetActive(true);
        if (discardDeckPanel.activeSelf == true)
        {
            OnDiscardDeckDestroy();
        }
        if (panelIndex == 3)
        {
            panelIndex = 0;
            //OnDiscardDeckDestroy();
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
                deckPanel.SetActive(false);
                drawDeckPanel.SetActive(false);
                discardDeckPanel.SetActive(false);
                settingsPanel.SetActive(false);
                mapPanel.SetActive(false);
                if(GameObject.Find("Player") != null)
                playerHealthBar.SetActive(true);
                eqPanel.SetActive(false);
                break;
            case 1:
                //wlaczenie panelu decklayoutu
                deckPanel.SetActive(true);
                drawDeckPanel.SetActive(false);
                discardDeckPanel.SetActive(false);
                settingsPanel.SetActive(false);
                mapPanel.SetActive(false);
                eqPanel.SetActive(false);
                break;
            case 2:
                //wlaczenie panelu drawpile'u
                deckPanel.SetActive(false);
                drawDeckPanel.SetActive(true);
                discardDeckPanel.SetActive(false);
                settingsPanel.SetActive(false);
                mapPanel.SetActive(false);
                eqPanel.SetActive(false);

                break;
            case 3:
                //wlaczenie panelu discardu
                deckPanel.SetActive(false);
                drawDeckPanel.SetActive(false);
                discardDeckPanel.SetActive(true);
                settingsPanel.SetActive(false);
                mapPanel.SetActive(false);
                eqPanel.SetActive(false);

                break;
            case 4:
                //wlaczenie panelu ustawien
                deckPanel.SetActive(false);
                drawDeckPanel.SetActive(false);
                discardDeckPanel.SetActive(false);
                settingsPanel.SetActive(true);
                mapPanel.SetActive(false);
                eqPanel.SetActive(false);

                break;
            case 5:
                //wlaczenie panelu mapy
                deckPanel.SetActive(false);
                drawDeckPanel.SetActive(false);
                discardDeckPanel.SetActive(false);
                settingsPanel.SetActive(false);
                mapPanel.SetActive(true);
                eqPanel.SetActive(false);

                break;
                //wlaczenie panelu eq
            case 6:
                deckPanel.SetActive(false);
                drawDeckPanel.SetActive(false);
                discardDeckPanel.SetActive(false);
                settingsPanel.SetActive(false);
                mapPanel.SetActive(false);
                eqPanel.SetActive(true);
                break;

        }

    }
    public void OnDeckLayoutClick()
    {
        if (deckPanel.activeSelf)
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
                if (clone.transform.IsChildOf(deckPanelTransform.transform))
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
                if (clone.transform.IsChildOf(deckPanel.transform))
                {
                    Destroy(clone);
                }
            }
        }
    }
    void DisplayDeck()
    {

        //spawn kart do decku
        for (int i = 0; i < gameplayManager.startingDeck.Count; i++)
        {
            if (deckPanel.activeSelf)
                Instantiate(gameplayManager.startingDeck[i], GameObject.FindGameObjectWithTag("Panel").transform);
        }
    }

    public void OnDrawDeckCheck()
    {
        if (drawDeckPanel.activeSelf)
        {
            DisplayDrawDeck();
        }
    }
    void OnDrawDeckDestroy()
    {
        var clones = GameObject.FindGameObjectsWithTag("Card");
        if (panelIndex != 2)
        {
            foreach (var clone in clones)
            {
                if (clone.transform.IsChildOf(drawDeckPanel.transform))
                {
                    Destroy(clone);
                }
            }
        }
    }
    public void DisplayDrawDeck()   //Pokazanie DrawDeck
    {

        for (int i = 0; i < gameplayManager.drawDeck.Count; i++)
        {
            if (drawDeckPanel.activeSelf)
                Instantiate(gameplayManager.drawDeck[i], GameObject.FindGameObjectWithTag("DrawPanel").transform);
        }

    }
    public void OnDiscardDeckCheck()
    {
        if (discardDeckPanel.activeSelf)
            DisplayDiscardDeck();
    }
    void OnDiscardDeckDestroy()
    {
        var clones = GameObject.FindGameObjectsWithTag("Card");

        foreach (var clone in clones)
        {

            if (clone.transform.IsChildOf(discardDeckPanel.transform))
            {
                Destroy(clone);
            }
        }

    }
    void DisplayDiscardDeck()
    {
        for (int i = 0; i < gameplayManager.discardDeck.Count; i++)
        {
            if (discardDeckPanel.activeSelf)
                Instantiate(gameplayManager.discardDeck[i], GameObject.FindGameObjectWithTag("DiscardDeck").transform);
        }
    }
    public void OnSettingsClick()
    {
        Debug.Log("???");
        settingsPanel.SetActive(true);
        if (panelIndex == 4)
        {
            panelIndex = 0;
            //OnDiscardDeckDestroy();
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
        mapPanel.SetActive(true);
        playerHealthBar.SetActive(false);
        
        if (panelIndex == 5)
        {
            gameplayManager.battleUI.SetActive(true);
            for (int i = 0; i < gameplayManager.enemyBattleStation.Length; i++)
            {
                gameplayManager.enemyBattleStation[i].gameObject.SetActive(true);
            }
            panelIndex = 0;
            //OnDiscardDeckDestroy();
        }
        else
        {
            gameplayManager.battleUI.SetActive(false);
            for (int i = 0; i < gameplayManager.enemyBattleStation.Length; i++)
            {
                gameplayManager.enemyBattleStation[i].gameObject.SetActive(false);
            }
            panelIndex = 5;
        }
        //Debug.Log(panelIndex);
    }
    public void OnEqipmentClick()
    {
        eqPanel.SetActive(true);
        UpdateStatValues();
        if (panelIndex == 6)
        {
            panelIndex = 0;
            //OnDiscardDeckDestroy();
        }
        else
        {
            panelIndex = 6;
        }
        //Debug.Log(panelIndex);
    }
    void UpdateStatValues()
    {
        values.Clear();
        for (int i = 0; i < eqValuesPanel.transform.childCount; i++)
            values.Add(eqValuesPanel.transform.GetChild(i).GetComponent<TextMeshProUGUI>());
        values[0].text = gameplayManager.player.currentHealth.ToString() + " / " + gameplayManager.player.maxHealth.ToString();
        //dodac armor rating
        //moznabylo dac for loopke tu
        values[1].text = 0.ToString();
        values[2].text = gameplayManager.player.strenght.ToString();
        values[3].text = gameplayManager.player.dexterity.ToString();
        values[4].text = gameplayManager.player.inteligence.ToString();
        values[5].text = gameplayManager.gold.ToString();
            values[6].text = gameplayManager.startingDeck.Count.ToString();
            values[7].text = gameplayManager.currentFloor.ToString();
    }
    public void DisableButtons(int select)
    {
        switch (select)
        {
            case 0:
                if (gameplayManager.discardDeck.Count == 0)
                {
                    Debug.Log("no card to choose from");
                    return;
                }
                break;
            case 1:
                if (gameplayManager.drawDeck.Count == 0)
                {
                    Debug.Log("no card to choose from");
                    return;
                }
                break;
            case 2:
                if (gameplayManager.exhaustedDeck.Count == 0)
                {
                    Debug.Log("no card to choose from");
                    return;
                }
                break;
        }

        gameplayManager.state = BattleState.CREATING;
        SetButtonsNonInterractible();
        if (select == 0)
        {
            OnDiscardDeckClick();
            OnDiscardDeckCheck();
        }
        else if(select == 1)
        {
            OnDrawDeckClick();
            OnDrawDeckCheck();
        }
        else
        {

        }

    }
    public void SetButtonsInterractible()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void SetButtonsNonInterractible()
    {

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = false;
        }
    }
    public void EnableButtons(int select)
    {
        SetButtonsInterractible();
        if (select == 0)
        {
            OnDiscardDeckClick();
            OnDiscardDeckCheck();
        }
        else if (select == 1)
        {
            OnDrawDeckClick();
            OnDrawDeckCheck();
        }
        else
        {

        }
    }
}

