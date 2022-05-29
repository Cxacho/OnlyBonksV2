using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public enum BattleState { NODE ,START, PLAYERTURN, ENEMYTURN, WON, LOST, VictoryScreen}

[RequireComponent(typeof(AudioListener))]
[RequireComponent(typeof(AudioSource))]
public class GameplayManager : MonoBehaviour
{
    public BattleState state;

    
    [SerializeField] private Transform[] enemyBattleStation = new Transform[3];
    
    public GameObject drawPile,playersHand;
    public GameObject discardPile;
    public GameObject playerHandObject;
    public GameObject panelWin;
    public UiActive ui;
    public GameObject panelLose;
    public GameObject cardHolder;
    public GameObject battleUI;
    public GameObject goldtxt;
    public GameObject drawButton;
    public LevelProgress levelProgress;
    public GameObject shopPanel;

    [SerializeField] private Player player;

    private CardAlign cardAlign;

    public bool canPlayCards = true;
    public bool firstRound = true;
    public int basePlayerDrawAmount;
    public int playerDrawAmount;
    public int maxCardDraw = 12;
    public int drawAmount;

    private int random;

    public int gold = 100;
    public int currentXP=0;

    public GameObject textPanel;
    public TextMeshProUGUI darkSoulsText;

    //lista wszystkich kart w grze, wa¿ne ¿eby dodawaæ je po kolei 
    public List<GameObject> allCards = new List<GameObject>();

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
    public List<GameObject> enemies = new List<GameObject>();

    //lista wszystkich przeciwnikow (nie zaimplementowane)
    public List<GameObject> floorOneEnemies = new List<GameObject>();

    public List<GameObject> floorTwoEnemies = new List<GameObject>();

    public List<GameObject> floorThreeEnemies = new List<GameObject>();


    public List<GameObject> EliteEnemies = new List<GameObject>();


    public List<GameObject> Mistery = new List<GameObject>();


    public List<GameObject> Boss = new List<GameObject>();

    public GameObject Shopkeep;

    public List<Enemy> enType = new List<Enemy>();


    public Map.MapPlayerTracker map;



    private void Awake()
    {
        cardAlign = GameObject.Find("PlayerHand").GetComponent<CardAlign>();
        gogo();


    }

    private void Update()
    {
        goldtxt.GetComponent<TextMeshProUGUI>().text = gold.ToString();
        
    }

    void Start()
    {
        
        
    
        state = BattleState.START;

        



    }
    IEnumerator ChooseNode()
    {

        state = BattleState.NODE;
        map.Locked = false;
        ui.OnMapClick();
        yield return new WaitForSeconds(.1f);
        if (playerHand.Count != 0)
        {
            resetDeck();
        }
    }
    public IEnumerator SetupBattle()
    {

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < 1; i++)
        {
            SpawnEnemies(i);
        }
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        foreach (GameObject en in enemies)
            enType.Add(en.GetComponent<Enemy>());
        


        state = BattleState.PLAYERTURN;
        StartCoroutine(OnPlayersTurn());
    }

    public IEnumerator SetupEliteBattle()
    {
        yield break;
    }
    public IEnumerator SetupRestSite()
    {
        yield break;
    }
    public IEnumerator SetupBoss()
    {
        yield break;
    }
    public IEnumerator SetupStore()
    {
        
        ui.panelIndex = 0;
        ui.Check();
        shopPanel.SetActive(true);
        var fix = GameObject.Find("5CardsArea").GetComponent<GridLayoutGroup>();
        fix.enabled = true;
        yield break;
    }
    public IEnumerator SetupTreasure()
    {
        yield break;
    }
    public IEnumerator SetupMistery()
    {
        yield break;
    }
    IEnumerator OnPlayersTurn()
    {
        drawAmount = 0;

        DrawCards(basePlayerDrawAmount);

        player.AssignMana();

        player.ResetImg();

        canPlayCards = true;


         List<GameObject> _indicators = new List<GameObject>();
        _indicators.AddRange(GameObject.FindGameObjectsWithTag("EnemyIndicator"));
        
        foreach(var indicator in _indicators)
        {
            indicator.GetComponent<Image>().enabled = true;           
        }

        yield return new WaitForSeconds(2);

    }
    IEnumerator OnEnemiesTurn()
    {
        yield return new WaitForSeconds(3f);
        foreach (var enemy in enemies)
        {
            ITakeTurn takeTurn = enemy.GetComponent<ITakeTurn>();
            takeTurn.takeTurn(player);
            yield return new WaitForSeconds(1f);
        }

        

        

        player.ResetPlayerArmor();

        player.OnEndTurn();

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
    public IEnumerator OnBattleLost()
    {
        yield return new WaitForSeconds(2);


        StartCoroutine(LoseScreen());
        
    }
    IEnumerator VictoryScreen()
    {
        yield return new WaitForSeconds(2f);

        Debug.Log("Victory Screen");

        PanelsOnWin();
        map.Locked = false;
        
        for (int i = 0; i < 2; i++)
        {
            random = Random.Range(0, cards.Count);
            Instantiate(cards[random], GameObject.FindGameObjectWithTag("cardHolder").transform);
        }
        
    }
    IEnumerator LoseScreen()
    {
        yield return new WaitForSeconds(0f);

        levelProgress.UpdateLevelProgress();

        panelLose.SetActive(true);
        battleUI.SetActive(false);
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

        ExecuteDarkSoulsText("Enemy Turn");

        
        state = BattleState.ENEMYTURN;
        StartCoroutine(OnEnemiesTurn());
        
    }
    public void DrawCards(int amount)
    {
        playerDrawAmount = amount;
        drawAmount++;
        if (playerDrawAmount >= drawAmount)
        {
            if (drawDeck.Count == 0)
            {
                shuffleDeck();
            }
            var random = Random.Range(0, drawDeck.Count);
            GameObject card = Instantiate(drawDeck[random], drawButton.transform.position, transform.rotation);
            card.transform.SetParent(cardAlign.gameObject.transform);
            card.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            playerHand.Add(drawDeck[random]);
            drawDeck.RemoveAt(random);
            cardAlign.Animate();
            
        }
        if (playerDrawAmount < drawAmount)
            drawAmount = 0;
    }
    private void shuffleDeck()
    {
        discardDeck.ForEach(item => drawDeck.Add(item));
        discardDeck.Clear();
    }
    private void resetDeck()
    {
        playerHand.ForEach(item => drawDeck.Add(item));
        var hand = GameObject.Find("PlayerHand");
        for (int i = 0; i < playerHand.Count; i++)
        {
            Destroy(hand.transform.GetChild(i).gameObject);
        }
        
        playerHand.Clear();
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
    private void PanelsOnWin()
    {
        panelWin.SetActive(true);
        battleUI.SetActive(false);
    }
    public void PanelsOnButtonNext()
    {
        panelWin.SetActive(false);
        battleUI.SetActive(true);
    }
    private void ExecuteDarkSoulsText(string _text)
    {
        textPanel.SetActive(true);
        darkSoulsText.text = _text;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(darkSoulsText.DOFade(1, 1.5f));
        sequence.Insert(0,darkSoulsText.transform.DOScale(1.5f, 3f));
        sequence.OnComplete(()=>
        {
            darkSoulsText.DOFade(0, 0);
            darkSoulsText.transform.DOScale(1f, 0);
            textPanel.SetActive(false);        
        });
        



    }
    private void SpawnEnemies(int i)
    {
        Instantiate(floorOneEnemies[i], enemyBattleStation[i].transform.position, Quaternion.identity, enemyBattleStation[i].transform);
    }
    public void gogo()
    {
        
        StartCoroutine(ChooseNode());
        
    }
}

