using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Inventory : MonoBehaviour ,IPointerEnterHandler ,IPointerExitHandler
{
    RectTransform rect;
    [SerializeField]WhereAmI currentOccupation;
    public InventoryItem _inventoryItem;
    public List<GameObject> cards;
    private Player pl;
    FollowMouse fm;
    bool dragged;
    public UiActive ui;
    Vector2 originalPosition;
    List<GameObject> temporary = new List<GameObject>();
    [SerializeField] List<inventorySpace> allSpaces = new List<inventorySpace>();
    [SerializeField]List<GameObject> eqSlots = new List<GameObject>();
    [SerializeField] List<float> eqSlotsPositions = new List<float>();
    [SerializeField] List<GameObject> Squares = new List<GameObject>();
    [SerializeField]List<inventorySpace> invSpace = new List<inventorySpace>();
    GameObject currentSlot;
    SpriteRenderer sr;
    [SerializeField] GameObject inventoryPanel,eqPanel;//
    Vector3 objCentre,_objectCentre;
    private int useMe;
    [SerializeField] int spaceUsage;
    Transform canvasTransform;
    string getSlotName,getSecondSlot;
    GameplayManager gm;
    public GameplayManager.Weapon myWeaponType;

    private void Awake()
    {
        ui = FindObjectOfType<UiActive>();
        gameObject.name = ui.eqBackpackPanel.name;
        inventoryPanel = ui.eqBackpackPanel;
        eqPanel = ui.eqSlotsPanel;
        spaceUsage = _inventoryItem.spaceUsage;
        this.name = _inventoryItem.name;
        cards = _inventoryItem._cards;
        gm = GameObject.FindObjectOfType<GameplayManager>();
        pl = gm.player;
        //uimanager
        
        for (int i = 0; i < 90; i++)
        {
            allSpaces.Add(inventoryPanel.transform.GetChild(0).gameObject.transform.GetChild(i).GetComponent<inventorySpace>());
        }
        for (int i = 0; i < 6; i++)
        {
            eqSlots.Add(eqPanel.transform.GetChild(i).gameObject);
        }
        
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = _inventoryItem.sprite;
        canvasTransform = FindObjectOfType<Canvas>().transform;
        originalPosition = this.transform.position;
        fm = GameObject.Find("Cursor").GetComponent<FollowMouse>();
        rect = GetComponent<RectTransform>();
        switch(_inventoryItem.myItemType)
        {
            case InventoryItem.ItemType.Head:
                getSlotName = "HeadItemSlot";
                break;
            case InventoryItem.ItemType.Chest:
                getSlotName = "ChestItemSlot";
                break;
            case InventoryItem.ItemType.Legs:
                getSlotName = "LegsItemSlot";
                break;
            case InventoryItem.ItemType.Boots:
                getSlotName = "BootsItemSlot";
                break;
            case InventoryItem.ItemType.Weapon:
                getSlotName = "PrimaryWeapon";
                getSecondSlot = "SecondaryWeapon";
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
        //zrobic dzialanie ze mozna ruszac itemami tylko na mapie, i poza walka
        if (gm.state != BattleState.PLAYERTURN && gm.state != BattleState.ENEMYTURN)
            dragged = true;
        else
            return;

        if (currentOccupation==WhereAmI.onPlayer)
        {
            RemoveCards();
            RemoveStats();
            currentSlot.GetComponent<ItemSlots>().occupied = false;
            if (currentSlot.GetComponent<ItemSlots>().slot == ItemSlots.mySlotType.primaryWeapon)
                gm.primaryWeapon = GameplayManager.Weapon.Brak;
            else if (currentSlot.GetComponent<ItemSlots>().slot == ItemSlots.mySlotType.secondaryWeapon)
                gm.secondaryWeapon = GameplayManager.Weapon.Brak;
            //currentSlot = null; 
        }
        if (currentOccupation == WhereAmI.inInventory)
        {
            foreach (inventorySpace inv in invSpace)
                inv.occupied = false;
            gm.ItemsInInventory.Remove(this.gameObject);
        }
        currentOccupation = WhereAmI.elsewhere;
        objCentre = Vector3.zero;
        gameObject.transform.SetParent(canvasTransform);

    }
    private void OnMouseUp()
    {
        if (gm.state != BattleState.PLAYERTURN && gm.state != BattleState.ENEMYTURN)
            dragged = false;
        else
            return;
        var checkDistance = Vector3.Distance(this.transform.position, currentSlot.transform.position);
        var isOccupied = currentSlot.GetComponent<ItemSlots>().occupied;
        
        //wywoluje sie na zrzuceniu na odpowiedni slot
        if (checkDistance < 4  &&getSlotName == currentSlot.name || getSecondSlot== currentSlot.name)
        {
            if (isOccupied)
            {
                if (currentSlot.GetComponent<ItemSlots>().slot == ItemSlots.mySlotType.primaryWeapon)
                    gm.primaryWeapon = myWeaponType;
                else if (currentSlot.GetComponent<ItemSlots>().slot == ItemSlots.mySlotType.secondaryWeapon)
                    gm.secondaryWeapon = myWeaponType;
                Debug.Log("switch item slots");
                currentSlot.GetComponent<ItemSlots>().currentItem.GetComponent<Inventory>().RemoveStats();
                currentSlot.GetComponent<ItemSlots>().currentItem.GetComponent<Inventory>().RemoveCards();
                //gameplayManager.ItemsInInventory.Add(this.gameObject);
                moveToFirstOpenSpace(currentSlot.GetComponent<ItemSlots>().currentItem, currentSlot.GetComponent<ItemSlots>().currentItem.GetComponent<Inventory>().spaceUsage);
                currentSlot.GetComponent<ItemSlots>().currentItem = this.gameObject;
                this.transform.SetParent(currentSlot.transform);
                this.transform.position = currentSlot.transform.position;
                currentOccupation = WhereAmI.onPlayer;
                originalPosition = this.transform.position;
                currentSlot.GetComponent<ItemSlots>().occupied = true;
                OnPickup();
            }


            else
            {
                if (currentSlot.GetComponent<ItemSlots>().slot == ItemSlots.mySlotType.primaryWeapon)
                    gm.primaryWeapon = myWeaponType;
                else if (currentSlot.GetComponent<ItemSlots>().slot == ItemSlots.mySlotType.secondaryWeapon)
                    gm.secondaryWeapon = myWeaponType;
                //gameplayManager.ItemsInInventory.Add(this.gameObject);
                currentSlot.GetComponent<ItemSlots>().currentItem = this.gameObject;
                this.transform.SetParent(currentSlot.transform);
                this.transform.position = currentSlot.transform.position;
                currentOccupation = WhereAmI.onPlayer;
                originalPosition = this.transform.position;
                currentSlot.GetComponent<ItemSlots>().occupied = true;
                OnPickup();
            }
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
            if (invSpace.Count > 0)
                foreach (inventorySpace inv in invSpace)
                if (inv.occupied == true)
                {
                    //gdy juz raz byl w inv, ma wrocic spowrotem do swojego miejsca i ustawic odpowiednie squary 
                    this.transform.position = originalPosition;
                    currentOccupation = WhereAmI.inInventory;
                    this.transform.SetParent(inventoryPanel.transform);
                        gm.ItemsInInventory.Add(this.gameObject);
                    StartCoroutine(delay());
                    Debug.Log("you are placing item on occupied spot");
                    return;
                }
            currentOccupation = WhereAmI.inInventory;
            //zrucenie na kratki w inventory
            for (int i = 0; i < Squares.Count; i++)
            {
                objCentre = new Vector3(objCentre.x + Squares[i].transform.position.x, objCentre.y + Squares[i].transform.position.y, objCentre.z + Squares[i].transform.position.z);
            }
            _objectCentre = new Vector3 (objCentre.x / Squares.Count,objCentre.y/Squares.Count,objCentre.z/Squares.Count);
            this.transform.SetParent(inventoryPanel.transform);
            this.transform.position = _objectCentre;
            gm.ItemsInInventory.Add(this.gameObject);
            originalPosition = _objectCentre;
            if(invSpace.Count >0)
            foreach (inventorySpace inv in invSpace)
                inv.occupied = true;
        }
    }
    void moveToFirstOpenSpace(GameObject go,int spaceUsage)
    {
        switch(spaceUsage)
        {
            //warunek dla kolumny z prawej strony
            case 1:
                for (int i = 0; i < allSpaces.Count; i++)
                {

                    //wyjebie przy ostatnim warunki
                    if (allSpaces[i].occupied == true)
                        continue;
                    else
                    {
                        //var temp = List<>
                        var getGoScript = go.GetComponent<Inventory>();
                        go.transform.SetParent(inventoryPanel.transform);
                        go.transform.position = allSpaces[i].gameObject.transform.position;
                        getGoScript.originalPosition = go.transform.position;
                        go.GetComponent<Inventory>().currentOccupation = WhereAmI.inInventory;
                        StartCoroutine(go.GetComponent<Inventory>().delay());
                        return;
                         
                    }
                }
                break;
            case 2:
                var setCentre = new Vector3(0, 0, 0);
                for (int i = 0; i < allSpaces.Count - 1; i++)
                {
                    if (i % 9 ==8)
                    {
                        continue;
                    }
                    //wyjebie przy ostatnim warunki
                    if (allSpaces[i].occupied == true || allSpaces[i + 1].occupied == true)
                        continue;
                    else
                    {
                        setCentre = new Vector3(0.5f*(allSpaces[i].transform.position.x + allSpaces[i + 1].transform.position.x), 0.5f*(allSpaces[i].transform.position.y 
                            + allSpaces[i + 1].transform.position.y), 1);
                    }
                    go.transform.position = setCentre;
                    go.GetComponent<Inventory>().originalPosition = go.transform.position;
                    go.transform.SetParent(inventoryPanel.transform);
                    StartCoroutine(go.GetComponent<Inventory>().delay());
                    go.GetComponent<Inventory>().currentOccupation = WhereAmI.inInventory;
                    //ustawic original position, znalesc centrum
                    //go.transform.position = go.GetComponent<>;
                    //currentOccupation = WhereAmI.inInventory;
                    return;
                }
                break;
            case 4:
                var setCentrefor4 = new Vector3(0, 0, 0);
                for (int i = 0; i < allSpaces.Count - 9; i++)
                {
                    if (i % 9 == 8)
                    {
                        continue;
                    }
                    //wyjebie przy ostatnim warunki
                    if (allSpaces[i].occupied == true || allSpaces[i + 1].occupied == true || allSpaces[i + 9].occupied == true|| allSpaces[i + 10].occupied == true)
                        continue;
                    else
                    {
                        setCentrefor4 = new Vector3(0.25f * ((allSpaces[i].transform.position.x + allSpaces[i + 1].transform.position.x + allSpaces[i +9].transform.position.x
                            + allSpaces[i + 10].transform.position.x)-1), 0.25f * (allSpaces[i].transform.position.y + allSpaces[i + 1].transform.position.y + allSpaces[i+9].transform.position.y +
                            allSpaces[i + 10].transform.position.y), 1);
                    }
                    go.transform.position = setCentrefor4;
                    go.GetComponent<Inventory>().originalPosition = go.transform.position;
                    go.transform.SetParent(inventoryPanel.transform);
                    StartCoroutine(go.GetComponent<Inventory>().delay());
                    go.GetComponent<Inventory>().currentOccupation = WhereAmI.inInventory;
                    return;
                }
                break;
            case 6:
                var setCentrefor6 = new Vector3(0, 0, 0);
                for (int i = 0; i < allSpaces.Count - 18; i++)
                {
                    var calc = 1f / 6f;
                    if (i % 9 == 8)
                    {
                        continue;
                    }
                    //wyjebie przy ostatnim warunki
                    if (allSpaces[i].occupied == true || allSpaces[i + 1].occupied == true || allSpaces[i + 9].occupied == true || allSpaces[i + 10].occupied == true
                        || allSpaces[i + 18].occupied == true || allSpaces[i + 19].occupied == true)
                        continue;
                    else
                    {
                        var getGoScript = go.GetComponent<Inventory>();
                        setCentrefor6 = new Vector3(calc * ((allSpaces[i].transform.position.x + allSpaces[i + 1].transform.position.x + allSpaces[i + 9].transform.position.x
                            + allSpaces[i + 10].transform.position.x + allSpaces[i + 18].transform.position.x
                            + allSpaces[i + 19].transform.position.x)-2), calc * (allSpaces[i].transform.position.y + allSpaces[i + 1].transform.position.y + allSpaces[i + 9].transform.position.y +
                            allSpaces[i + 10].transform.position.y + allSpaces[i + 18].transform.position.y +
                            allSpaces[i + 19].transform.position.y), 1);
                    }
                    go.transform.position = setCentrefor6;
                    go.GetComponent<Inventory>().originalPosition = go.transform.position;
                    go.transform.SetParent(inventoryPanel.transform);
                    StartCoroutine(go.GetComponent<Inventory>().delay());
                    go.GetComponent<Inventory>().currentOccupation = WhereAmI.inInventory;
                    return;
                }
                break;

        }    
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(0.05f);
        if (invSpace.Count > 0)
            foreach (inventorySpace sqr in invSpace)
            {
                sqr.occupied = true;

                    }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Inventory"))
        {
            Squares.Add(collision.gameObject);
            invSpace.Add(collision.gameObject.GetComponent<inventorySpace>());
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Inventory"))
        {
            Squares.Remove(collision.gameObject);
        invSpace.Remove(collision.gameObject.GetComponent<inventorySpace>());
        }
    }



    void OnPickup()
    {
        AddStats();
        AddCards();
    }
    public virtual void RemoveStats()
    {
        pl.armor -= 2;
    }
    public virtual void RemoveCards()
    {
        int remove = 0;
        //GameplayManager.instance.startingDeck.remove(cards);
        for(int i =0;i< gm.startingDeck.Count;i++)
        {
            for(int j = 0; j < _inventoryItem._cards.Count; j++)
            {
                if (gm.startingDeck[i] == _inventoryItem._cards[j])
                {
                    Debug.Log("check if this is the same item");
                    gm.startingDeck.RemoveAt(i);
                    remove++;
                    continue;
                }
                if (_inventoryItem._cards.Count == remove)
                {
                    return;
                }
            }
        }
    }
    public virtual void AddCards()
    {
        gm.startingDeck.AddRange(_inventoryItem._cards);
    }
    public virtual void AddStats()
    {
        pl.armor += 2;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        temporary.Clear();
        for (int i=0;i<cards.Count;i++)
        {
            temporary.Add(Instantiate(cards[i], new Vector2(this.transform.position.x - (i + 1) * 8, this.transform.position.y), Quaternion.identity));

            temporary[i].transform.SetParent(gm.canvas.transform);
            temporary[i].transform.localScale = Vector3.one;
            temporary[i].GetComponent<Card>().enabled = false;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach(GameObject obj in temporary)
        {
            Destroy(obj);
        }
    }

    enum WhereAmI
    {
        elsewhere,
        inInventory,
        onPlayer
    }

}
