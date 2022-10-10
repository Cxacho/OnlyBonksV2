using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System;
public class Player : MonoBehaviour
{
    public GameObject fillArmor;
    public GameObject textHealth;
    public GameObject textArmor;
    public GameObject shield;
    GameObject frailIndicator, vurneableIndicator, poisonIndicator, strengthBuffIndicator,pierceIndicator;
    [HideInInspector]public GameObject energizeIndicator;
    TextMeshProUGUI value;
    public int maxHealth;
    public int currentHealth;
    public int armor;
    public int armorAndHp;
    public int mana;
    public GameObject armorImage,buffsAndDebuffs,testStatusIndicator;
    public TMP_Text manaText;
    public TMP_Text healthText,topHp;
    public TMP_Text armorText;
    public SliderHealth sdh;
    public GameObject dmgPopOutBlock;
    public TextMeshProUGUI dmgPopOutTMP;
    public GameplayManager gameplayManager;
    [HideInInspector]public int tempStrength,tempDexterity,tempInteligence;
    public int strenght = 0;
    public int dexterity = 0;
    public int inteligence = 0;
    public int frail,vurneable,poison,energize;
    public List<GameObject> buffIndicators = new List<GameObject>();
    playerStatusses currentBuff;
    public Vector3 myOriginalPosition;

    

    private void Awake()
    {
        currentHealth = maxHealth;
        armor = 0;
        mana = 3;
        myOriginalPosition = GetComponent<RectTransform>().anchoredPosition3D;
        gameplayManager.OnTurnEnd += UpdateValues;
    }

    void Start()
    {
        manaText.text = mana.ToString();
    }
    private void Update()
    {
        if (Input.GetKeyUp("p"))
        {
            Time.timeScale = 2f;
        }
        if (Input.GetKeyUp("l"))
        {
            Time.timeScale = 0.5f;

        }
        if (Input.GetKeyUp("m"))
        {
            Time.timeScale = 1f;
        }
        if (armor > 0)
        {
            armorImage.SetActive(true);
            fillArmor.SetActive(true);
            textHealth.SetActive(true);
            textArmor.SetActive(true);
            armorText.text = armor.ToString();
        }
        else
            ResetImg();
    }

    public void ResetImg()
    {
        armorImage.SetActive(false);
        fillArmor.SetActive(false);
        textHealth.SetActive(true);
        textArmor.SetActive(false);
        armorText.text = armor.ToString();
    }
    /// <summary>
    ///         frail=0,vurneable=1, poision =2,strengthBuff=3,energize=4,
    /// </summary>
    /// <param name="value"></param>
    /// <param name="select"></param>
    /// <param name="buffIndicator"></param>
    public void setStatusIndicator(int value,int select,GameObject buffIndicator)
    {
        currentBuff = (playerStatusses)select;
        switch(currentBuff)
        {
            case Player.playerStatusses.frail:
                frail += value;

                if (frailIndicator == null)
                {
                    frailIndicator = Instantiate(buffIndicator, buffsAndDebuffs.transform);

                }
                var buffValue = frailIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                buffValue.text = frail.ToString();
                break;
            case Player.playerStatusses.vurneable:
                vurneable += value;
                if (vurneableIndicator == null)
                {
                    vurneableIndicator = Instantiate(buffIndicator, buffsAndDebuffs.transform);

                }
                var buffValue1 = vurneableIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                buffValue1.text = vurneable.ToString();
                break;
            case Player.playerStatusses.poision:
                poison += value;
                if (poisonIndicator == null)
                {
                    poisonIndicator = Instantiate(buffIndicator, buffsAndDebuffs.transform);

                }
                var buffValue2 = poisonIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                buffValue2.text = poison.ToString();
                break;
            case Player.playerStatusses.strengthBuff:
                //strenght += tempStrength;
                strenght += value;
                if (strengthBuffIndicator == null)
                {
                    strengthBuffIndicator = Instantiate(buffIndicator, buffsAndDebuffs.transform);

                }
                var buffValue3 = strengthBuffIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                buffValue3.text = strenght.ToString();
                break;
            case Player.playerStatusses.energize:
                energize += value;
                if (strengthBuffIndicator == null)
                {
                    energizeIndicator = Instantiate(buffIndicator, buffsAndDebuffs.transform);

                }
                foreach (Card obj in FindObjectsOfType<Card>())
                    if(obj.cost >0)
                    obj.cost -= 1; 
                var buffValue4 = energizeIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                buffValue4.text = energize.ToString();
                break;

            case Player.playerStatusses.pierce:
                if (pierceIndicator == null)
                {

                    pierceIndicator = Instantiate(buffIndicator, buffsAndDebuffs.transform);
                    //onturnend destroy indicator/effect
                }
                break;
        }

    }
    
    public void TakeDamage(float damage)
    {
        Debug.Log(damage);
        if (armor > 0)
        {
            if (armor > damage)
            {
                armor -= Mathf.RoundToInt(damage);
                armorText.text = armor.ToString();

                dmgPopOutTMP.text = "Blocked " + Mathf.RoundToInt(damage) + " dmg";
                dmgPopOutTMP.color = new Color(0, 0, 255);
                
                TextMeshProUGUI dmgText;
                dmgText = Instantiate(dmgPopOutTMP, dmgPopOutBlock.transform);

                
                Sequence dmgTextSeq = DOTween.Sequence();
                dmgTextSeq.Append(dmgText.transform.DOMoveY(8f, 1f));
                dmgTextSeq.Insert(0, dmgText.transform.DOMoveX(1f, dmgTextSeq.Duration()));
                dmgTextSeq.OnComplete(() => { GameObject.Destroy(dmgText); });

            }
            else
            {
                armorAndHp = armor + currentHealth;
                armorAndHp -= Mathf.RoundToInt(damage);
                int dmgarm;
                dmgarm = Mathf.RoundToInt(damage) - armor;
                armor = 0;
                currentHealth = armorAndHp;
                ResetImg();
                setHP();

                
                
                dmgPopOutTMP.text = "- " + dmgarm;
                dmgPopOutTMP.color = new Color(255, 0, 0);
                
                TextMeshProUGUI dmgText;
                dmgText = Instantiate(dmgPopOutTMP, dmgPopOutBlock.transform);

                Sequence dmgTextSeq = DOTween.Sequence();
                dmgTextSeq.Append(dmgText.transform.DOMoveY(8f, 0.5f));
                dmgTextSeq.Append(dmgText.transform.DOMoveY(-60f, 1f));
                dmgTextSeq.Insert(0, dmgText.transform.DOMoveX(1f, dmgTextSeq.Duration()));
                dmgTextSeq.OnComplete(() => { GameObject.Destroy(dmgText); });
            }
        }
        else
        {                      
            currentHealth -= Mathf.RoundToInt(damage);

            setHP();

            dmgPopOutTMP.text = "- " + Mathf.RoundToInt(damage);
            dmgPopOutTMP.color = new Color(255, 0, 0);
            TextMeshProUGUI dmgText;
            dmgText = Instantiate(dmgPopOutTMP, dmgPopOutBlock.transform);

            Sequence dmgTextSeq = DOTween.Sequence();
            dmgTextSeq.Append(dmgText.transform.DOMoveY(8f, 0.5f));
            dmgTextSeq.Append(dmgText.transform.DOMoveY(-60f, 1f));
            dmgTextSeq.Insert(0, dmgText.transform.DOMoveX(1f, dmgTextSeq.Duration()));
            dmgTextSeq.OnComplete(() => { GameObject.Destroy(dmgText); });
        }
            
    }
    public void TakeHealthDamage(float damage)
    {
        currentHealth -= Mathf.RoundToInt(damage);
        setHP();
        dmgPopOutTMP.text = "- " + Mathf.RoundToInt(damage);
        dmgPopOutTMP.color = new Color(255, 0, 0);
        TextMeshProUGUI dmgText;
        dmgText = Instantiate(dmgPopOutTMP, dmgPopOutBlock.transform);

        Sequence dmgTextSeq = DOTween.Sequence();
        dmgTextSeq.Append(dmgText.transform.DOMoveY(8f, 0.5f));
        dmgTextSeq.Append(dmgText.transform.DOMoveY(-60f, 1f));
        dmgTextSeq.Insert(0, dmgText.transform.DOMoveX(1f, dmgTextSeq.Duration()));
        dmgTextSeq.OnComplete(() => { GameObject.Destroy(dmgText); });
    }
    public void Heal(int value)
    {
        
        if (currentHealth + value < maxHealth)
            currentHealth += value;
        else if (value + currentHealth > maxHealth)
        {
            if (currentHealth == maxHealth)
            {
                TakeDamage(5);
                //wstaw wiadomosc NO OVERHEALING U MOROn
            }
            else
            {
                var ss = value + currentHealth;
                int healValue = ss - maxHealth;
                currentHealth = maxHealth;
            }
        }
        setHP();


    }
    public void GetArmor(int value)
    {

        if (armor == 0)
        {
            Instantiate(shield, new Vector3(GameObject.Find("Player").transform.position.x+2.5f, GameObject.Find("Player").transform.position.y+2.5f, 0), Quaternion.identity, GameObject.Find("Player").transform);
        }
        armor += value;
    }
    public void Charmed()
    {
       strenght = strenght - 1;
    }
    public void setHP()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            
        }
        sdh.SetHealth(currentHealth);
        healthText.text = currentHealth + "/" + maxHealth;
        topHp.text = currentHealth + "/" + maxHealth;
    }
    public void ResetPlayerArmor()
    {
        armor = 0;
    }
    public void AssignMana()
    {
        mana = 3;
        manaText.text = mana.ToString();
    }
    public void OnEndTurn()
    {
        foreach(Card card in FindObjectsOfType<Card>())
        {
            card.index = card.gameObject.transform.GetSiblingIndex();
        }

        //if (strenght != 0)
            //strenght--;
        //dowymiany
        foreach (Enemy en in FindObjectsOfType<Enemy>())
        {
            en.OnEndTurn();
            //zadziala poprawnie, wypierdoli sie gdy dostaniemy itemek lub karte ktora cleansuje debuffy
            if (vurneable > 0)
                en.damage = Mathf.RoundToInt(en.baseDamage * 1.25f);
            else
                en.damage = en.baseDamage;
        }

    }
    
    public void OnBattleSetup()
    {
        armor = 0;
        gameplayManager.playerHand.Clear();

        //gameplayManager.discardDeck.ForEach(item => gameplayManager.drawDeck.Add(item));
        gameplayManager.drawDeck.Clear();
        gameplayManager.discardDeck.Clear();
        gameplayManager.drawDeck.AddRange(gameplayManager.startingDeck);
        strenght = 0;
        dexterity = 0;
        inteligence = 0;
        frail = 0;
        vurneable= 0;
        poison=0;
        //usuniecie indicatora


        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("BuffIndicator"))
        {
            Destroy(obj);
        }
    }
    public void Walk(Vector3 destination)
    {
        //check jak daleka jest odleglosc poprzez lerp
        //ruch wylancznie sprajtem, bez paska i innych dupereli
        transform.DORotate(new Vector3(0, 180, -40), 0.4f);

        RectTransform rect = GetComponent<RectTransform>();
        rect.DOAnchorPos3D(destination,1f).OnComplete(()=>
        {
            transform.DORotate(new Vector3(0, 180, 0), 0.2f);
        }); 
        //na dotarciu przerwac rotacje
    }
    void UpdateValues(object sender, EventArgs e)
    {
        //tempStrength = 0;
        //setStatusIndicator(0, 3, buffIndicators[3]);
        //dodac

    }
    public enum playerStatusses
    {
        frail = 0,
        vurneable = 1,
        poision = 2,
        strengthBuff = 3,
        energize = 4,
        pierce = 5,
        brak = 10
    }
    public void setStatus(playerStatusses ps,int value)
    {
        currentBuff = ps;
        switch (currentBuff)
        {
            case playerStatusses.brak:

                //instantiate obiekt, ten obiekt na spawnie bedzie sprawdzal czy juz jest obiekt tego typu, gdy jest, dodaj lub odejmij value, je?li go nie ma, dodaj obiekt
                break;
            case playerStatusses.energize:
                var statusGO = Instantiate(buffIndicators[5], buffsAndDebuffs.transform);
                statusGO.GetComponent<Indicator>().checkIfIExist(ps, value);
                break;
            case playerStatusses.frail:
                var statusGO1 = Instantiate(buffIndicators[0], buffsAndDebuffs.transform);
                statusGO1.GetComponent<Indicator>().checkIfIExist(ps, value);
                break;
            case playerStatusses.pierce:
                var statusGO2 = Instantiate(buffIndicators[4],buffsAndDebuffs.transform);
                statusGO2.GetComponent<Indicator>().checkIfIExist(ps, value);
                break;
            case playerStatusses.poision:
                var statusGO3 = Instantiate(buffIndicators[2], buffsAndDebuffs.transform);
                statusGO3.GetComponent<Indicator>().checkIfIExist(ps, value);
                break;
            case playerStatusses.strengthBuff:
                var statusGO4 = Instantiate(buffIndicators[3], buffsAndDebuffs.transform);
                statusGO4.GetComponent<Indicator>().checkIfIExist(ps, value);
                break;
            case playerStatusses.vurneable:
                var statusGO5 = Instantiate(buffIndicators[1], buffsAndDebuffs.transform);
                statusGO5.GetComponent<Indicator>().checkIfIExist(ps, value);
                break;
        }
    }
    
}
