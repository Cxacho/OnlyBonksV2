using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CardAlign : MonoBehaviour
{
    public GameplayManager gm;
    public float cardsInHand, moveCardUp, totalTwist, twistPerCard, startTwist, drawTime;
    public List<RectTransform> children = new List<RectTransform>();
    public List<Vector3> positions = new List<Vector3>();
    [SerializeField] List<Vector3> rotations = new List<Vector3>();
    [SerializeField] private TMP_Text discardDeck_text, drawDeck_text;
    float delay;
    [SerializeField] Transform pos1, pos2;
    PointerEventData ped;
    public int cardIndex;
    [SerializeField] float mnoznik, pierwszyWyraz, cardHeight;
    [SerializeField] private AnimationCurve anCurve;
    //public Transform empty;
    private float twistFirstCard, nTyWyraz, liczbaWyrazow, dist, place;
    public int pointerHandler;

    void Update()
    {
        OnHandAmountChange();
        /*
        if (gm.delay < Time.time && gm.delay != 0)
        {
            gm.state = BattleState.PLAYERTURN;
            gm.delay = 0;
        }
        */
    }
    private void Awake()
    {
       ped = new PointerEventData(EventSystem.current);
    }
    public void Realign()
    {
        
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (i + 1 < gameObject.transform.childCount)
                children[i].transform.DOMove(positions[i], 0.05f);
            else
            {
                children[i].transform.DOMove(positions[i], 0.1f).OnComplete(() =>
                {
                    if (cardIndex - 1 >= 0)
                        children[cardIndex - 1].DOMoveX(children[cardIndex - 1].position.x - 2.5f, 0.2f);
                    if (cardIndex + 1 < children.Count)
                        children[cardIndex + 1].DOMoveX(children[cardIndex + 1].position.x + 2.5f, 0.2f);
                    children[cardIndex].DOMove(new Vector3(children[cardIndex].position.x, -23, children[cardIndex].position.z), 0.05f);
                    if (cardIndex - 2 >= 0)
                        children[cardIndex - 2].DOMoveX(children[cardIndex - 2].position.x - 1.5f, 0.2f);
                    if (cardIndex + 2 < children.Count)
                        children[cardIndex + 2].DOMoveX(children[cardIndex + 2].position.x + 1.5f, 0.2f);
                });
            }
                
        }
    }
    void Align()
    {
        foreach (RectTransform child in transform)
        {
            child.transform.rotation = Quaternion.identity;
        }

        cardsInHand = gm.playerHand.Count;
        twistPerCard = totalTwist / cardsInHand;
        startTwist = -1f * (totalTwist / 2f);
        float twistForThisCard = startTwist + twistPerCard;
        Quaternion rotZero = new Quaternion();
        rotZero.eulerAngles = Vector3.zero;
        /*
        foreach (RectTransform child in transform)
        {
            children.Add(child);
            child.transform.rotation = rotZero;
        }
        */
        //foreach (RectTransform pog in children)
        //height.Add(pog);
        switch (cardsInHand)
        {
            case 1:
                twistFirstCard = 0;
                break;
            case 2:
                twistFirstCard = 10f;
                break;
            case 3:
                twistFirstCard = 13.3f;

                break;
            case 4:
                twistFirstCard = 15f;
                break;
            case 5:
                twistFirstCard = 16f;

                break;
            case 6:
                twistFirstCard = 16.6f;

                break;
            case 7:
                twistFirstCard = 17.1f;

                break;
            case 8:
                twistFirstCard = 17.5f;

                break;
            case 9:
                twistFirstCard = 17.7f;

                break;
            case 10:
                twistFirstCard = 18;

                break;
        }

        children[0].transform.Rotate(0f, 0f, twistFirstCard);
        Quaternion rotForFirst = new Quaternion();
        rotForFirst = children[0].transform.rotation;
        var rot = rotForFirst.eulerAngles.z;
        rotations[0] = rotForFirst.eulerAngles;
        for (int i = 1; i < gm.playerHand.Count; i++)
        {
            //children[i].transform.Rotate(0f, 0f, rot + (twistPerCard * (-i)));
            rotations[i] = new Vector3(0f, 0f, rot + (twistPerCard * (-i)));
        }
    }

    void FitCards()
    {
        liczbaWyrazow = gm.playerHand.Count - 1;
        nTyWyraz = pierwszyWyraz * Mathf.Pow(mnoznik, liczbaWyrazow - 1);
        var multiplier = cardsInHand;
        if (cardsInHand == 10)
        {
            multiplier = 0;
        }
        if (cardsInHand == 3)
            multiplier = 4;
        nTyWyraz += multiplier;
        pos1.position = new Vector3(-nTyWyraz, pos1.position.y, 0);
        pos2.position = new Vector3(-pos1.position.x, pos2.position.y, 0);
        var leftPoint = pos1.position;
        var rightPoint = pos2.position;
        var delta = (rightPoint - leftPoint).magnitude;
        var howMany = this.transform.childCount;
        var howManyGapsBetweenItems = howMany - 1;
        var theHighestIndex = howMany;
        var gapFromOneItemToTheNextOne = delta / howManyGapsBetweenItems;
        dist = 1.0f / (gm.playerHand.Count - 1);


        for (int i = 0; i < theHighestIndex; i++)
        {
            positions.Add(leftPoint);
            //children[i].transform.position = leftPoint;
            if (theHighestIndex > 1)
                positions[i] += new Vector3((i * gapFromOneItemToTheNextOne), cardHeight, children[i].transform.position.z);
            //children[i].transform.position += new Vector3((i * gapFromOneItemToTheNextOne), cardHeight, children[i].transform.position.z);
            //
            if (i == 0)
            {
                place = anCurve.Evaluate(0);
                positions[i] = new Vector3(positions[i].x, place + cardHeight, positions[i].z);

            }
            else
            {
                place = anCurve.Evaluate(i * dist);
                positions[i] = new Vector3(positions[i].x, place + cardHeight, positions[i].z);
            }
        }
        if (theHighestIndex <= 1)
        {
            positions.Add(new Vector3(0, pos1.position.y + cardHeight, 0));
            //children[0].transform.position = new Vector3(0, pos1.position.y + cardHeight, children[0].transform.position.z);
        }

    }
    
    void OnHandAmountChange()
    {

        if (cardsInHand != gm.playerHand.Count)
        {
            if (gm.playerHand.Count != 0)
            {
                SetValues();
            }
            Invoke("ValueUpdate", 0.02f);
        }
        cardsInHand = gm.playerHand.Count;
    }

    void ValueUpdate()
    {
        string dek = gm.drawDeck.Count.ToString();
        string dis = gm.discardDeck.Count.ToString();
        discardDeck_text.SetText(dis);
        drawDeck_text.SetText(dek);
    }
    public void SetValues()
    {

        positions.Clear();
        children.Clear();
        rotations.Clear();
        foreach (RectTransform child in transform)
        {
            children.Add(child);
            rotations.Add(child.rotation.eulerAngles);
        }
        FitCards();
        Align();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            children[i].transform.DORotate(rotations[i], 0.2f);
            children[i].transform.DOMove(positions[i], 0.2f);
        }
    }
    IEnumerator doDelay()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.transform.GetChild(pointerHandler).GetComponent<Card>().OnPointerEnter(ped);
        gameObject.transform.GetChild(pointerHandler).transform.rotation = Quaternion.identity;
    }

    public void Animate()
    {

        SetValues();
        GameObject card = children[children.Count-1].gameObject;
        card.transform.DOScale(Vector3.one, 0.2f).OnComplete(() =>
        {
            card.transform.DOMove(positions[card.transform.GetSiblingIndex()], 0.2f).OnComplete(() =>
            {

                for (int i = 0; i < this.transform.childCount; i++)
                {
                    children[i].transform.DORotate(rotations[i], 0.2f);
                    children[i].transform.DOMove(positions[i], 0.2f);
                }
                //wywolanie draw'u kolejnych kart
                if (gm.drawAmount == gm.playerDrawAmount)
                {
                    gm.state = BattleState.PLAYERTURN;

                    gm.drawAmount=0;
                    
                    if (EventSystem.current.IsPointerOverGameObject()&& pointerHandler < 11)
                    {
                        StartCoroutine(doDelay());
                    }
                    
                }
                else
                {
                    if (gm.playerHand.Count < 10)
                        gm.DrawCards(gm.playerDrawAmount);
                    else
                    {
                        gm.state = BattleState.PLAYERTURN;
                        gm.drawAmount = 0;
                        Debug.Log("Can't draw any further, u have already drawn" + " " + gm.playerHand.Count);
                    }
                }
 
            });
        });

    }

}