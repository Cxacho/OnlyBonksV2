using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;


public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int cost;
    public int scaleCardValues;
    public int index;
    private int numOfTargets;
    public int baseNumOfTargets;

    private float posY;
    public float attack;
    public float defaultattack;
    [HideInInspector] public float kalkulacja;
    [HideInInspector] public float kalkulacjaPrzeciwnik;


    public cardType cType;
    public cardState currentCardState;
    public scalingType cardScalingtype;
    public scalingType secondaryScalingType;
    public damageType cardDamageType;

    private Vector3 discDek;
        
    protected FollowMouse followMouse;


    [SerializeField] TrailRenderer trail;

    [HideInInspector] public Player player;


    private Quaternion oldRot;
    private Quaternion newRot;
    private Quaternion hoverRotation = new Quaternion(0, 0, 0, 0);

    private CardAlign cardAlign;
    [HideInInspector] public GameplayManager gameplayManager;
    private RectTransform pos;
    private GameObject par;
    
    
    //private float clickDelay;
    private List<GameObject> meshes = new List<GameObject>();

    [HideInInspector]public List<Enemy> _enemies = new List<Enemy>();

    private List<GameObject> temp = new List<GameObject>();
    

    [SerializeField]private bool exhaustable;
    public bool retainable;
    
    

    [HideInInspector]public string desc;

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
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.transform.IsChildOf(GameObject.Find("PlayerHand").transform) && gameplayManager.playerHand.Count == par.transform.childCount && gameplayManager.state== BattleState.PLAYERTURN)
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
            /*
            foreach(Card crd in FindObjectsOfType<Card>())
            {

            }
            */
            
        }
        if (eventData.pointerEnter.transform.parent.GetComponent<Card>() != null)
            cardAlign.pointerHandler = eventData.pointerEnter.transform.parent.GetSiblingIndex();
        else
            cardAlign.pointerHandler = eventData.pointerEnter.transform.parent.transform.parent.GetSiblingIndex();
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
    }

    

    void Awake()
    {

        par = GameObject.Find("PlayerHand");
        cardAlign = par.GetComponent<CardAlign>();
        meshes.AddRange(GameObject.FindGameObjectsWithTag("Indicator"));
        player = GameObject.Find("Player").GetComponent<Player>();
        discDek = GameObject.Find("DiscardDeckButton").transform.position;
        trail = transform.GetComponent<TrailRenderer>();
        gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        pos = this.transform.GetComponent<RectTransform>();
        followMouse = GameObject.Find("Cursor").GetComponent<FollowMouse>();
        defaultattack = attack;
        this.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = cost.ToString();

    }
    void Update()
    {
        StateCheck();
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
                //???
                break;
            case cardState.Targetable:
                DropControl();
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
        brak =0,
        strength =1,
        dexterity =2,
        magic =3

    }
    public enum cardState
    {
        InHand = 0,
        OnMouse = 1,
        OnCursor = 2,
        Elsewhere = 3,
        Targetable = 4
    }
    void ReturnToHand()
    {
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
        foreach(RectTransform obj in followMouse.rect)
        {
            obj.DOScale(Vector3.zero, 0.2f).OnComplete (()=>
            {
                foreach (GameObject obj in meshes)
                {
                    obj.GetComponent<SpriteRenderer>().enabled = false;
                }
            });
        }


    }
    void EnableIndicator()
    {
        Cursor.visible = false;
        foreach (GameObject obj in meshes)
        {
            obj.GetComponent<SpriteRenderer>().enabled = true;
        }
        
        for (int i = 0; i < followMouse.objs.Count; i++)
        {
            followMouse.rect[i].DOScale(followMouse.spriteScale[i], 0.5f);
        }
    }
    void OnClick()
    {
        if (Input.GetButton("Fire1"))
        {

            oldRot = this.transform.rotation;
            currentCardState = cardState.OnCursor;
            this.transform.rotation = newRot;
            this.transform.SetParent(GameObject.Find("Canvas").transform);

        }
    }
    void Move()
    {
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
        if (exhaustable)
                {

                    currentCardState = cardState.Elsewhere;
                    //play card
                    trail.enabled = true;
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
                        trail.enabled = false;
                        this.transform.localScale = Vector3.one;
                        this.transform.rotation = newRot;
                        Destroy(this.gameObject);
                    }

                    );
                }
                else
                {
                    currentCardState = cardState.Elsewhere;
                    //play card
                    trail.enabled = true;
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
                        trail.enabled = false;
                        this.transform.localScale = Vector3.one;
                        this.transform.rotation = newRot;
                        Destroy(this.gameObject);
                    }

                    );
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
                    
                    else if (baseNumOfTargets <=3  & _enemies.Count == 1)
                        followMouse.en.isFirstTarget = true;
                    else

                    {
                        //warunek dla 3 przeciwnikow
                        if (_enemies.Count ==3)
                        {
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
                    //clickDelay = Time.time + 0.3f;
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
                    //clickDelay = Time.time + 0.3f;
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
                OnDrop();
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
}