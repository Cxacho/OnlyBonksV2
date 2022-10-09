using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using System;


public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int baseCost;
    public int cost;
    public int scaleCardValues;
    public int index;
    private int numOfTargets;
    public int baseNumOfTargets;
    bool canCreate;
    private float posY;
    public float attack;
    public float defaultattack;
    public int getPanel;
    public GameplayManager.Weapon myWeaponType;
    public cardType cType;
    public cardState currentCardState;
    public scalingType cardScalingtype;
    public scalingType secondaryScalingType;
    public damageType cardDamageType;
    private Vector3 discDek;
    public GameObject exitButton;
    protected FollowMouse followMouse;
    [HideInInspector]public UiActive ui;
    [HideInInspector] public Player player;
    List<RectTransform> trails = new List<RectTransform>();
    GameObject playArea;
    private Quaternion oldRot;
    private Quaternion newRot;
    private Quaternion hoverRotation = new Quaternion(0, 0, 0, 0);

    [HideInInspector] public CardAlign cardAlign;
    [HideInInspector] public GameplayManager gameplayManager;
    private RectTransform pos;
    private GameObject par;
    TextMeshProUGUI manaCostTxt;
    [SerializeField] bool interactible;
    [SerializeField] private Vector2 trailOffset= new Vector2(110,170);

    //private float clickDelay;
    public  List<GameObject> meshes = new List<GameObject>();

    [HideInInspector] public List<Enemy> _enemies = new List<Enemy>();

    private List<GameObject> temp = new List<GameObject>();


    [SerializeField] private bool exhaustable;
    [SerializeField] bool isNeutral;
    public bool retainable;


    [HideInInspector] public string desc;

    private BasicAttack bs;


    private void Start()
    {
        currentCardState = cardState.Elsewhere;

    }
    IEnumerator Return()
    {
        yield return new WaitForSeconds(0.15f);
        if (followMouse.crd == null)
            cardAlign.SetValues();
        /*
        for (int i = 0; i < cardAlign.gameObject.transform.childCount; i++)
        {
            cardAlign.children[i].transform.DOMove(cardAlign.positions[i], 0.1f);
        }
        */
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (this.transform.IsChildOf(GameObject.Find("PlayerHand").transform) && gameplayManager.playerHand.Count == par.transform.childCount && gameplayManager.state == BattleState.PLAYERTURN)
        {
            followMouse.crd = null;
            this.transform.localScale = Vector3.one;
            transform.rotation = hoverRotation;
            transform.position = new Vector3(this.transform.position.x, posY, this.transform.position.z);
            this.transform.SetSiblingIndex(index);
            StartCoroutine(Return());
            currentCardState = cardState.InHand;

        }

        cardAlign.helpingGO = null;
        if (currentCardState == cardState.Creatable)
        {
            canCreate = false;
            gameplayManager.cardToCreateInt = -1;
            currentCardState = cardState.Elsewhere;
            gameplayManager.cardToCreate = null;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        cardAlign.helpingGO = this.gameObject;
        if (this.transform.IsChildOf(GameObject.Find("PlayerHand").transform) && gameplayManager.playerHand.Count == par.transform.childCount && gameplayManager.state == BattleState.PLAYERTURN)
        {
            followMouse.crd = GetComponent<Card>();
            hoverRotation = this.transform.rotation;
            this.transform.localScale += new Vector3(0.15f, 0.15f, 0.15f);
            transform.rotation = Quaternion.identity;
            posY = this.transform.position.y;
            currentCardState = cardState.OnMouse;
            index = this.transform.GetSiblingIndex();
            cardAlign.cardIndex = index;
            cardAlign.Realign();
            this.transform.SetAsLastSibling();
        }
        if (eventData.pointerEnter.transform.parent.GetComponent<Card>() != null)
            cardAlign.pointerHandler = eventData.pointerEnter.transform.parent.GetSiblingIndex();
        else
            cardAlign.pointerHandler = eventData.pointerEnter.transform.parent.transform.parent.GetSiblingIndex();
        /*if (currentCardState == cardState.Elsewhere)
        {
            canCreate = true;
            gameplayManager.cardToCreateInt = transform.GetSiblingIndex();
            currentCardState = cardState.Creatable;
            gameplayManager.cardToCreate = this.gameObject;
        }*/
    }

    public void resetTargetting()
    {
        foreach (Enemy enemy in gameplayManager.enemyType)
        {

            enemy.targeted = false;
            enemy.isFirstTarget = false;
            enemy.isSecondTarget = false;
            enemy.isThirdTarget = false;

        }

    }





    void Awake()
    {
        playArea = GameObject.FindGameObjectWithTag("PlayArea");
        trailOffset=new Vector2(110, 170);
        ui = GameObject.FindObjectOfType<UiActive>();
        manaCostTxt = transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        manaCostTxt.text = baseCost.ToString();
        cost = baseCost;
        par = GameObject.Find("PlayerHand");
        cardAlign = par.GetComponent<CardAlign>();
        meshes.AddRange(GameObject.FindGameObjectsWithTag("Indicator"));
        player = GameObject.Find("Player").GetComponent<Player>();
        discDek = GameObject.Find("Discard_Deck_Button").transform.position;
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        pos = this.transform.GetComponent<RectTransform>();
        followMouse = GameObject.Find("Cursor").GetComponent<FollowMouse>();
        defaultattack = attack;
        this.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = cost.ToString();

    }
    void Update()
    {
        StateCheck();
        manaCostTxt.text = cost.ToString();
    }

    void StateCheck()
    {
        switch (currentCardState)
        {
            case cardState.InHand:
                //animacja ruchu + skala danych kart 
                break;
            case cardState.OnMouse:
                //if(parent == playershand)
                //OnClick()=>>oncursour;
                //else if (GetComponentInParent != playershand)
                //Inspect() fire2 == inspect
                OnClick();
                break;
            case cardState.OnCursor:
                Move();
                DropControl();
                break;
            case cardState.Elsewhere:
                break;
            case cardState.Targetable:
                DropControl();
                break;
            case cardState.Interactible:
                InteractWithOther();
                break;
            case cardState.Creatable:
                ChooseOfCreationMenu();
                break;
        }

    }
    public void calc(int baseDamage, scalingType type, scalingType secondType)
    {
        if (currentCardState == cardState.Elsewhere) return;
        switch (type)
        {
            case scalingType.strength:
                if (secondType == scalingType.dexterity)
                    scaleCardValues = player.strenght + player.dexterity;
                else if (secondType == scalingType.magic)
                    scaleCardValues = player.strenght + player.inteligence;
                else
                    scaleCardValues = player.strenght;
                break;
            case scalingType.dexterity:
                if (secondType == scalingType.strength)
                    scaleCardValues = player.strenght + player.dexterity;
                else if (secondType == scalingType.magic)
                    scaleCardValues = player.dexterity + player.inteligence;
                else
                    scaleCardValues = player.dexterity;
                break;
            case scalingType.magic:
                if (secondType == scalingType.strength)
                    scaleCardValues = player.strenght + player.inteligence;
                else if (secondType == scalingType.dexterity)
                    scaleCardValues = player.dexterity + player.inteligence;
                else
                    scaleCardValues = player.inteligence;
                break;
        }
        //na najechaniu na przeciwnika
        if (followMouse.en != null)
        {
            if (followMouse.en.vurneable > 0 && player.frail > 0)
            {
                attack = defaultattack + scaleCardValues;
            }
            else if (followMouse.en.vurneable > 0)
                attack = (defaultattack + scaleCardValues) * 1.25f;
            else if (player.frail > 0)
                attack = (defaultattack + scaleCardValues) * 0.75f;
            else
            {
                attack = defaultattack + scaleCardValues;
            }

        }
        else if (followMouse.en == null)
        {
            if (player.frail > 0)
                attack = (defaultattack + scaleCardValues) * 0.75f;
            else
                attack = defaultattack + scaleCardValues;
        }
        attack = Mathf.RoundToInt(attack);
    }

    public enum scalingType
    {
        brak = 0,
        strength = 1,
        dexterity = 2,
        magic = 3
    }
    public enum cardState
    {
        InHand = 0,
        OnMouse = 1,
        OnCursor = 2,
        Elsewhere = 3,
        Targetable = 4,
        Interactible = 5,
        Creatable,
    }
    void ReturnToHand()
    {
        playArea.GetComponent<playAreaAnimator>().killTweens();
        foreach(RectTransform obj in trails)
            Destroy(obj.gameObject);
        followMouse.rect[0].anchoredPosition = new Vector3(0, -400, 0);
        this.transform.localScale = Vector3.one;
        currentCardState = cardState.InHand;
        transform.SetParent(GameObject.Find("PlayerHand").transform);
        numOfTargets = baseNumOfTargets;
        _enemies.Clear();
        _enemies.AddRange(gameplayManager.enemyType);
        foreach (Enemy en in _enemies)
        {

            en.targeted = false;
            en.isFirstTarget = false;
            en.isSecondTarget = false;
            en.isThirdTarget = false;

        }
        this.transform.position = cardAlign.positions[index];
        this.transform.rotation = hoverRotation;
        this.transform.SetSiblingIndex(index);
    }
    void DisableIndicator()
    {
        Cursor.visible = true;
        foreach (RectTransform obj in followMouse.rect)
        {
            obj.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
           {
               for (int i = 0; i < meshes.Count-1; i++)
               {
                   meshes[i].GetComponent<Image>().enabled = false;
               }
           });
        }


    }
    void EnableIndicator()
    {
        Cursor.visible = false;
        for (int i =0;i<meshes.Count-1;i++)
        {
            meshes[i].GetComponent<Image>().enabled = true;
        }

        for (int i = 0; i < followMouse.objs.Count; i++)
        {
            followMouse.rect[i].DOScale(followMouse.spriteScale[i], 0.5f);
        }
    }
    void OnClick()
    {

        //if (myWeaponType != gameplayManager.primaryWeapon && isNeutral == false)
           // Debug.Log("cant play due to card to weapon type difference");
        //return;
        if (player.mana - cost < 0)
            return;



        //dodac visual effect, lub tmp, ze nie masz wystarczajaco duzo many

        if (Input.GetButton("Fire1"))
        {
            playArea.GetComponent<playAreaAnimator>().doUnfade();
            trails.Clear();
            for (int i = 0; i < 2; i++)
            {
                var Vfx = Instantiate(followMouse.trailVFX, followMouse.rectPos.anchoredPosition, Quaternion.identity, gameplayManager.vfxCanvas.transform);

                trails.Add(Vfx.GetComponent<RectTransform>());
            }
            var centerTrail = Instantiate(followMouse.centerTrailVFX, followMouse.rectPos.anchoredPosition, Quaternion.identity, gameplayManager.vfxCanvas.transform);
            trails.Add(centerTrail.GetComponent<RectTransform>());
            oldRot = this.transform.rotation;
            currentCardState = cardState.OnCursor;
            this.transform.rotation = newRot;
            this.transform.SetParent(gameplayManager.canvas.transform);

        }
    }
    void Move()
    {
        trails[0].anchoredPosition = followMouse.rectPos.anchoredPosition+new Vector2(-trailOffset.x,-trailOffset.y);
        trails[1].anchoredPosition = followMouse.rectPos.anchoredPosition + new Vector2(trailOffset.x, -trailOffset.y);
        trails[2].anchoredPosition = followMouse.rectPos.anchoredPosition;
        pos.anchoredPosition = followMouse.rectPos.anchoredPosition;
        if (Input.GetButton("Fire2"))
        {
            ReturnToHand();
            DisableIndicator();
        }



        //na wyjsciu z viewportu karta ma wracac do reki
    }
    public enum cardType
    {
        Attack = 0,
        Skill = 1,
        Power = 2
    }
    public enum damageType
    {
        physical,
        cold,
        fire,
        earth,
        wind
    }
    public virtual void OnDrop()
    {
        playArea.GetComponent<playAreaAnimator>().killTweens();
        foreach (RectTransform obj in trails)
            Destroy(obj.gameObject);
        if (cType == cardType.Attack)
        {
            gameplayManager.cardsPlayed++;
            gameplayManager.attackCardsPlayed++;
            gameplayManager.lastCardPlayed = GameplayManager.cardPlayed.attack;

        }
        else if (cType == cardType.Skill)
        {
            gameplayManager.cardsPlayed++;
            gameplayManager.skillCardsPlayed++;
            gameplayManager.lastCardPlayed = GameplayManager.cardPlayed.skill;
        }
        else
        {
            gameplayManager.cardsPlayed++;
            gameplayManager.powerCardsPlayed++;
            gameplayManager.lastCardPlayed = GameplayManager.cardPlayed.power;
        }
        //mechanika energize

        if (exhaustable)
        {
            if (gameplayManager.OnCardPlayedDetail != null)
                gameplayManager.OnCardPlayedDetail(this, attack, cost);
            currentCardState = cardState.Elsewhere;
            gameplayManager.OnCardExhausted += gameplayManager.UpdateSomeValues;
            if (gameplayManager.OnCardExhausted != null)
                gameplayManager.OnCardExhausted(this, EventArgs.Empty);
            //play card
            //anim
            var go = this.gameObject;
            var nazwaObiektu = go.name.Remove(go.name.Length - 7);
            for (int i = 0; i < gameplayManager.playerHand.Count; i++)
            {
                if (nazwaObiektu.Equals(gameplayManager.playerHand[i].name))
                {
                    temp.Add(gameplayManager.playerHand[i]);
                    gameplayManager.exhaustedDeck.Add(temp[0]);
                    gameplayManager.playerHand.RemoveAt(i);
                    temp.RemoveAt(0);
                }
            }
            transform.DOMove(new Vector3(discDek.x, discDek.y, 0), 1.5f);
            transform.DOScale(0.25f, 0.5f);
            transform.DORotate(new Vector3(0, 0, -150f), 1.5f).OnComplete(() =>
            {
                this.transform.localScale = Vector3.one;
                this.transform.rotation = newRot;
                Destroy(this.gameObject);
            }

            );
            cardAlign.SetValues();
        }
        else
        {
            if(gameplayManager.OnCardPlayedDetail!=null)
            gameplayManager.OnCardPlayedDetail(this, attack, cost);
            gameplayManager.OnCardPlayed += gameplayManager.UpdateSomeValues;
            if (gameplayManager.OnCardPlayed != null)
                gameplayManager.OnCardPlayed(this, EventArgs.Empty);
            currentCardState = cardState.Elsewhere;
            //play card
            //anim
            var go = this.gameObject;
            var nazwaObiektu = go.name.Remove(go.name.Length - 7);
            for (int i = 0; i < gameplayManager.playerHand.Count; i++)
            {
                if (nazwaObiektu.Equals(gameplayManager.playerHand[i].name))
                {
                    temp.Add(gameplayManager.playerHand[i]);
                    gameplayManager.discardDeck.Add(temp[0]);
                    gameplayManager.playerHand.RemoveAt(i);
                    temp.RemoveAt(0);
                }
            }
            transform.DOMove(new Vector3(discDek.x, discDek.y, 0), 1.5f);
            transform.DOScale(0.25f, 0.5f);
            transform.DORotate(new Vector3(0, 0, -150f), 1.5f).OnComplete(() =>
            {
                Destroy(this.gameObject);
            }

            );
            cardAlign.SetValues();
        }
    }
    public void DropControl()
    {
        if (cType == cardType.Attack)
        {
            if (pos.anchoredPosition.y > -90)
            {
                numOfTargets = baseNumOfTargets;
                _enemies.Clear();
                _enemies.AddRange(gameplayManager.enemyType);
                if (numOfTargets > _enemies.Count)
                {
                    numOfTargets = _enemies.Count;
                }
                // else
                // Debug.Log(enemies.Count);
                //podniesienie ataku ponad -90, przesuniencie atakku do poz srodka renki
                EnableIndicator();
                currentCardState = cardState.Targetable;
                gameplayManager.canEndTurn = false;
                pos.anchoredPosition = new Vector3(0, -400, 0);
            }
            if (followMouse.en != null)
            {
                if (Input.GetButtonUp("Fire1") && followMouse.en.targeted == false && numOfTargets > 0)
                {
                    //wybor targetu, gdy liczba 
                    //dodatkowy warunek od enemies.count tak jak w lini 215
                    if (_enemies.Count >= 3)
                    {
                        if (numOfTargets == baseNumOfTargets)
                            followMouse.en.isFirstTarget = true;

                        else if (numOfTargets == baseNumOfTargets - 1)
                            followMouse.en.isSecondTarget = true;
                        else if (numOfTargets == baseNumOfTargets - 2)
                            followMouse.en.isThirdTarget = true;
                    }

                    else if (baseNumOfTargets <= 3 & _enemies.Count == 1)
                        followMouse.en.isFirstTarget = true;
                    else

                    {
                        //warunek dla 3 przeciwnikow
                        if (_enemies.Count == 2 &&baseNumOfTargets == 3)
                        {
                            //to sie chyba nigdy nie wywo
                            if (numOfTargets == baseNumOfTargets - 1)
                                followMouse.en.isFirstTarget = true;
                            else if (numOfTargets == baseNumOfTargets - 2)
                                followMouse.en.isSecondTarget = true;
                        }
                        else
                        {
                            if (numOfTargets == baseNumOfTargets)
                                followMouse.en.isFirstTarget = true;
                            if (numOfTargets == baseNumOfTargets - 1)
                                followMouse.en.isSecondTarget = true;
                        }
                    }

                    followMouse.en.targeted = true;
                    numOfTargets -= 1;
                    if (numOfTargets == 0)
                        DisableIndicator();
                }
                else if (Input.GetButtonUp("Fire1") && followMouse.en.targeted == true && numOfTargets >= 0)
                {
                    //cofnienice zaznaczenia
                    if (numOfTargets == baseNumOfTargets - 3 && followMouse.en.isThirdTarget == true)
                    {
                        followMouse.en.targeted = false;
                        followMouse.en.isThirdTarget = false;
                        numOfTargets += 1;
                    }
                    else if (numOfTargets == baseNumOfTargets - 2 && followMouse.en.isSecondTarget == true)
                    {
                        followMouse.en.targeted = false;
                        followMouse.en.isSecondTarget = false;
                        numOfTargets += 1;
                    }
                    else if (numOfTargets == baseNumOfTargets - 1 && followMouse.en.isFirstTarget == true)
                    {
                        followMouse.en.targeted = false;
                        followMouse.en.isFirstTarget = false;
                        numOfTargets += 1;
                    }



                    if (numOfTargets == 1)
                        EnableIndicator();
                }

            }
            if (Input.GetButtonUp("Fire1") && currentCardState == cardState.Targetable && numOfTargets == 0)
            {
                //zagranie ataku gdy liczba targetow rowna zero i nie ma targetu na myszcze
                DisableIndicator();
                OnDrop();
                //play card
                //trail.enabled = true;
                //anim
                gameplayManager.canEndTurn = true;
            }
            //cofnienicie karty do reki reset indeksu reset targetowania
            if (Input.GetButton("Fire2") && currentCardState == cardState.Targetable)
            {
                ReturnToHand();
                DisableIndicator();
                cardAlign.SetValues();
                gameplayManager.canEndTurn = true;
                //move to discard pile || exhaust


            }
        }
        else if (cType == cardType.Power || cType == cardType.Skill)
        {
            if (Input.GetButtonUp("Fire1") && pos.anchoredPosition.y > -90)
            {
                gameplayManager.canEndTurn = true;
                _enemies.Clear();
                _enemies.AddRange(gameplayManager.enemyType);
                if (interactible == false)
                    OnDrop();
                else
                {
                    currentCardState = cardState.Interactible;
                    transform.DOMove(Vector3.zero, 1);
                    followMouse.rect[0].anchoredPosition = Vector3.zero;
                    EnableIndicator();
                    //przeniesc pierwzsy element indicatora na srodek gry
                }
            }


        }

        if (followMouse.viewPortPosition.x < 0 || followMouse.viewPortPosition.x > 1 || followMouse.viewPortPosition.y < 0 || followMouse.viewPortPosition.y > 1)
        {
            gameplayManager.canEndTurn = true;
            ReturnToHand();
            DisableIndicator();
            cardAlign.SetValues();
        }

    }
    void InteractWithOther()
    {
        //show text explaining action to take np choose target card

        if (Input.GetButton("Fire1") && cardAlign.helpingGO != null && cardAlign.helpingGO != gameObject)
        {
            DisableIndicator();
            followMouse.rect[0].anchoredPosition = new Vector3(0, -400, 0);
            CastOnPlay();
            cardAlign.helpingGO = null;
            OnDrop();
        }
        if (Input.GetButton("Fire2"))
        {
            ReturnToHand();
            DisableIndicator();
        }
    }
    /// <summary>
    /// type = rodzaj buffa, np koszt zmniejsza koszt wybranej karty, attack zwieksza atak danej karty
    /// 0 koszt, 1 atak,2 discard
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    public void ApplyEffectToCard(int value, int type, Card obj)
    {
        if (type == 0)
            if (value > obj.cost)
                obj.cost = 0;
            else
                obj.cost -= value;
        else if (type == 1)
            if (obj.cType == Card.cardType.Attack)
                obj.defaultattack += value;
            else
                Debug.Log("cant apply attack to skill");
        else if (type == 2)
        {

            //discard anim
            currentCardState = cardState.Elsewhere;
            //play card
            //anim
            var go = cardAlign.helpingGO;
            var nazwaObiektu = go.name.Remove(go.name.Length - 7);
            for (int i = 0; i < gameplayManager.playerHand.Count; i++)
            {
                if (nazwaObiektu.Equals(gameplayManager.playerHand[i].name))
                {
                    temp.Add(gameplayManager.playerHand[i]);
                    gameplayManager.discardDeck.Add(temp[0]);
                    gameplayManager.playerHand.RemoveAt(i);
                    temp.RemoveAt(0);
                }
            }
            gameplayManager.state = BattleState.DRAWING;
            go.transform.SetParent(FindObjectOfType<Canvas>().transform);
            go.transform.DOMove(new Vector3(discDek.x, discDek.y, 0), 1.5f);
            go.transform.DOScale(0.25f, 0.5f);
            go.transform.DORotate(new Vector3(0, 0, -150f), 1.5f).OnComplete(() =>
            {
                gameplayManager.state = BattleState.PLAYERTURN;
                Destroy(go);

            }

            );
        }
    }
    public virtual void CastOnPlay()
    {

    }
    /// <summary>
    /// int select oznacza wybor ktory panel otwieramy 0=discarddeck,1=drawddeck,2 exhaust???
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    public void ChooseOfCreationMenu()
    {
        if (gameplayManager.state != BattleState.CREATING)
        {


            if (Input.GetButtonUp("Fire1") && canCreate == true && gameplayManager.state == BattleState.PLAYERTURN && ui.cardToInspect == null)
            {
                GameObject inspected = null;
                Button currentButton = null;
                if (ui.panelIndex != 2 && ui.panelIndex != 3 && ui.panelIndex !=1)
                    return;
                canCreate = false;
                switch (ui.panelIndex)
                {
                    //deck layout
                    case 1:
                        currentButton=ui.deckPanel.transform.GetChild(1).GetComponent<Button>();
                        inspected = Instantiate(gameplayManager.startingDeck[gameplayManager.cardToCreateInt], Vector3.zero, Quaternion.identity);
                        break;
                    //deck
                    case 2:
                        currentButton = ui.drawDeckPanel.transform.GetChild(1).GetComponent<Button>();
                        inspected = Instantiate(gameplayManager.drawDeck[gameplayManager.cardToCreateInt], Vector3.zero, Quaternion.identity);
                        break;
                        //discard
                    case 3:
                        currentButton=ui.discardDeckPanel.transform.GetChild(1).GetComponent<Button>();
                        inspected = Instantiate(gameplayManager.discardDeck[gameplayManager.cardToCreateInt], Vector3.zero, Quaternion.identity);
                        break;
                        //exhaustdeck
                }


                currentButton.interactable = false;
                inspected.transform.SetParent(gameplayManager.canvas.transform);
                inspected.transform.localScale = Vector3.one;
                inspected.transform.DOScale(new Vector3(2.5f, 2.5f, 2.5f), 0.5f);
                ui.cardToInspect = inspected;
                var obj = Instantiate(exitButton, new Vector3(0, -30, 0), Quaternion.identity);

                obj.transform.SetParent(gameplayManager.canvas.transform);
                obj.transform.localScale = Vector3.one;
                obj.GetComponent<exitButton>().cardToDestroy =inspected;
                obj.GetComponent<exitButton>().button = currentButton;
                ui.SetButtonsNonInterractible();
                //instantiate button ktory usuwa wszystko i resetuje buttony
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && canCreate == true)
            {
                // czy nie pociagnie karty przy klikniecniu jej
                gameplayManager.CreateCard(gameplayManager.cardToCreate, gameplayManager.cardToCreateInt, getPanel);
                //zaleznie od panelu z ktorego ciagniemy
                gameplayManager.discardDeck.RemoveAt(gameplayManager.cardToCreateInt);
                ui.EnableButtons(0);
                gameplayManager.state = BattleState.PLAYERTURN;
                canCreate = false;
            }
        }
    }
    private void OnDestroy()
    {
        if (gameplayManager.OnCardExhausted != null)
            gameplayManager.OnCardExhausted -= gameplayManager.UpdateSomeValues;
        if (gameplayManager.OnCardPlayed != null)
            gameplayManager.OnCardPlayed -= gameplayManager.UpdateSomeValues;
    }
}