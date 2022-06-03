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
    GameObject frailIndicator, vurneableIndicator, poisonIndicator,strengthBuffIndicator;
    TextMeshProUGUI value;
    public int maxHealth = 70;
    public int currentHealth;
    public int armor;
    public int armorAndHp;
    public int mana;
    public GameObject armorImage,buffsAndDebuffs;
    public TMP_Text manaText;
    public TMP_Text healthText;
    public TMP_Text armorText;
    public SliderHealth sdh;
    public GameObject dmgPopOutBlock;
    public TextMeshProUGUI dmgPopOutTMP;
    public GameplayManager gm;
    public int strenght = 0;
    public int dexterity = 0;
    public int inteligence = 0;
    public int frail,vurneable,poison;
    public List<GameObject> buffIndicators = new List<GameObject>();
    buffs currentBuff;
    private void Awake()
    {
        currentHealth = maxHealth;
        armor = 0;
        mana = 3;

    }

    void Start()
    {
        manaText.text = mana.ToString();
    }
    private void Update()
    {
        if (armor > 0)
        {
            armorImage.SetActive(true);
            fillArmor.SetActive(true);
            textHealth.SetActive(false);
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
    
    public void setDebuffIndicator(int value,int select,GameObject buffIndicator)
    {
        currentBuff = (buffs)select;
        switch(currentBuff)
        {
            case Player.buffs.frail:
                frail += value;

                if (frailIndicator == null)
                {
                    frailIndicator = Instantiate(buffIndicator, buffsAndDebuffs.transform);

                }
                var buffValue = frailIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                buffValue.text = frail.ToString();
                break;
            case Player.buffs.vurneable:
                vurneable += value;
                if (vurneableIndicator == null)
                {
                    vurneableIndicator = Instantiate(buffIndicator, buffsAndDebuffs.transform);

                }
                var buffValue1 = vurneableIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                buffValue1.text = vurneable.ToString();
                break;
            case Player.buffs.poision:
                poison += value;
                if (poisonIndicator == null)
                {
                    poisonIndicator = Instantiate(buffIndicator, buffsAndDebuffs.transform);

                }
                var buffValue2 = poisonIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                buffValue2.text = poison.ToString();
                break;
            case Player.buffs.strengthBuff:
                strenght += value;
                if (strengthBuffIndicator == null)
                {
                    strengthBuffIndicator = Instantiate(buffIndicator, buffsAndDebuffs.transform);

                }
                var buffValue3 = strengthBuffIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                buffValue3.text = strenght.ToString();
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
                armor -= (int)damage;
                armorText.text = armor.ToString();

                dmgPopOutTMP.text = "Blocked " + damage + " dmg";
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
                armorAndHp -= (int)damage;
                int dmgarm;
                dmgarm = (int)damage - armor;
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
            currentHealth -= (int)damage;
            setHP();

            dmgPopOutTMP.text = "- " + damage;
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
        currentHealth -= (int)damage;
        setHP();
        dmgPopOutTMP.text = "- " + damage;
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
            Instantiate(shield, new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, 0), Quaternion.identity, GameObject.Find("Player").transform);
        }
        armor += value;
        manaText.text = mana.ToString();
    }
    public void Charmed()
    {
       strenght = strenght - 1;
    }
    public void setHP()
    {
        if (currentHealth <= 0)
        {
            gm.StartCoroutine(gm.OnBattleLost());
        }
        sdh.SetHealth(currentHealth);
        healthText.text = currentHealth + "/" + maxHealth;
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

        
        if(strenght>0 ) strenght--;
        if(dexterity>0 ) dexterity--;
        if(inteligence>0) inteligence--;
        if (frail > 0) frail--;
        if (vurneable > 0) vurneable--;
        if (poison > 0)
        {
            TakeDamage(poison);
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
    enum buffs
    {
        frail=0,
        vurneable=1,
        poision =2,
        strengthBuff=3
    }
}
