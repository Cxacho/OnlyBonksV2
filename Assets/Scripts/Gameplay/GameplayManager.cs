using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public enum BattleState { NODE ,START, PLAYERTURN, ENEMYTURN, WON, LOST, VictoryScreen,DRAWING,INANIMATION}

[RequireComponent(typeof(AudioListener))]
[RequireComponent(typeof(AudioSource))]
public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    public BattleState state;

    #region GameObjectsHidden
    [HideInInspector] public GameObject drawPile,playersHand;
    [HideInInspector] public GameObject discardPile;
    [HideInInspector] public GameObject discardDeckButton;
    [HideInInspector] public GameObject playerHandObject;
    [HideInInspector] public GameObject treasurePanel, restSitePanel, mysteryPanel;
    [HideInInspector] public GameObject treasurePanelButton;
    [HideInInspector] public GameObject panelLose;
    [HideInInspector] public GameObject goldtxt;
    [HideInInspector] public GameObject shopPanel;
    #endregion

    #region GameObjects
    public GameObject goldRewardGameObject;
    public GameObject textPanel;
    public GameObject panelWin;
    public GameObject canvas;
    public GameObject cardHolder;
    public GameObject battleUI;
    public GameObject drawButton;
    #endregion

    #region Ints
    public int numOfList;
    public int currentFloor;
    public int basePlayerDrawAmount;
    private int random;
    public int gold = 100;
    public int currentXP = 0;
    public int goldReward;
    public int cardsPlayed;
    public int attackCardsPlayed;
    public int skillCardsPlayed;
    public int powerCardsPlayed;
    public int maxCardDraw = 12;
    [HideInInspector] public int playerDrawAmount;
    [HideInInspector] public int drawAmount;
    #endregion

    #region Bools
    public bool canPlayCards = true;
    public bool firstRound = true;
    private bool isAnyoneTargeted;
    [HideInInspector] public bool canEndTurn;
    #endregion

    #region Buttons
    private Button coinButton;
    private Button healButton;
    public Button endTurn;
    public Button mapButton;
    #endregion

    #region TMP

    public TextMeshProUGUI darkSoulsText;
    #endregion

    #region Enemy
    private EnemiesSpawner enemiesSpawner;
    public Transform[] enemyBattleStation = new Transform[3];
    public Sprite[] indicatorImages;
    #endregion

    #region Player
    public LevelProgress levelProgress;
    [SerializeField] private Player player;
    #endregion

    #region Card
    private CardAlign cardAlign;
    [HideInInspector] public cardPlayed lastCardPlayed;
    #endregion

    #region Ui

    [HideInInspector] public UiActive ui;

    #endregion

    #region Shop
    private ShopManager shopmanager;
    #endregion

    #region Map
    public Map.MapPlayerTracker map;
    public Map.ScrollNonUI scroll;
    #endregion

    #region Lists
    public List<GameObject> allRelicsList = new List<GameObject>();
    
    public List<GameObject> allCards = new List<GameObject>();

    //lista wszystkich kart w grze, wa�ne �eby dodawa� je po kolei 
    public List<GameObject> allCardsSHOP = new List<GameObject>();

    //lista kart ktore posiada gracz na poczatku
    public List<GameObject> startingDeck = new List<GameObject>();
    //lista kart ktore usuwamy z gryw trakcie pojedynku
    public List<GameObject> exhaustedDeck = new List<GameObject>();
    //lista kart ktore mozemy dobrac do reki
    public List<GameObject> drawDeck;
    public List<GameObject> enemiesIndicators = new List<GameObject>();

    public List<GameObject> relicsList = new List<GameObject>();
    //Player Hand
    public List<GameObject> playerHand = new List<GameObject>();

    public List<GameObject> retain = new List<GameObject>();

    List<GameObject> temp = new List<GameObject>();

    //lista kart ktore zagralismy
    public List<GameObject> discardDeck = new List<GameObject>();

    //lista kart dost�pnych
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
    //[HideInInspector] public float delay;
    public GameObject Shopkeep;

    public List<Enemy> enemyType = new List<Enemy>();
    #endregion

    private void Awake()
    {
        instance = this;



        cardAlign = GameObject.Find("PlayerHand").GetComponent<CardAlign>();
        enemiesSpawner = FindObjectOfType<EnemiesSpawner>();
        shopmanager = GetComponent<ShopManager>();
        

        choosingNode();


        canEndTurn = true;

            
    }
    private void Update()
    {
        foreach (Enemy enemy in enemyType)
        {
            if (enemy.targeted == true || enemy.isFirstTarget == true || enemy.isSecondTarget == true || enemy.isThirdTarget == true) isAnyoneTargeted = true;
            else isAnyoneTargeted = false;
        }
        if (state == BattleState.PLAYERTURN && isAnyoneTargeted == false && canEndTurn == true)
        {
            endTurn.interactable = true;
        }
        else endTurn.interactable = false;

        
        goldtxt.GetComponent<TextMeshProUGUI>().text = gold.ToString();
    }
    IEnumerator ChooseNode()
    {
        if (playerHand.Count != 0)
        {
            resetDeck();
        }
        battleUI.SetActive(false);
        state = BattleState.NODE;
        map.Locked = false;
        ui.OnMapClick();
        yield return new WaitForSeconds(.1f);
        
        
        scroll = GameObject.Find("MapParentWithAScroll").GetComponent<Map.ScrollNonUI>();
        scroll.freezeX = true;
    }
    public IEnumerator SetupBattle()
    {
        battleUI.SetActive(true);
        if (playerHand.Count != 0)
        {
            resetDeck();
        }
        goldReward = 0;
        //tu w construktorze trzeba wrzucic liste przeciwnikow na ktorom mozemy trafic, zeby mozna bylo ich roznie spawnic
        exhaustedDeck.Clear();
        player.OnBattleSetup();

        yield return new WaitForSeconds(0.1f);
        
        if(currentFloor<4)
        {
            numOfList = Random.Range(0, 3);
        }
        else if(currentFloor<7)
                {
            numOfList = Random.Range(3, 6);
        }
        else
                    {
            numOfList = Random.Range(6, 10);
        }

        SpawnEnemies(enemiesSpawner.floorOneEnemies[numOfList]);
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        foreach (GameObject enemy in enemies)
            enemyType.Add(enemy.GetComponent<Enemy>());
        


        
        StartCoroutine(OnPlayersTurn());
    }
    public IEnumerator SetupEliteBattle()
    {
        battleUI.SetActive(true);
        exhaustedDeck.Clear();
        player.OnBattleSetup();
        yield return new WaitForSeconds(0.1f);
        SpawnEnemies(enemiesSpawner.floorOneElites[0]);
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        foreach (GameObject en in enemies)
            enemyType.Add(en.GetComponent<Enemy>());




        StartCoroutine(OnPlayersTurn());
    }
    public IEnumerator SetupRestSite()
    {
        player.OnBattleSetup();
        restSitePanel.SetActive(true);
        coinButton = restSitePanel.transform.GetChild(1).gameObject.GetComponent<Button>();
        healButton = restSitePanel.transform.GetChild(2).gameObject.GetComponent<Button>();

        yield break;
    }
    public IEnumerator SetupBoss()
    {
        battleUI.SetActive(true);
        exhaustedDeck.Clear();
        player.OnBattleSetup();
        yield return new WaitForSeconds(0.1f);
        SpawnEnemies(enemiesSpawner.floorOneBosses[0]);
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        foreach (GameObject en in enemies)
            enemyType.Add(en.GetComponent<Enemy>());




        StartCoroutine(OnPlayersTurn());
    }
    public IEnumerator SetupStore()
    {
        
        player.OnBattleSetup();
        
        ui.panelIndex = 0;
        ui.Check();
        shopPanel.SetActive(true);
        shopmanager.SpawnShuffledCards();
        var fix = GameObject.Find("5CardsArea").GetComponent<GridLayoutGroup>();
        fix.enabled = true;
        yield break;
    }
    public IEnumerator SetupTreasure()
    {
        player.OnBattleSetup();
        treasurePanel.SetActive(true);
        if (treasurePanel.transform.childCount > 0)
        {
            var checkChildCount = treasurePanel.transform.childCount;
            for (int i = 0; i < checkChildCount - 1; i++)
            {
                Destroy(treasurePanel.transform.GetChild(i).gameObject);
            }
            var random = Random.Range(1, allRelicsList.Count - 1);
            Instantiate(allRelicsList[random],treasurePanel.transform);
            allRelicsList.RemoveAt(random);
            treasurePanelButton.transform.SetAsLastSibling();
            //GetMoneyButton Instantiate(GetMoneyButtonczycos, treasurePanel.transform);
        }
        yield break;
    }
    public IEnumerator SetupMistery()
    {
        
        player.OnBattleSetup();
        mysteryPanel.SetActive(true);
        var random = Random.Range(0, Mistery.Count);
        Instantiate(Mistery[random], mysteryPanel.transform);
        Mistery.RemoveAt(random);

        yield break;
    }
    IEnumerator OnPlayersTurn()
    {
        lastCardPlayed = cardPlayed.brak;
        cardsPlayed = 0;
        attackCardsPlayed = 0;
        skillCardsPlayed = 0;
        powerCardsPlayed = 0;
        //state = BattleState.PLAYERTURN;
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

        yield return new WaitForSeconds(0.5f);

    }
    private async void OnEnemiesTurn()
    {
        foreach (GameObject enemy in enemies)
        {
            await EnemyTurn(enemy);
        }
        player.ResetPlayerArmor();
        player.OnEndTurn();
        StartCoroutine(OnPlayersTurn());
    }
    public async Task EnemyTurn(GameObject enemy)
    {
        
        
            ITakeTurn takeTurn = enemy.GetComponent<ITakeTurn>();
            takeTurn.takeTurn(player);
            if (player.currentHealth == 0)
            {
                state = BattleState.LOST;
                StartCoroutine(OnBattleLost());
            }
            await Task.Delay(600);
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
        //animacja �mierci
        foreach (GameObject enemy in enemies) Destroy(enemy);
        Destroy(GameObject.FindGameObjectWithTag("Player"));
           yield return new WaitForSeconds(0.5f);


        StartCoroutine(LoseScreen());
        
    }
    IEnumerator VictoryScreen()
    {
        yield return new WaitForSeconds(2f);

        Debug.Log("Victory Screen");
        PanelsOnWin();
        map.Locked = false;
        goldRewardGameObject.gameObject.SetActive(true);
        GameObject.Find("GoldReward").transform.GetComponentInChildren<TextMeshProUGUI>().text = goldReward.ToString();
        
        for (int i = 0; i < 2; i++)
        {
            random = Random.Range(0, allCardsSHOP.Count);
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
    public async void OnClick()
    {
        retain.Clear();
        if (playerHand.Count != 0)
        {
            temp.Clear();
            //mechanika retain'u kart
            foreach (Card card in FindObjectsOfType<Card>())
                if (card.retainable == true)
                {
                    var cardIndex = card.gameObject.transform.GetSiblingIndex();
                    Debug.Log(cardIndex);
                    retain.Add(playerHand[cardIndex]);
                    playerHand.RemoveAt(cardIndex);
                    //zbierz index player handu.
                    temp.Add(card.gameObject);
                    card.transform.SetParent(battleUI.transform);
                    
                }
            var phCount = playerHand.Count;
            playerHand.ForEach(item => discardDeck.Add(item));
            playerHand.Clear();
            for(var i = phCount-1;i>=0;i--)
            {
                Destroy(playerHandObject.transform.GetChild(i).gameObject);
            }
            playerHand.AddRange(retain);
            temp.ForEach(obj => obj.transform.SetParent(playerHandObject.transform));
        }

        //cardAlign.SetValues();


        await ExecuteDarkSoulsText("Enemy Turn");

        
        state = BattleState.ENEMYTURN;
        OnEnemiesTurn();
        
    }
    public void DrawCards(int amount)
    {
        state = BattleState.DRAWING;
        playerDrawAmount = amount;
        /*
        if (delay == 0)
        {
            //gowniany fix, ale jednak fix
            delay = ((playerDrawAmount + playerHand.Count) * 0.5f) + Time.time;
        }
        */
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
            card.GetComponent<Card>().index = card.transform.GetSiblingIndex();
            var updateValue = drawButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            updateValue.text = (drawDeck.Count -1).ToString();
            card.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            playerHand.Add(drawDeck[random]);
            drawDeck.RemoveAt(random);
            cardAlign.Animate();
            
        }
        if (playerDrawAmount < drawAmount)
            drawAmount = 0;
    }
    public void CreateCard(int cardIndex)
    {
        //var card = PrefabUtility.InstantiatePrefab(allCards[cardIndex]as GameObject) as GameObject;
        playerHand.Add(allCards[cardIndex]);
        var card = Instantiate(allCards[cardIndex], Vector3.zero, Quaternion.identity);
        card.transform.SetParent(cardAlign.gameObject.transform);
        card.transform.localScale = Vector3.one;
        
        cardAlign.SetValues();
        
        
    }
    private void shuffleDeck()
    {
        discardDeck.ForEach(item => drawDeck.Add(item));
        discardDeck.Clear();
        var updateText = discardDeckButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        updateText = discardDeck.Count.ToString();
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
            if(cost == 0) canPlayCards = true;

            else canPlayCards = false;
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
        
    }
    public void PanelsOnButtonNext()
    {
        panelWin.SetActive(false);
        battleUI.SetActive(true);
    }
    private async Task ExecuteDarkSoulsText(string _text)
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
        await Task.Delay(2000);
        


    }
    private void SpawnEnemies(List<GameObject> enemies)
    {
        for (int i = 0;i < enemies.Count;i++)
        Instantiate(enemies[i], enemyBattleStation[i].transform.position, Quaternion.identity, enemyBattleStation[i].transform);
    }
    public void choosingNode()
    {
        
        StartCoroutine(ChooseNode());
        
    }
    public void DestroyChilds() // MUHAHAHHA DIE STUPID CHILDS
    {
        var numberOfChilds = GameObject.FindGameObjectWithTag("cardHolder").transform.childCount;
        for (int i = 0; i < numberOfChilds; i++)
        {
            Destroy(GameObject.FindGameObjectWithTag("cardHolder").transform.GetChild(i).gameObject);
        }
        
    }
    public void DisableTreasureNodePanel()

    {
        treasurePanel.SetActive(false);
        mysteryPanel.SetActive(false);
    }
    public void DisableRestSitePanel()

    {
        restSitePanel.SetActive(false);
        coinButton.interactable = true;
        healButton.interactable = true;
    }
    public void GetGold()
    {
        gold = Mathf.RoundToInt(gold*1.3f);
        goldtxt.GetComponent<TextMeshProUGUI>().text = gold.ToString();
        coinButton.interactable = false;
        healButton.interactable = false;
    }
    public void GetHealthBack()
    {
        var value = Mathf.RoundToInt(player.maxHealth * 0.3f);
        player.Heal(value);
        coinButton.interactable = false;
        healButton.interactable = false;
    }
    public enum cardPlayed
    {
        brak,
        attack,
        skill,
        power
    }
}

