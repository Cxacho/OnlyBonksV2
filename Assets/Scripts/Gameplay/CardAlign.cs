using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CardAlign : MonoBehaviour
{
    public GameplayManager gm;
    public float cardsInHand, moveCardUp, totalTwist, twistPerCard, startTwist;
    public List<RectTransform> children = new List<RectTransform>();
    public List<Transform> faken = new List<Transform>();
    //List <RectTransform> height = new List<RectTransform>();
    //public List<Vector3> positions = new List<Vector3>();
    [SerializeField] private TMP_Text discardDeck_text, drawDeck_text;
    [SerializeField] Transform pos1, pos2;
    [SerializeField] float mnoznik, pierwszyWyraz, cardHeight;
    [SerializeField] private AnimationCurve anCurve;
    //public Transform empty;
    private float twistFirstCard, nTyWyraz, liczbaWyrazow,dist,place;
    public int index;


    void Update()
    {
        OnHandAmountChange();
        // Animate();
    }
    
    void Align()
    {
        children.Clear();
        cardsInHand = gm.playerHand.Count;
        twistPerCard = totalTwist / cardsInHand;
        startTwist = -1f * (totalTwist / 2f);
        float twistForThisCard = startTwist + twistPerCard;
        Quaternion rotZero = new Quaternion();
        rotZero.eulerAngles = Vector3.zero;
        foreach (RectTransform child in transform)
        {
            children.Add(child);
            child.transform.rotation = rotZero;
        }
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

            for (int i = 1; i < this.transform.childCount; i++)
            {
                children[i].transform.Rotate(0f, 0f, rot + (twistPerCard * (-i)));
            }
    }
    
    void FitCards()
    {
        //
       // foreach (Transform child in empty.transform)
         //   faken.Add(child);

        //
        liczbaWyrazow = gm.playerHand.Count - 1;
        nTyWyraz = pierwszyWyraz * Mathf.Pow(mnoznik, liczbaWyrazow - 1);
        var multiplier = cardsInHand;
        if(cardsInHand==10)
        {
            multiplier = 0;
        }
        if (cardsInHand == 3)
            multiplier = 4; 
        nTyWyraz += multiplier;
        pos1.position = new Vector3(-nTyWyraz, pos1.position.y, 0);
        pos2.position = new Vector3(-pos1.position.x, pos2.position.y,0);
        var leftPoint = pos1.position;
        var rightPoint = pos2.position;
        var delta = (rightPoint - leftPoint).magnitude;
        var howMany = this.transform.childCount;
        var howManyGapsBetweenItems = howMany-1;
        var theHighestIndex = howMany;
        var gapFromOneItemToTheNextOne = delta / howManyGapsBetweenItems;
        dist = 1.0f / (cardsInHand - 1);

        for (int i = 0; i < theHighestIndex; i++)
        {
    
            children[i].transform.position = leftPoint;
            if(theHighestIndex>1)
            children[i].transform.position += new Vector3((i * gapFromOneItemToTheNextOne), cardHeight, children[i].transform.position.z);
            //
            if (i == 0)
            {
                place = anCurve.Evaluate(0);
                children[i].anchoredPosition = new Vector3(children[i].anchoredPosition.x, place, children[i].transform.position.z);
            }
            else
            {
                place = anCurve.Evaluate(i * dist);
                children[i].anchoredPosition = new Vector3(children[i].anchoredPosition.x, place, children[i].transform.position.z);
            }
        }
        if (theHighestIndex <= 1)
        {
            children[0].transform.position = new Vector3(0, pos1.position.y + cardHeight, children[0].transform.position.z);
        }

    }
    void OnHandAmountChange()
    {
        if (cardsInHand != gm.playerHand.Count)
        {
            if (gm.playerHand.Count != 0)
            {
                Invoke("Align", 0.02f);
                //Invoke("GetValues", 0.02f);
                Invoke("FitCards", 0.02f);
            }
            Invoke("ValueUpdate", 0.02f);
        }
        cardsInHand = gm.playerHand.Count;
    }
    /*
    void BedzieLepiej()
    {
        var n = gm.playerHand.Count;
        for (var i = n - 1; i >= 0; i--)
        {
            var y = 1.5f - i;
             
            var x = cardParent.transform.GetChild(i).gameObject;
            x.transform.Rotate(0f,0f,y * 10f);
            x.transform.Translate(0,  40f, 0);
            
        }
    }
    */
    void ValueUpdate()
    {
        string dek = gm.drawDeck.Count.ToString();
        string dis = gm.discardDeck.Count.ToString();
        discardDeck_text.SetText(dis);
        drawDeck_text.SetText(dek);
    }
    void Animate()
    {

        if(Input.GetKey("l"))
        {
            faken[index].transform.DOMove(this.transform.position, 0.5f);
        }
    }
}
