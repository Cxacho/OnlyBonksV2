using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler
{

    [SerializeField] private RectTransform dragTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] Transform playersHand;
    [SerializeField] private GameObject canvasAttach,previousCard,nextCard;
    [SerializeField] public GameplayManager gm;
    [SerializeField] private GameObject gmAttach;
    [SerializeField] private GameObject phAttach;
    [SerializeField] private CardAlign ph;
    [SerializeField] public Player pl;
    [SerializeField] public Enemy enemy;
    [SerializeField] public Animator animator;
    [SerializeField] public GameObject discarddeck;
    Draggable getPrvsScript, getNextScript;
    public TrailRenderer trail;
    public bool moveRight,moveLeft;
    int previous, next, thisOne;
    [SerializeField] private float singleStep;
    public GameObject enemyGO;


    Quaternion oldRot;
    Transform parentToReturn = null;
    GameObject placeholder = null;
    private GameObject go;
    public List<GameObject> temp = new List<GameObject>();
    private string nazwaObiektu;


    void Start()
    {

        singleStep = 0.3f;
        // animator.SetTrigger("CardPlayed");
    }
    void Update()
    {
        //moveCard();
        
    }

   

    public virtual void OnDrop()
    {
        /*
        for (int i = 0; i < ph.children.Count; i++)
        {
            Debug.Log(ph.children[i].transform.position);
            ph.children[i].transform.position = ph.positions[i];
        }
        */
        trail.enabled = true;
        go = this.gameObject;
        nazwaObiektu = go.name.Remove(go.name.Length - 7);
        Debug.Log(nazwaObiektu);
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

    }

    void moveCard()
    {
        
        if (moveLeft == true)
        {
            if (ph.children[thisOne].position.x > ph.positions[thisOne].x - 2.5f)
            {
                //Debug.Log(ph.positions[thisOne].x);
                //Debug.Log(ph.positions[thisOne].x - 2.5f);
                ph.children[thisOne].position += new Vector3(-singleStep * 1.5f, 0f, 0f);
            }
        }
        

        if (moveRight == true)
        {
            //ph.children[thisOne].position = Vector3.MoveTowards(ph.children[thisOne].position, new Vector3(ph.positions[thisOne].x, ph.positions[thisOne].y + 9, ph.positions[thisOne].z), singleStep * 3);

            /*
            if (moveRight && this.gameObject.transform.GetSiblingIndex() != ph.children.Count - 1)
            {
            if (ph.children[thisOne].position.x < ph.positions[thisOne].x + 2.5f)
                ph.children[thisOne].position += new Vector3(singleStep * 1.5f, 0f, 0f);
            }
            */
            if (ph.children[thisOne].position.x < ph.positions[thisOne].x + 2.5f)
            {
                //Debug.Log(ph.positions[thisOne].x);
                //Debug.Log(ph.positions[thisOne].x - 2.5f);
                ph.children[thisOne].position += new Vector3(singleStep * 1.5f, 0f, 0f);
            }
        }



        //return

        if (moveLeft == false && moveRight == false)
        {
            if (ph.children[thisOne].position.x <= ph.positions[thisOne].x)
                ph.children[thisOne].position += new Vector3(singleStep * 1.5f, 0f, 0f);

        }

        if (moveLeft == false && moveRight == false)
        {
            if (ph.children[thisOne].position.x >= ph.positions[thisOne].x)
                ph.children[thisOne].position += new Vector3(-singleStep * 1.5f, 0, 0);

        }
    }


    void Awake()
    {
        
        moveLeft = false;
        moveRight = false;
        trail = this.GetComponent<TrailRenderer>();
        animator = this.GetComponent<Animator>();
        pl = GameObject.Find("Player").GetComponent<Player>();
        enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
        canvasAttach = GameObject.Find("Canvas");
        canvas = canvasAttach.GetComponent<Canvas>();
        gmAttach = GameObject.Find("GameplayManager");
        gm = gmAttach.GetComponent<GameplayManager>();
        phAttach = GameObject.Find("PlayerHand");
        ph = phAttach.GetComponent<CardAlign>();
        playersHand = phAttach.transform;
        discarddeck = GameObject.Find("DiscardDeckButton");
        enemyGO = GameObject.Find("EnemyImage");
    }

    public void CardDestroy()
    {
        Destroy(this.gameObject);
    }

    
    

    #region IBeginDragHandler implementation
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this.transform.IsChildOf(playersHand) && gm.canPlayCards)
        {
            
            this.transform.localRotation = Quaternion.identity;
            placeholder = new GameObject();
            placeholder.name = this.gameObject.name;
            placeholder.transform.SetParent(this.transform.parent);
            LayoutElement le = placeholder.AddComponent<LayoutElement>();
            le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
            le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
            le.flexibleHeight = 0;
            le.flexibleWidth = 0;
            this.
            placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
            
            parentToReturn = this.transform.parent;
            this.transform.SetParent(this.transform.parent.parent);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else
        {
            eventData.pointerDrag = null;
        }
    }
    #endregion

    #region IDragHandler implementation
    public void OnDrag(PointerEventData eventData)
    {
        
        dragTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        
    }
    #endregion

    #region IEndDragHandler implementation
    public void OnEndDrag(PointerEventData eventData)
    {
        
        Vector3 mousePos = Input.mousePosition;
            if (mousePos.y < 175)
            {
                this.transform.SetParent(parentToReturn);
                this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                Destroy(placeholder);
                
        }
        else
            {
            this.gameObject.transform.SetParent(GameObject.Find("CardAnimator").transform);
            OnDrop();
            next = gm.playerHand.Count - 1;
            transform.DOMove(new Vector3(discarddeck.transform.position.x, discarddeck.transform.position.y, 0), 1.5f);
            transform.DOScale(0.25f, 0.5f);
            transform.DORotate(new Vector3(0, 0, -150f), 1.5f);

            //dopasowaæ carddestroy do animacji
            
            Invoke("CardDestroy", 2.1f);
                Destroy(placeholder);
            
            

        }
        

    }
    #endregion


    #region IPointerEnterHandler implementation
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        previous = this.transform.GetSiblingIndex() - 1;
        next = this.transform.GetSiblingIndex() + 1;
        if (this.gameObject.transform.GetSiblingIndex() != 0)
        {
        previousCard = ph.transform.GetChild(previous).gameObject;
        getPrvsScript = previousCard.GetComponent<Draggable>();
        }
        if (this.gameObject.transform.GetSiblingIndex() != ph.children.Count - 1)
        {
            nextCard = ph.transform.GetChild(next).gameObject;
            getNextScript = nextCard.GetComponent<Draggable>();
        }
        
        //Debug.Log(ph.children[1].position);
        if (this.transform.IsChildOf(playersHand))
            {
                //this.transform.localScale += new Vector3(30f, 30f, 0f) * Time.deltaTime;
            }
            //Dodac vfx i sfx wejscia kursora na karte
            //Debug.Log(this.transform.GetSiblingIndex());
            thisOne = this.transform.GetSiblingIndex();

            oldRot = ph.children[thisOne].rotation;
            ph.children[thisOne].rotation = Quaternion.Euler(Vector3.zero);
            ph.children[thisOne].position += new Vector3(0, 9f, 0f);

        if (previous >= 0 && this.gameObject.transform.IsChildOf(playersHand))
            ph.children[previous].position += new Vector3(-1.5f, 0, 0);
            //getPrvsScript.moveLeft = true;
            //move = true;
       

        if (next != ph.children.Count)
            ph.children[next].position += new Vector3(1.5f, 0, 0);
            //getNextScript.moveRight = true;
        //move = true;
        




    }
    #endregion


    #region IPointerExitHandler implementation
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        
        //zrobic smooth ruch tych kart najprawdopodobniej w updejcie
        if (this.transform.IsChildOf(playersHand))
        {
            this.transform.localScale = Vector3.one;
        }
        thisOne = this.transform.GetSiblingIndex();
        previous = this.transform.GetSiblingIndex() - 1;
        next = this.transform.GetSiblingIndex() + 1;
        ph.children[thisOne].rotation = oldRot;
        ph.children[thisOne].position += new Vector3(0, -9f, 0f);

        if (previous >= 0 && this.gameObject.transform.IsChildOf(playersHand))
            ph.children[previous].position += new Vector3(+1.5f, 0, 0);
        //getPrvsScript.moveLeft = false;
        //getPrvsScript = null;


        if (next != ph.children.Count)
            ph.children[next].position += new Vector3(-1.5f, 0, 0);
        //getNextScript.moveRight = false;
        //getNextScript = null;

        GetComponent<CanvasGroup>().blocksRaycasts = true;
       // Dodac vfx i sfx wyjscia kursora na karte
        
    }
    #endregion


    

}
