using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardAlign : MonoBehaviour
{
    public GameplayManager gm;
    public float cardsInHand,moveCardUp,totalTwist,twistPerCard,startTwist;
    public List <Transform> children = new List<Transform>();
    public List <Vector3> positions = new List<Vector3>();
    [SerializeField] private TMP_Text discardDeck_text, drawDeck_text; 

    float twistFirstCard;

    public GameObject cardParent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OnHandAmountChange();
    }
    #region jestkurewskozlenapisane Align
    void Align()
    {
        
        //nic nie dzialalo to zbruteforcowalem to
        children.Clear();
        cardsInHand = gm.playerHand.Count;
        twistPerCard = totalTwist / cardsInHand;
        startTwist = -1f * (totalTwist / 2f);
        float twistForThisCard = startTwist + twistPerCard;
        Quaternion rotZero = new Quaternion();
        rotZero.eulerAngles = Vector3.zero;
        foreach (Transform child in transform)
        {
            children.Add(child);
            child.transform.rotation = rotZero;
            //transform.rotation zmienac a nie .rotate
        }
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
                children[1].transform.Translate(0, 2* moveCardUp, 0);
                

                
                break;
        case 4:
                twistFirstCard = 15f;
                children[0].transform.Translate(0, -2f * moveCardUp, 0);
                children[1].transform.Translate(0, 2 * moveCardUp, 0);
                children[2].transform.Translate(0, 2 * moveCardUp, 0);
                children[3].transform.Translate(0, -2f * moveCardUp, 0);
                

                
                break;
        case 5:
                twistFirstCard = 16f;
               // children[0].transform.Translate(0, -2f , 0);
                children[1].transform.Translate(0, 3, 0);
                children[2].transform.Translate(0, 7f, 0);
                children[3].transform.Translate(0, 3 , 0);
                //children[4].transform.Translate(0, -2f, 0);
                
                break;
        case 6:
                twistFirstCard = 16.6f;
                //children[0].transform.Translate(0, -2f * moveCardUp, 0);
                children[1].transform.Translate(0, 2* moveCardUp, 0);
                children[2].transform.Translate(0, 4* moveCardUp, 0);
                children[3].transform.Translate(0, 4 * moveCardUp, 0);
                children[4].transform.Translate(0, 2 * moveCardUp, 0);
                //children[5].transform.Translate(0, -2f * moveCardUp, 0);

                break;
        case 7:
                twistFirstCard = 17.1f;
                //children[0].transform.Translate(0, -2f * moveCardUp, 0);
                children[1].transform.Translate(0, 2 * moveCardUp, 0);
                children[2].transform.Translate(0, 4* moveCardUp, 0);
                children[3].transform.Translate(0, 5 * moveCardUp, 0);
                children[4].transform.Translate(0, 4 * moveCardUp, 0);
                children[5].transform.Translate(0, 2* moveCardUp, 0);
                //children[6].transform.Translate(0, -2f * moveCardUp, 0);
                break;
        case 8:
                twistFirstCard = 17.5f;
                //children[0].transform.Translate(0, -2f * moveCardUp, 0);
                children[1].transform.Translate(0, 2 * moveCardUp, 0);
                children[2].transform.Translate(0, 4.5f * moveCardUp, 0);
                children[3].transform.Translate(0, 6 * moveCardUp, 0);
                children[4].transform.Translate(0, 6 * moveCardUp, 0);
                children[5].transform.Translate(0, 4.5f * moveCardUp, 0);
                children[6].transform.Translate(0, 2 * moveCardUp, 0);
                //children[7].transform.Translate(0, -2f * moveCardUp, 0);
                break;
        case 9:
                twistFirstCard = 17.7f;
                //children[0].transform.Translate(0, -2f * moveCardUp, 0);
                children[1].transform.Translate(0, 2 * moveCardUp, 0);
                children[2].transform.Translate(0, 4.5f * moveCardUp, 0);
                children[3].transform.Translate(0, 6.5f* moveCardUp, 0);
                children[4].transform.Translate(0, 8 * moveCardUp, 0);
                children[5].transform.Translate(0, 6.5f* moveCardUp, 0);
                children[6].transform.Translate(0, 4.5f * moveCardUp, 0);
                children[7].transform.Translate(0, 2 * moveCardUp, 0);
                //children[8].transform.Translate(0, -2f * moveCardUp, 0);
                break;
        case 10:
                twistFirstCard = 18;
                children[0].transform.Translate(0, -2f * moveCardUp, 0);
                children[1].transform.Translate(0, 2* moveCardUp, 0);
                children[2].transform.Translate(0, 4.5f * moveCardUp, 0);
                children[3].transform.Translate(0, 6.5f * moveCardUp, 0);
                children[4].transform.Translate(0, 8 * moveCardUp, 0);
                children[5].transform.Translate(0, 8 * moveCardUp, 0);
                children[6].transform.Translate(0, 6.5f * moveCardUp, 0);
                children[7].transform.Translate(0, 4.5f * moveCardUp, 0);
                children[8].transform.Translate(0, 2 * moveCardUp, 0);
                children[9].transform.Translate(0, -2f * moveCardUp, 0);
                break;
        }

        children[0].transform.Rotate(0f, 0f, twistFirstCard);
        Quaternion rotForFirst = new Quaternion();
        rotForFirst = children[0].transform.rotation;
        var rot = rotForFirst.eulerAngles.z;

            for (int i = 1; i < cardsInHand; i++)
            {
                children[i].transform.Rotate(0f, 0f, rot + (twistPerCard * (-i)));
            }
            //JEBNac switchstatement od liczby kart bo sie wkurwilem ze to nie dziala
            //przy liczbiekart rownej 0 wypierdala index out of range; 
            //przy liczbie kart wiekszej niz 5 odpierdala sie dziwna rzecz,pierwsza karta zmienia swoja rotacje i pozycje
    }
#endregion
    void OnHandAmountChange()
    {
        if (cardsInHand != gm.playerHand.Count)
        {
            if (gm.playerHand.Count != 0)
                Invoke("Align", 0.02f);
            Invoke("ValueUpdate", 0.02f);
            
            if (gm.playerHand.Count !=0)
            Invoke("GetValues",0.02f);

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
    public void GetValues()
    {
        positions.Clear();
        //Debug.Log("")
        //positions.Add(children[0].position);
        //positions[0] = children[0].position;

        
        for (int i = 0; i < children.Count; i++)
        {
            positions.Add(children[i].position);
            

        }


    }
}
