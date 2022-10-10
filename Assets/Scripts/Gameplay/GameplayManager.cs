using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using System.Linq;

public enum BattleState { NODE ,START, PLAYERTURN, ENEMYTURN, WON, LOST, VictoryScreen,DRAWING,INANIMATION,CREATING,INANIM}

[RequireComponent(typeof(AudioListener))]
[RequireComponent(typeof(AudioSource))]
public class GameplayManager : MonoBehaviour
{
    private GameplayManager() { }
    private static GameplayManager instance;
    //Card 
    public BattleState state;
    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;
    public Weapon currentWeapon;

    #region Events
    public event EventHandler OnDraw;
    public event EventHandler OnTurnStart;
    public event EventHandler OnTurnEnd;
    public EventHandler OnCardPlayed;
    public EventHandler OnCardExhausted;
    public EventHandler OnEnemyKilled;
    public EventHandler OnDamageTaken;
    public Action<Card, float, int> OnCardPlayedDetail;
    #endregion

    #region GameObjects
    [Header("GameObjects")]
    public GameObject drawPile; 
    public GameObject playersHand;
    public GameObject discardPile;
    public GameObject discardDeckButton;
    public GameObject playerHandObject;
    public GameObject treasurePanel;
    public GameObject restSitePanel;
    public GameObject mysteryPanel;
    public GameObject treasurePanelButton;
    public GameObject panelLose;
    public GameObject shopPanel;
    public GameObject cardToCreate;

    #endregion

    #region GameObjects
    public GameObject goldRewardGameObject;
    public GameObject enemyTurnTextPanel;
    public GameObject panelWin;
    public GameObject characterCanvas;
    public GameObject vfxCanvas;
    public GameObject canvas;
    public GameObject cardHolder;
    public GameObject battleUI;
    public GameObject drawDeckButton;
    public GameObject goldTxtTopUI;

    [HideInInspector]public int cardToCreateInt;
    #endregion

    #region Ints
    [Header("Integers")]
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
    public int playerDrawAmount;
    public int drawAmount;
    public int discardAmount;
    #endregion

    #region Bools
    [Header("Bools")]
    public bool canPlayCards = true;
    public bool firstRound = true;
    private bool isAnyoneTargeted;
    public bool canEndTurn;
    #endregion

    #region Buttons
    [Header("Buttons")]
    private Button coinButton;
    private Button healButton;
    public Button endTurn;
    public Button mapButton;
    #endregion

    #region TMP
    [Header("TMPro")]
    public TextMeshProUGUI cardAmount;
    public TextMeshProUGUI darkSoulsText;
    #endregion

    #region Enemy
    [Header("Enemy")]
    public Transform[] enemyBattleStation = new Transform[3];
    public Sprite[] indicatorImages;
    private EnemiesSpawner enemiesSpawner;
    
    #endregion

    #region Player
    [Header("Player")]
    public LevelProgress levelProgress;
    public Player player;
    #endregion

    #region Card
    private CardAlign cardAlign;
    [HideInInspector] public cardPlayed lastCardPlayed;
    #endregion

    #region Ui
    [Header("UI")]
    public UiActive ui;

    #endregion

    #region Shop
    private ShopManager shopmanager;
    #endregion

    #region Map
    [Header("Map")]
    public Map.MapPlayerTracker map;
    public Map.ScrollNonUI scroll;
    #endregion

    #region Lists
    [Header("Lists")]
    public List<GameObject> ItemsInInventory = new List<GameObject>();
    public List<GameObject> allRelicsList = new List<GameObject>();

    public List<GameObject> allCardsCreatable = new List<GameObject>();

    //lista wszystkich kart w grze, wa¿ne ¿eby dodawaæ je po kolei 
    public List<GameObject> allCardsSHOP = new List<GameObject>();

    //lista kart ktore posiada gracz na poczatku
    public List<GameObject> startingDeck = new List<GameObject>();

    public List<GameObject> startingDeckKatana = new List<GameObject>();

    public List<GameObject> startingDeckBaseball = new List<GameObject>();

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
    //[HideInInspector] public float delay;
    public GameObject Shopkeep;

    public List<Enemy> enemyType = new List<Enemy>();
    #endregion

    public void SwitchWeapon()
    {
        if (state == BattleState.ENEMYTURN)
            return;
        if (currentWeapon == Weapon.Palka)
        {
            currentWeapon = Weapon.Katana;
            //OnClick();

            drawDeck.Clear();

            startingDeck[0] = startingDeckKatana[0];
            startingDeck[1] = startingDeckKatana[1];
            startingDeck[2] = startingDeckKatana[2];
            startingDeck[3] = startingDeckKatana[3];
            startingDeck[4] = startingDeckKatana[4];
            startingDeck[5] = startingDeckKatana[5];
            startingDeck[6] = startingDeckKatana[6];
            startingDeck[7] = startingDeckKatana[7];

            var hand = GameObject.Find("PlayerHand");
            for (int i = 0; i < playerHand.Count; i++)
            {
                Destroy(hand.transform.GetChild(i).gameObject);
            }

            playerHand.Clear();

            drawDeck.Clear();
            discardDeck.Clear();
            drawDeck.AddRange(startingDeck);
            OnClick();
        }
        else if (currentWeapon == Weapon.Katana)
        {
            currentWeapon = Weapon.Palka;

            drawDeck.Clear();

            startingDeck[0] = startingDeckBaseball[0];
            startingDeck[1] = startingDeckBaseball[1];
            startingDeck[2] = startingDeckBaseball[2];
            startingDeck[3] = startingDeckBaseball[3];
            startingDeck[4] = startingDeckBaseball[4];
            startingDeck[5] = startingDeckBaseball[5];
            startingDeck[6] = startingDeckBaseball[6];
            startingDeck[7] = startingDeckBaseball[7];

            var hand = GameObject.Find("PlayerHand");
            for (int i = 0; i < playerHand.Count; i++)
            {
                Destroy(hand.transform.GetChild(i).gameObject);
            }

            playerHand.Clear();

            drawDeck.Clear();
            discardDeck.Clear();
            drawDeck.AddRange(startingDeck);
            OnClick();
        }

    }


    private void Awake()
    {

        GetInstance();


        Debug.Log(playerHandObject.GetComponent<RectTransform>().anchoredPosition);

        primaryWeapon = Weapon.Brak;
        secondaryWeapon = Weapon.Brak;
        cardAlign = GameObject.Find("PlayerHand").GetComponent<CardAlign>();
        enemiesSpawner = FindObjectOfType<EnemiesSpawner>();
        shopmanager = GetComponent<ShopManager>();


        choosingNode();


        canEndTurn = true;


    }

    public static GameplayManager GetInstance()
    {
        if (instance == null)
        {
            instance = new GameplayManager();
        }
        return instance;
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

        cardAmount.text = startingDeck.Count.ToString();
        goldTxtTopUI.GetComponent<TextMeshProUGUI>().text = gold.ToString();
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
        
        exhaustedDeck.Clear();
        player.OnBattleSetup();

        yield return new WaitForSeconds(0.1f);

        if (currentFloor < 4)
        {
            numOfList = UnityEngine.Random.Range(0, 3);
        }
        else if (currentFloor < 7)
        {
            numOfList = UnityEngine.Random.Range(3, 6);
        }
        else
        {
            numOfList = UnityEngine.Random.Range(6, 10);
        }
        Debug.Log("Num of list : " + numOfList);
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
            var random = UnityEngine.Random.Range(1, allRelicsList.Count - 1);
            Instantiate(allRelicsList[random], treasurePanel.transform);
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
        var random = UnityEngine.Random.Range(0, Mistery.Count);
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
        if(OnTurnStart!=null)
        OnTurnStart(this, EventArgs.Empty);
        //state = BattleState.PLAYERTURN;
        drawAmount = 0;
        DrawCards(basePlayerDrawAmount);
        player.AssignMana();
        player.ResetImg();
        canPlayCards = true;




        List<GameObject> _indicators = new List<GameObject>();
        _indicators.AddRange(GameObject.FindGameObjectsWithTag("EnemyIndicator"));

        foreach (var indicator in _indicators)
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
        //animacja œmierci
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
            random = UnityEngine.Random.Range(0, allCardsSHOP.Count);
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
                    retain.Add(playerHand[cardIndex]);
                    playerHand.RemoveAt(cardIndex);
                    //zbierz index player handu.
                    temp.Add(card.gameObject);
                    card.transform.SetParent(battleUI.transform);

                }
            var phCount = playerHand.Count;
            var cardsToTransform = new List<GameObject>();
            foreach(Transform obj in playerHandObject.transform)
            {
                cardsToTransform.Add(obj.gameObject);
            }



            playerHand.ForEach(item => discardDeck.Add(item));
            playerHand.ForEach(item => discardAmount++);
            playerHand.Clear();
            for (var i =phCount -1;i>=0;i--)
            {
                //cards[i].GetComponent<RectTransform>().DOAnchorPos(discardDeckButton.GetComponent<RectTransform>().anchoredPosition,0.2f);
                cardsToTransform[i].transform.DORotate(new Vector3(0, 0, -90),0.2f);
                cardsToTransform[i].transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f),0.2f);
                await Task.Delay(100);
                cardsToTransform[i].transform.DOMove(discardDeckButton.transform.position, 0.2f);
                await Task.Delay(200);
                //var cardToDestroyIndex=cardsToTransform[i].transform.GetSiblingIndex();
                //Destroy(playerHandObject.transform.GetChild(i).gameObject);
                DestroyObject(cardsToTransform[i].gameObject);
            }

            playerHand.AddRange(retain);
            temp.ForEach(obj => obj.transform.SetParent(playerHandObject.transform));
            cardsToTransform.Clear();
        }
        player.energize = 0;
        Destroy(player.energizeIndicator);
        foreach (Card obj in FindObjectsOfType<Card>())
            obj.cost = obj.baseCost;

        await ExecuteDarkSoulsText("Enemy Turn");
        cardAlign.SetValues();
        if(OnTurnEnd!=null)
        OnTurnEnd(this, EventArgs.Empty);

        state = BattleState.ENEMYTURN;
        OnEnemiesTurn();

    }
    public void DrawCards(int amount)
    {
        state = BattleState.DRAWING;
        playerDrawAmount = amount;
        drawAmount++;
        if (playerDrawAmount >= drawAmount)
        {
            if (OnDraw != null)
                OnDraw(this, EventArgs.Empty);
            if (drawDeck.Count == 0)
            {
                shuffleDeck();
            }
            var random = UnityEngine.Random.Range(0, drawDeck.Count);
            GameObject card = Instantiate(drawDeck[random], drawDeckButton.GetComponent<RectTransform>().anchoredPosition, transform.rotation);
            //Debug.Log(drawDeckButton.GetComponent<RectTransform>().anchoredPosition);
            card.transform.SetParent(cardAlign.gameObject.transform);
            card.GetComponent<Card>().index = card.transform.GetSiblingIndex();
            //Debug.Log(card.GetComponent<RectTransform>().anchoredPosition);
            var updateValue = drawDeckButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            updateValue.text = (drawDeck.Count - 1).ToString();
            card.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            playerHand.Add(drawDeck[random]);
            drawDeck.RemoveAt(random);
            cardAlign.Animate();

        }
        if (playerDrawAmount < drawAmount)
            drawAmount = 0;
    }
    /// <summary>
    /// select wybor = wybor z ktorego decku bedziemy dociagac karte 0=discard, 1 = drawdeck,2=exhaust deck
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="num"></param>
    /// <param name="select"></param>
    public void CreateCard(GameObject obj,int num,int select)
    {
        //var card = PrefabUtility.InstantiatePrefab(allCards[cardIndex]as GameObject) as GameObject;
        if (num < 0)
        {
            var card = Instantiate(obj, Vector3.zero, Quaternion.identity);
            playerHand.Add(obj);
            card.transform.SetParent(cardAlign.gameObject.transform);
            card.transform.localScale = Vector3.one;
        }
        else
        {
            GameObject instance;
            //check z ktorej listy ma byc obiekt
            if (select == 0)
                instance = Instantiate(discardDeck[num], Vector3.zero, Quaternion.identity);
            else if (select == 1)
                instance = Instantiate(drawDeck[num], Vector3.zero, Quaternion.identity);
            else instance = Instantiate(exhaustedDeck[num], Vector3.zero, Quaternion.identity);
            //playerHand.Add(instance);
            playerHand.Add(discardDeck[num]);
            instance.transform.SetParent(cardAlign.gameObject.transform);
            instance.transform.localScale = Vector3.one;
            
        }
        cardAlign.SetValues();

    }
    private void shuffleDeck()
    {
        discardDeck.ForEach(item => drawDeck.Add(item));
        discardDeck.Clear();
        discardAmount = 0;
        discardDeckButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = discardAmount.ToString();
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
            if (cost == 0) canPlayCards = true;

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
        enemyTurnTextPanel.SetActive(true);
        darkSoulsText.text = _text;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(darkSoulsText.DOFade(1, 1.5f));
        sequence.Insert(0, darkSoulsText.transform.DOScale(1.5f, 3f));
        sequence.OnComplete(() =>
        {
            darkSoulsText.DOFade(0, 0);
            darkSoulsText.transform.DOScale(1f, 0);
            enemyTurnTextPanel.SetActive(false);
        });
        await Task.Delay(3500);



    }
    private void SpawnEnemies(List<GameObject> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
            if (enemies.Count > 1)
                Instantiate(enemies[i], enemyBattleStation[i].transform.position, Quaternion.identity, enemyBattleStation[i].transform);
            else
                Instantiate(enemies[i], enemyBattleStation[i + 1].transform.position, Quaternion.identity, enemyBattleStation[i + 1].transform);
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
        gold = Mathf.RoundToInt(gold * 1.3f);
        goldTxtTopUI.GetComponent<TextMeshProUGUI>().text = gold.ToString();
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
    public enum Weapon
    {
        Palka,
        Kostur,
        Ksiazka,
        Alchemia,
        Topor,
        Katana,
        Brak
    }
    public void SwitchWeapons()
    {

        //koszt many dodac
        //dodac switch miejsc w ktorych siedza itemy w eq.
        if (state == BattleState.ENEMYTURN)
            return;
        if (secondaryWeapon == Weapon.Brak)
            return;
        var getEnum = primaryWeapon;
        primaryWeapon = secondaryWeapon;
        secondaryWeapon = getEnum;
    }
    public void UpdateSomeValues(object sender, EventArgs e)
    {
    }
}

