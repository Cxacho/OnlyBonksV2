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
    public Player pl;
    Quaternion oldRot, newRot, hoverRotation = new Quaternion(0, 0, 0, 0);
    public List<Enemy> enemies = new List<Enemy>();
    CardAlign cAlign;
    public bool playable;
    public GameplayManager gm;
    RectTransform pos;
    [SerializeField] GameObject par;
    List<GameObject> temp = new List<GameObject>();
    [SerializeField] int index, numOfTargets;
    public int baseNumOfTargets;
    float clickDelay;
    [SerializeField] List<GameObject> meshes = new List<GameObject>();
    float posY;



    private void Start()
    {
        currentCardState = cardState.Elsewhere;
    }
    IEnumerator Return()
    {
        yield return new WaitForSeconds(0.05f);
        if (fm.crd == null)
            for (int i = 0; i < cAlign.gameObject.transform.childCount; i++)
            {
                cAlign.children[i].transform.DOMove(cAlign.positions[i], 0.1f);
            }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.transform.IsChildOf(GameObject.Find("PlayerHand").transform) && gm.playerHand.Count == par.transform.childCount)
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

        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (this.transform.IsChildOf(GameObject.Find("PlayerHand").transform) && gm.playerHand.Count == par.transform.childCount)
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
        playable = true;
        meshes.AddRange(GameObject.FindGameObjectsWithTag("Indicator"));
        pl = GameObject.Find("Player").GetComponent<Player>();
        //kod do wymiany
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
    void ReturnToHand()
    {
        this.transform.localScale = Vector3.one;
        currentCardState = cardState.InHand;
        transform.SetParent(GameObject.Find("PlayerHand").transform);
        numOfTargets = baseNumOfTargets;
        foreach (Enemy en in enemies)
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
        foreach (GameObject obj in meshes)
        {
            obj.GetComponent<SpriteRenderer>().enabled = false;
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
                numOfTargets = baseNumOfTargets;
                if (numOfTargets > enemies.Count)
                {
                    numOfTargets = enemies.Count;
                }
                // else
                // Debug.Log(enemies.Count);
                //podniesienie ataku ponad -90, przesuniencie atakku do poz srodka renki

                foreach (GameObject obj in meshes)
                {
                    obj.GetComponent<SpriteRenderer>().enabled = true;
                }
                currentCardState = cardState.Targetable;
                var phPos = GameObject.Find("PlayerHand").transform.position;
                pos.anchoredPosition = new Vector3(0, -400, 0);
            }
            if (fm.en != null)
            {
                if (Input.GetButtonUp("Fire1") && fm.en.targeted == false && numOfTargets > 0)
                {
                    //wybor targetu, gdy liczba 
                    //dodatkowy warunek od enemies.count tak jak w lini 215
                    if (numOfTargets == baseNumOfTargets)
                        fm.en.isFirstTarget = true;
                    else if (numOfTargets == baseNumOfTargets - 1)
                        fm.en.isSecondTarget = true;
                    else if (numOfTargets == baseNumOfTargets - 2)
                        fm.en.isThirdTarget = true;

                    fm.en.targeted = true;
                    numOfTargets -= 1;
                    if (numOfTargets == 0)
                        DisableIndicator();
                    clickDelay = Time.time + 0.3f;
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
                        foreach (GameObject obj in meshes)
                        {
                            obj.GetComponent<SpriteRenderer>().enabled = true;
                        }
                    clickDelay = Time.time + 0.3f;
                }

            }
            if (Input.GetButtonUp("Fire1") && currentCardState == cardState.Targetable && numOfTargets == 0 && Time.time > clickDelay && fm.en == null)
            {
                //zagranie ataku gdy liczba targetow rowna zero i nie ma targetu na myszcze
                DisableIndicator();
                OnDrop();
                //play card
                //trail.enabled = true;
                //anim

            }
            //cofnienicie karty do reki reset indeksu reset targetowania
            if (Input.GetButton("Fire2") && currentCardState == cardState.Targetable)
            {
                ReturnToHand();
                DisableIndicator();
                //move to discard pile || exhaust


            }
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
            /*
            //cofnienice wybraniej karty gdy opusci viewport
            currentCardState = cardState.InHand;
            this.transform.SetParent(GameObject.Find("PlayerHand").transform);
            pos.anchoredPosition = posInHand;
            this.transform.rotation = oldRot;
            this.transform.SetSiblingIndex(index);
            foreach (Enemy en in enemies)
                en.targeted = false;
            */
            //ReturnToHand();
            //DisableIndicator();
        }





    }
}