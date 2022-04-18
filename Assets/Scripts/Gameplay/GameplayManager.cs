using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, VictoryScreen}


public class GameplayManager : MonoBehaviour
{
    public BattleState state;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;
    
    public GameObject drawPile,playersHand;
    public GameObject discardPile;
    public GameObject playerHandObject;
    public GameObject panelWin;
    public GameObject cardHolder;
    public GameObject battleUI;
    public GameObject enemyGameObject;
    public CardAlign cAlign;


    public Player player;
    public Enemy enemy;

    public bool canPlayCards = true;
    public bool firstRound = true;

    public int playerDrawAmount,drawAmount;
    public int maxCardDraw = 12;
    private int random;
    public float tilt = 20;

    //lista kart ktore posiada gracz na poczatku
    public List<GameObject> startingDeck = new List<GameObject>();


    //lista kart ktore mozemy dobrac do reki
    public List<GameObject> drawDeck;

   
    //Player Hand
    public List<GameObject> playerHand = new List<GameObject>();


    //lista kart ktore zagralismy
    public List<GameObject> discardDeck = new List<GameObject>();

    //lista kart dostêpnych
    public List<GameObject> cards = new List<GameObject>();

    //lista przeciwnikow na scenie
    public GameObject[] enemies;
    
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        cAlign = playerHandObject.GetComponent<CardAlign>();
    }
    IEnumerator SetupBattle()
    {

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        StartCoroutine(OnPlayersTurn());
    }
    IEnumerator OnPlayersTurn()
    {
        DrawCards();
        player.mana = 3;
        player.manaText.text = player.mana.ToString();
        player.ResetImg();
        canPlayCards = true;
        yield return new WaitForSeconds(2);

    }
    IEnumerator OnEnemiesTurn()
    {
        foreach (var enemy in enemies)
        {
            ITakeTurn takeTurn = enemy.GetComponent<ITakeTurn>();
            takeTurn.takeTurn(player);
        }

        yield return new WaitForSeconds(2f);

        player.armor = 0;
        state = BattleState.PLAYERTURN;
        StartCoroutine(OnPlayersTurn());
    }
    public IEnumerator OnBattleWin()
    {
        Debug.Log("Batlle Won");
        state = BattleState.VictoryScreen;
        
        
        StartCoroutine(VictoryScreen());
        yield return new WaitForSeconds(2f);
        //check czy jakiego statusu byl przeciwnik normal/elite/boss
        //roll kart ktore mozesz dodac do decku 
        //+gold
        //Zmien battlestate do mapy, przenies gracza na mape

    }
    IEnumerator VictoryScreen()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Victory Screen");
        panelWin.SetActive(true);
        battleUI.SetActive(false);
        enemyGameObject.SetActive(false);
        for (int i = 0; i < 2; i++)
        {
            random = Random.Range(0, cards.Count);
            Instantiate(cards[random], GameObject.FindGameObjectWithTag("cardHolder").transform);
        }
        
    }
    IEnumerator OnBattleLost()
    {
        yield return new WaitForSeconds(2);
        //pokaz panel statystyk
        //przenies do menu glownego
    }
    
    

    public void OnClick()
    {
        if (playerHand.Count != 0)
        {
            playerHand.ForEach(item => discardDeck.Add(item));
            playerHand.Clear();
            
            for(var i = playerHandObject.transform.childCount-1;i>=0;i--)
            {
                Destroy(playerHandObject.transform.GetChild(i).gameObject);
            }

        }
            
          

        state = BattleState.ENEMYTURN;
        StartCoroutine(OnEnemiesTurn());
        
    }
    public void DrawCards()
    {
        /*
        for (int i = 0; i < playerDrawAmount; i++)
        {
            if (drawDeck.Count == 0)
            {
                
                shuffleDeck();
            }
            random = Random.Range(0, drawDeck.Count);
            Instantiate(drawDeck[random], GameObject.FindGameObjectWithTag("PlayerHand").transform);
            
            playerHand.Add(drawDeck[random]);
            drawDeck.RemoveAt(random);
            
        }
        */
        drawAmount++;
        if (playerDrawAmount>=drawAmount)
        {
            if (drawDeck.Count == 0)
            {
                shuffleDeck();
            }
            var random = Random.Range(0, drawDeck.Count);
            var card = Instantiate(drawDeck[random], drawPile.transform.position, transform.rotation);
            card.transform.SetParent(playerHandObject.transform);
            card.transform.localScale = Vector3.one;
            playerHand.Add(drawDeck[random]);
            drawDeck.RemoveAt(random);
            cAlign.Invoke("Animate",1f);
        }

    }
    
    void shuffleDeck()
    {
        discardDeck.ForEach(item => drawDeck.Add(item));
        discardDeck.Clear();
    }
    public void checkPlayerMana(int cost)
    {
        if (cost > player.mana || player.mana == 0)
        {
            canPlayCards = false;
        }
        else
        {
            canPlayCards = true;
            player.mana -= cost;
        }
    }

    
}
