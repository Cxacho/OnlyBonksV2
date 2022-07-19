using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class Player : MonoBehaviour
{
    public GameObject fillArmor;
    public GameObject textHealth;
    public GameObject textArmor;
    public GameObject shield;
    GameObject frailIndicator, vurneableIndicator, poisonIndicator, strengthBuffIndicator;
    [HideInInspector]public GameObject energizeIndicator;
    TextMeshProUGUI value;
    public int maxHealth = 70;
    public int currentHealth;
    public int armor;
    public int armorAndHp;
    public int mana;
    public GameObject armorImage,buffsAndDebuffs;
    public TMP_Text manaText;
    public TMP_Text healthText,topHp;
    public TMP_Text armorText;
    public SliderHealth sdh;
    public GameObject dmgPopOutBlock;
    public TextMeshProUGUI dmgPopOutTMP;
    public GameplayManager gm;
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
        myOriginalPosition = this.transform.position;
    }

    void Start()
    {
        manaText.text = mana.ToString();
    }
    private void Update()
    {
        if (Input.GetKeyUp("p"))
        {
            Walk(myOriginalPosition);
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
        
        if(strenght>0 ) strenght--;
        if(dexterity>0 ) dexterity--;
        if(inteligence>0) inteligence--;
        if (frail > 0) frail--;
        if (vurneable > 0) vurneable--;
        if (poison > 0)
        {
            TakeDamage(poison);
            if (currentHealth == 0)
            {
                gm.state = BattleState.LOST;
                StartCoroutine(gm.OnBattleLost());
            }
            poison--;
        }
        //usuniecie indicatora
        

        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("BuffIndicator"))
        {
             var some = obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            int statusAmount;
            int.TryParse(some.text.ToString(),out statusAmount);
            statusAmount -= 1;
            if (statusAmount > 0)
            {
                some.text = statusAmount.ToString();

            }
            else
            {
                Destroy(obj);

             }
        }
        
        foreach(Enemy en in FindObjectsOfType<Enemy>())
        {
            en.OnEndTurn();
            //zadziala poprawnie, wypierdoli sie gdy dostaniemy itemek lub karte ktora cleansuje debuffy
            if (vurneable > 0)
                en.damage = Mathf.RoundToInt(en.baseDamage * 1.25f);
            else
                en.damage = en.baseDamage;
        }
        foreach(Relic re in FindObjectsOfType<Relic>())
        {
            re.OnEndTurn();
        }

    }
    enum playerStatusses
    {
        frail=0,
        vurneable=1,
        poision =2,
        strengthBuff=3,
        energize=4,
        brak = 10
    }
    public void OnBattleSetup()
    {
        armor = 0;
        gm.playerHand.Clear();

        //gm.discardDeck.ForEach(item => gm.drawDeck.Add(item));
        gm.drawDeck.Clear();
        gm.discardDeck.Clear();
        gm.drawDeck.AddRange(gm.startingDeck);
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
        float velocity = 34;

        float distance = Mathf.Abs(destination.x - this.transform.position.x);
        var time = distance / velocity;
        //ruch wylancznie sprajtem, bez paska i innych dupereli
        transform.DORotate(new Vector3(0, 180, -40), 0.4f).OnComplete(() =>
        {
            transform.DORotate(new Vector3(0, 180, 40), 0.4f, RotateMode.Fast).SetLoops(3, LoopType.Yoyo).OnComplete(() =>
           {
               transform.DORotate(new Vector3(0, 180, 0), 0.4f);
           });
        });
        
        transform.DOMove(destination, time); 
        //na dotarciu przerwac rotacje
    }
}
