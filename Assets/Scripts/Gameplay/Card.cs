using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;


public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int cost,scaleCardValues;
    public cardType cType;
    public cardState currentCardState;
    public scalingType cardScalingtype;
    public scalingType secondaryScalingType;
    private Vector3 mousePos, posInHand, discDek;
    protected FollowMouse fm;
    [SerializeField] TrailRenderer trail;
    [HideInInspector] public Player pl;
    Quaternion oldRot, newRot, hoverRotation = new Quaternion(0, 0, 0, 0);
    CardAlign cAlign;
    [HideInInspector] public GameplayManager gm;
    RectTransform pos;
    GameObject par;
    List<GameObject> temp = new List<GameObject>();
    public int index;
    private int numOfTargets;
    public int baseNumOfTargets;
    //private float clickDelay;
    private List<GameObject> meshes = new List<GameObject>();
    [HideInInspector]public List<Enemy> _enemies = new List<Enemy>();
    float posY;
    [SerializeField]private bool exhaustable;
    public bool retainable;
    public int dealDamage;
    public float attack;
    public float defaultattack;
    [HideInInspector] public float kalkulacja;
    [HideInInspector] public float kalkulacjaPrzeciwnik;

    [HideInInspector]public string desc;

    private BasicAttack bs;
    /*public enum textType
    {
        Str,
        Dex
    }*/
    /*public void textUpdate(textType typ)
    {
        if(typ == textType.Str) bs.UpdateAttack();
    }*/
    
    private void Start()
    {
        currentCardState = cardState.Elsewhere;
        
    }
    IEnumerator Return()
    {
        yield return new WaitForSeconds(0.15f);
        if (fm.crd == null)
            cAlign.SetValues();
            /*
            for (int i = 0; i < cAlign.gameObject.transform.childCount; i++)
            {
                cAlign.children[i].transform.DOMove(cAlign.positions[i], 0.1f);
            }
            */
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.transform.IsChildOf(GameObject.Find("PlayerHand").transform) && gm.playerHand.Count == par.transform.childCount && gm.state== BattleState.PLAYERTURN)
        {
            fm.crd = GetComponent<Card>();
            hoverRotation = this.transform.rotation;
            this.transform.localScale += new Vector3(0.15f, 0.15f, 0.15f);
            transform.rotation = Quaternion.identity;
            posY = this.transform.position.y;
            currentCardState = cardState.OnMouse;
            index = this.transform.GetSiblingIndex();
            cAlign.cardIndex = index;
            cAlign.Realign();
            this.transform.SetAsLastSibling();
            /*
            foreach(Card crd in FindObjectsOfType<Card>())
            {

            }
            */
            
        }
        if (eventData.pointerEnter.transform.parent.GetComponent<Card>() != null)
            cAlign.pointerHandler = eventData.pointerEnter.transform.parent.GetSiblingIndex();
        else
            cAlign.pointerHandler = eventData.pointerEnter.transform.parent.transform.parent.GetSiblingIndex();
    }
    public void resetTargetting()
    {
        foreach (Enemy en in gm.enType)
        {

            en.targeted = false;
            en.isFirstTarget = false;
            en.isSecondTarget = false;
            en.isThirdTarget = false;

        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (this.transform.IsChildOf(GameObject.Find("PlayerHand").transform) && gm.playerHand.Count == par.transform.childCount && gm.state == BattleState.PLAYERTURN)
        {
            fm.crd = null;
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
        cAlign = par.GetComponent<CardAlign>();
        meshes.AddRange(GameObject.FindGameObjectsWithTag("Indicator"));
        pl = GameObject.Find("Player").GetComponent<Player>();
        discDek = GameObject.Find("DiscardDeckButton").transform.position;
        trail = transform.GetComponent<TrailRenderer>();
        gm = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        pos = this.transform.GetComponent<RectTransform>();
        fm = GameObject.Find("Cursor").GetComponent<FollowMouse>();
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
        switch (type)
        {
            case scalingType.strength:
                if (secondType == scalingType.dexterity)
                    scaleCardValues = pl.strenght + pl.dexterity;
                else if (secondType == scalingType.magic)
                    scaleCardValues = pl.strenght + pl.inteligence;
                else
                    scaleCardValues = pl.strenght;
                break;
            case scalingType.dexterity:
                if (secondType == scalingType.strength)
                    scaleCardValues = pl.strenght + pl.dexterity;
                else if (secondType == scalingType.magic)
                    scaleCardValues = pl.dexterity + pl.inteligence;
                else
                    scaleCardValues = pl.dexterity;
                break;
            case scalingType.magic:
                if (secondType == scalingType.strength)
                    scaleCardValues = pl.strenght + pl.inteligence;
                else if (secondType == scalingType.dexterity)
                    scaleCardValues = pl.dexterity + pl.inteligence;
                else
                    scaleCardValues = pl.inteligence;
                break;
        }
        //na najechaniu na przeciwnika
        if (fm.en != null)
        {
            if (fm.en.vurneable > 0 && pl.frail > 0)
            {
                attack = defaultattack + scaleCardValues;
            }
            else if (fm.en.vurneable > 0)
                attack = (defaultattack + scaleCardValues) * 1.25f;
            else if (pl.frail > 0)
                attack = (defaultattack + scaleCardValues) * 0.75f;
            else
            {
                attack = defaultattack + scaleCardValues;
            }
            
        }
        else if (fm.en == null)
        {
            if (pl.frail > 0)
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
        _enemies.AddRange(gm.enType);
        foreach (Enemy en in _enemies)
        {

                en.targeted = false;
                en.isFirstTarget = false;
                en.isSecondTarget = false;
                en.isThirdTarget = false;
            
        }
        this.transform.position = cAlign.positions[index];
        this.transform.rotation = hoverRotation;
        this.transform.SetSiblingIndex(index);
    }
    void DisableIndicator()
    {
        Cursor.visible = true;
        foreach(RectTransform obj in fm.rect)
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
        
        for (int i = 0; i < fm.objs.Count; i++)
        {
            fm.rect[i].DOScale(fm.spriteScale[i], 0.5f);
        }
    }
    void OnClick()
    {
        if (Input.GetButton("Fire1"))
        {

            posInHand = pos.anchoredPosition;
            oldRot = this.transform.rotation;
            currentCardState = cardState.OnCursor;
            this.transform.rotation = newRot;
            this.transform.SetParent(GameObject.Find("Canvas").transform);

        }
    }
    void Move()
    {
        pos.anchoredPosition = fm.rectPos.anchoredPosition;
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
    public virtual void OnDrop()
    {
        if (exhaustable)
        {

            currentCardState = cardState.Elsewhere;
            //play card
            trail.enabled = true;
            //anim
            var go = this.gameObject;
            var nazwaObiektu = go.name.Remove(go.name.Length - 7);
            for (int i = 0; i < gm.playerHand.Count; i++)
            {
                if (nazwaObiektu.Equals(gm.playerHand[i].name))
                {
                    temp.Add(gm.playerHand[i]);
                    gm.exhaustedDeck.Add(temp[0]);
                    gm.playerHand.RemoveAt(i);
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
            for (int i = 0; i < gm.playerHand.Count; i++)
            {
                if (nazwaObiektu.Equals(gm.playerHand[i].name))
                {
                    temp.Add(gm.playerHand[i]);
                    gm.discardDeck.Add(temp[0]);
                    gm.playerHand.RemoveAt(i);
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
                _enemies.AddRange(gm.enType);
                if (numOfTargets > _enemies.Count)
                {
                    numOfTargets = _enemies.Count;
                }
                // else
                // Debug.Log(enemies.Count);
                //podniesienie ataku ponad -90, przesuniencie atakku do poz srodka renki
                EnableIndicator();
                currentCardState = cardState.Targetable;
                gm.canEndTurn = false;
                pos.anchoredPosition = new Vector3(0, -400, 0);
            }
            if (fm.en != null)
            {
                if (Input.GetButtonUp("Fire1") && fm.en.targeted == false && numOfTargets > 0)
                {
                    //wybor targetu, gdy liczba 
                    //dodatkowy warunek od enemies.count tak jak w lini 215
                    if (_enemies.Count >= 3)
                    {
                        if (numOfTargets == baseNumOfTargets)
                            fm.en.isFirstTarget = true;

                        else if (numOfTargets == baseNumOfTargets - 1)
                            fm.en.isSecondTarget = true;
                        else if (numOfTargets == baseNumOfTargets - 2)
                            fm.en.isThirdTarget = true;
                    }
                    
                    else if (baseNumOfTargets <=3  & _enemies.Count == 1)
                        fm.en.isFirstTarget = true;
                    else

                    {
                        //warunek dla 3 przeciwnikow
                        if (_enemies.Count ==3)
                        {
                            if (numOfTargets == baseNumOfTargets - 1)
                                fm.en.isFirstTarget = true;
                            else if (numOfTargets == baseNumOfTargets - 2)
                                fm.en.isSecondTarget = true;
                        }
                        else
                        {
                            if (numOfTargets == baseNumOfTargets)
                                fm.en.isFirstTarget = true;
                            if (numOfTargets == baseNumOfTargets - 1)
                                fm.en.isSecondTarget = true;
                        }
                    }
                    
                    fm.en.targeted = true;
                    numOfTargets -= 1;
                    if (numOfTargets == 0)
                        DisableIndicator();
                    //clickDelay = Time.time + 0.3f;
                }
                else if (Input.GetButtonUp("Fire1") && fm.en.targeted == true && numOfTargets >= 0)
                {
                    //cofnienice zaznaczenia
                    if (numOfTargets == baseNumOfTargets - 3 && fm.en.isThirdTarget == true)
                    {
                        fm.en.targeted = false;
                        fm.en.isThirdTarget = false;
                        numOfTargets += 1;
                    }
                    else if (numOfTargets == baseNumOfTargets - 2 && fm.en.isSecondTarget == true)
                    {
                        fm.en.targeted = false;
                        fm.en.isSecondTarget = false;
                        numOfTargets += 1;
                    }
                    else if (numOfTargets == baseNumOfTargets - 1 && fm.en.isFirstTarget == true)
                    {
                        fm.en.targeted = false;
                        fm.en.isFirstTarget = false;
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
                gm.canEndTurn = true;
            }
            //cofnienicie karty do reki reset indeksu reset targetowania
            if (Input.GetButton("Fire2") && currentCardState == cardState.Targetable)
            {
                ReturnToHand();
                DisableIndicator();
                cAlign.SetValues();
                gm.canEndTurn = true;
                //move to discard pile || exhaust


            }
        }
        else if (cType == cardType.Power || cType == cardType.Skill)
        {
            if (Input.GetButtonUp("Fire1") && pos.anchoredPosition.y > -90)
            {
                gm.canEndTurn = true;
                _enemies.Clear();
                _enemies.AddRange(gm.enType);
                OnDrop();
            }


        }

        if (fm.viewPortPosition.x < 0 || fm.viewPortPosition.x > 1 || fm.viewPortPosition.y < 0 || fm.viewPortPosition.y > 1)
        {
            gm.canEndTurn = true;
            ReturnToHand();
            DisableIndicator();
            cAlign.SetValues();
        }





    }
}