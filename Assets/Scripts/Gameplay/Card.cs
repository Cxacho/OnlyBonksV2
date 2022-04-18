using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;


public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public cardType cType;
    public cardState currentCardState;
    [SerializeField] Vector3 mousePos, posInHand, discDek;
    [SerializeField] FollowMouse fm;
    [SerializeField] TrailRenderer trail;
    [SerializeField] GameObject bonkAnim;
    public Player pl;
    Quaternion oldRot, newRot = new Quaternion(0, 0, 0, 0);
    public List<Enemy> enemies = new List<Enemy>();
    Animator animator;
    public GameplayManager gm;
    RectTransform pos;
    List<GameObject> temp = new List<GameObject>();
    [SerializeField] int index,numOfTargets;
    public int baseNumOfTargets;
    float clickDelay;


    private void Start()
    {
        numOfTargets = baseNumOfTargets;
           currentCardState = cardState.Elsewhere;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.transform.IsChildOf(GameObject.Find("PlayerHand").transform))
        {
            currentCardState = cardState.OnMouse;
            index = this.transform.GetSiblingIndex();
            this.transform.SetAsLastSibling();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (this.transform.IsChildOf(GameObject.Find("PlayerHand").transform))
        {
            this.transform.SetSiblingIndex(index);
            currentCardState = cardState.InHand;
        }
    }
    void Awake()
    {
        pl = GameObject.Find("Player").GetComponent<Player>();
        var find = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy en in find)
            enemies.Add(en);
        discDek = GameObject.Find("DiscardDeckButton").transform.position;
        trail = transform.GetComponent<TrailRenderer>();
        gm = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        pos = this.transform.GetComponent<RectTransform>();
        fm = GameObject.Find("Cursor").GetComponent<FollowMouse>();
    }
    void Update()
    {
        StateCheck();
        //mousePos = ui.mousePosition;  
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
    public enum cardState
    {
        InHand = 0,
        OnMouse = 1,
        OnCursor = 2,
        Elsewhere = 3,
        Targetable = 4
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
            currentCardState = cardState.InHand;
            this.transform.SetParent(GameObject.Find("PlayerHand").transform);
            pos.anchoredPosition = posInHand;
            this.transform.rotation = oldRot;
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
    public void DropControl()
    {
        if (cType == cardType.Attack)
        {
            if (pos.anchoredPosition.y > -90)
            {
                currentCardState = cardState.Targetable;
                var phPos = GameObject.Find("PlayerHand").transform.position;
                pos.anchoredPosition = new Vector3(0, -400, 0);
            }
            if (fm.en != null)
            {
                if (Input.GetButtonUp("Fire1") && fm.en.targeted == false && numOfTargets > 0)
                {

                    fm.en.targeted = true;
                    numOfTargets -= 1;
                    clickDelay = Time.time + 0.3f;
                }
                else if(Input.GetButtonUp("Fire1") && fm.en.targeted == true && numOfTargets>0)
                {
                    fm.en.targeted = false;
                    numOfTargets += 1;
                    clickDelay = Time.time + 0.3f;
                }
            }

            if (Input.GetButtonUp("Fire1") && currentCardState == cardState.Targetable && numOfTargets == 0 && Time.time >clickDelay&& fm.en ==null)
            {
                OnDrop();
                //play card
                //trail.enabled = true;
                //anim

                    }
            //cofnienicie karty do reki reset indeksu reset targetowania
                    if (Input.GetButton("Fire2") && currentCardState == cardState.Targetable)
                    {
                        currentCardState = cardState.InHand;
                        this.transform.SetParent(GameObject.Find("PlayerHand").transform);
                        numOfTargets = baseNumOfTargets;
                        foreach (Enemy en in enemies)
                            en.targeted = false;
                        pos.anchoredPosition = posInHand;
                        this.transform.rotation = oldRot;
                        this.transform.SetSiblingIndex(index);
                    }
                    //move to discard pile || exhaust
                
            
        }

        else if (cType == cardType.Power || cType == cardType.Skill)
        {
            if (Input.GetButtonUp("Fire1") && pos.anchoredPosition.y > -90)
            {
                OnDrop();
            }


        }
        if (fm.viewPortPosition.x < 0 || fm.viewPortPosition.x > 1 || fm.viewPortPosition.y < 0 || fm.viewPortPosition.y > 1)
        {
            currentCardState = cardState.InHand;
            this.transform.SetParent(GameObject.Find("PlayerHand").transform);
            pos.anchoredPosition = posInHand;
            this.transform.rotation = oldRot;
            this.transform.SetSiblingIndex(index);
            foreach (Enemy en in enemies)
                en.targeted = false;
        }
    }


    }

