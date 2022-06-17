using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Armor : MonoBehaviour
{
    RectTransform rect;
    [SerializeField]ArmorType _armorType;
    [SerializeField]WhereAmI currentOccupation;
    public List<GameObject> cards;
    FollowMouse fm;
    bool dragged;
    Vector2 originalPosition;
    [SerializeField]List<GameObject> eqSlots = new List<GameObject>();
    [SerializeField] List<float> eqSlotsPositions = new List<float>();
    [SerializeField] List<GameObject> Squares = new List<GameObject>();
    [SerializeField]List<inventorySpace> invSpace = new List<inventorySpace>();
    GameObject currentSlot;
    SpriteRenderer sr;
    [SerializeField] GameObject inventoryPanel;
    Vector3 objCentre,_objectCentre;
    int useMe;
    [SerializeField] int spaceUsage;
    Transform canvasTransform;
    string getSlotName;
    [SerializeField]GameObject shade;

    private void Awake()
    {
        //trzeba otworzyc panel z eq zanim wywola sie awake
        var getShadeSR = shade.GetComponent<SpriteRenderer>();
        
        sr = GetComponent<SpriteRenderer>();
        getShadeSR.sprite = sr.sprite;
        getShadeSR.color = Color.cyan;
        canvasTransform = FindObjectOfType<Canvas>().transform;
        originalPosition = this.transform.position;
        fm = GameObject.Find("Cursor").GetComponent<FollowMouse>();
        rect = GetComponent<RectTransform>();
        switch(_armorType)
        {
            case Armor.ArmorType.head:
                getSlotName = "HeadItemSlot";
                break;
            case Armor.ArmorType.chest:
                getSlotName = "ChestItemSlot";
                break;
            case Armor.ArmorType.legs:
                getSlotName = "LegsItemSlot";
                break;
            case Armor.ArmorType.boots:
                getSlotName = "BootsItemSlot";
                break;
        }
    }

    private void Update()
    {
        if (dragged == false) return;
        rect.anchoredPosition = fm.rectPos.anchoredPosition;
        GetClosest();
        // Debug.Log(currentSlot.GetType());
        //Debug.Log(GetType());
        //if this.anchoredpos  poza recttransform.parent gdy whereami => popup do u want to discard this item??/

    }
    private float GetClosest()
    {
        for(int i =0; i<eqSlotsPositions.Count;i++)
        {
            eqSlotsPositions[i] = Vector3.Distance(this.transform.position, eqSlots[i].transform.position);
            if(eqSlotsPositions[i] == eqSlotsPositions.Min())
            useMe = i;
        }
        currentSlot = eqSlots[useMe];
        return eqSlotsPositions.Min();
    }
    private void OnMouseDown()
    {
        dragged = true;
        if(currentOccupation == WhereAmI.inInventory)
        foreach (inventorySpace inv in invSpace)
            inv.occupied = false;
        currentOccupation = WhereAmI.elsewhere;
        objCentre = Vector3.zero;
        gameObject.transform.SetParent(canvasTransform);

    }
    private void OnMouseUp()
    {
        dragged = false;
        var checkDistance = Vector3.Distance(this.transform.position, currentSlot.transform.position);

        if (checkDistance < 4 && getSlotName == currentSlot.name)
        {
            this.transform.SetParent(currentSlot.transform);
            this.transform.position = currentSlot.transform.position;
            currentOccupation = WhereAmI.onPlayer;
            OnPickup();
        }
        else
        {
            this.transform.position = originalPosition;
            currentOccupation = WhereAmI.inInventory;
            this.transform.SetParent(inventoryPanel.transform);
            StartCoroutine(delay());
        }

        if (Squares.Count == spaceUsage)
        {
            foreach (inventorySpace inv in invSpace)
                if (inv.occupied == true)
                {
                    //gdy juz raz byl w inv, ma wrocic spowrotem do swojego miejsca i ustawic odpowiednie squary 
                    this.transform.position = originalPosition;
                    currentOccupation = WhereAmI.inInventory;
                    this.transform.SetParent(inventoryPanel.transform);
                    StartCoroutine(delay());
                    Debug.Log("you are placing item on occupied spot");
                    return;
                }
            currentOccupation = WhereAmI.inInventory;
            //if ()
            for (int i = 0; i < Squares.Count; i++)
            {
                objCentre = new Vector3(objCentre.x + Squares[i].transform.position.x, objCentre.y + Squares[i].transform.position.y, objCentre.z + Squares[i].transform.position.z);
            }
            _objectCentre = new Vector3 (objCentre.x / Squares.Count,objCentre.y/Squares.Count,objCentre.z/Squares.Count);
            this.transform.SetParent(inventoryPanel.transform);
            this.transform.position = _objectCentre;
            originalPosition = _objectCentre;
            foreach (inventorySpace inv in invSpace)
                inv.occupied = true;
            //else
              //  Debug.Log("you are placing item on occupied spot");
        }
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(0.05f);
        foreach (inventorySpace sqr in invSpace)
            sqr.occupied = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Squares.Add(collision.gameObject);
        invSpace.Add(collision.gameObject.GetComponent<inventorySpace>());
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Squares.Remove(collision.gameObject);
        invSpace.Remove(collision.gameObject.GetComponent<inventorySpace>());

    }





    void OnPickup()
    {
        //if previous armor piece !=null
        {
            RemoveStats();
            RemoveCards();
        }
        AddStats();
        AddCards();
    }
    void RemoveStats()
    {

    }
    void RemoveCards()
    {

    }
    void AddCards()
    {

    }
    void AddStats()
    {

    }
    enum ArmorType
    {
        head,
            chest,
            legs,
            boots
    }
    enum WhereAmI
    {
        elsewhere,
        inInventory,
        onPlayer


    }

}
