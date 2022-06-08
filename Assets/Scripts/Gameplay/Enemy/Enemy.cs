using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
public class Enemy : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    statuses currentStatus;
    public int maxHealth = 70;
    public int armor;
    [HideInInspector] public int armorAndHp;
    public int strength,vurneable,bleed;
    public float baseDamage,damage;
    public float numberOfAttacks=1;
    public int _currentHealth;
    [HideInInspector]public string _name;
    public int xp;
    [HideInInspector]public int actionsInt = 0;
    private Player pl;
    public SliderHealth sdh;
    public RectTransform rect;
    public GameObject shield;
    public GameObject fillArmor;
    public GameObject armorImage;
    public TextMeshProUGUI textArmor;
    public GameObject dmgPopOutBlock;
    public TextMeshProUGUI dmgPopOutTMP;

    public GameplayManager gm;
    public TMP_Text healthTxt;


    public bool targeted, isFirstTarget, isSecondTarget, isThirdTarget;
    [SerializeField] GameObject border;
    public EnemyType EnemyType;
    Vector3 mousePos;
    FollowMouse fm;
    GameObject vurneableIndicator,bleedIndicator,strengthBuffIndicator;
    public GameObject indicator;
    public TextMeshProUGUI attackIndicatortxt;
    public TextMeshProUGUI otherIndicatortxt;
    public List<GameObject> statusesList = new List<GameObject>();
    [HideInInspector]public List<string> indicatorStrings = new List<string>();
    [HideInInspector]public List<bool> indicatorStringsBool = new List<bool>();
    public int[] indicatorImagesInt;
    [HideInInspector]
    public Image indicatorSpriteRenderer;
    [SerializeField] GameObject  statusEffectsTransform;



    public void OnPointerExit(PointerEventData eventData)
    {
        fm.en = null;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        fm.en = this.gameObject.GetComponent<Enemy>();
    }


    private void Awake()
    {
        
        pl = FindObjectOfType<Player>();
        gm = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        statusesList.AddRange(gm.enemiesIndicators);
        _name = this.gameObject.name;
        Debug.Log("inicjalizacja");
        _currentHealth = maxHealth;
        healthTxt.text = _currentHealth + "/" + maxHealth;
        armor = 0;
        sdh.SetMaxHealth(maxHealth);
        fm = GameObject.Find("Cursor").GetComponent<FollowMouse>();
        indicatorSpriteRenderer = indicator.GetComponent<Image>();
    }
    
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        SetAttackString(numberOfAttacks);

       if (targeted == true)
        {
            border.SetActive(true);
        }
        else
        {
            border.SetActive(false);
        }
        
        //mousePos = fm.rectPos.anchoredPosition;
   
    }
    private void FixedUpdate()
    {
        if (armor > 0)
        {
            armorImage.SetActive(true);
            fillArmor.SetActive(true);
            textArmor.enabled = true;
            textArmor.text = armor.ToString();
        }
        else
            ResetImg();
    }

    public void OnDeath(int newHealthValue)
    {
        _currentHealth = newHealthValue;
        if(_currentHealth <= 0)
        {
            


            for(int i = 0; i < gm.enemies.Count; i++)
            {
                if(gm.enemies[i].name == _name)
                {
                    gm.currentXP += gm.enemies[i].GetComponent<Enemy>().xp; //dodanie xpa za przeciwnika

                    gm.enType.RemoveAt(i);
                    gm.enemies.RemoveAt(i); //usuni?cie danego przeciwnika z listy
                }
            }
            
            Destroy(gameObject);
            
        }
        
    }

    public void ReceiveDamage(float damage)
    {
        int updatedHealth;
        if (pl.frail > 0)
        {
            if (armor > 0)
            {
                
                if (armor > damage)
                {
                    armor -= Mathf.RoundToInt(damage*0.75f);
                    textArmor.text = armor.ToString();

                    dmgPopOutTMP.text = "Blocked " + damage + " dmg";
                    dmgPopOutTMP.color = new Color(0, 0, 255);

                    TextMeshProUGUI dmgText;
                    dmgText = Instantiate(dmgPopOutTMP, dmgPopOutBlock.transform);


                    Sequence dmgTextSeq = DOTween.Sequence();
                    dmgTextSeq.Append(dmgText.transform.DOMoveY(8f, 1f));
                    dmgTextSeq.Insert(0, dmgText.transform.DOMoveX(1f, dmgTextSeq.Duration()));
                    dmgTextSeq.OnComplete(() => { GameObject.Destroy(dmgText); });

                    updatedHealth = _currentHealth;
                } // enemy ma wiecej armora niz my obrazen
                else
                {
                    armorAndHp = armor + _currentHealth;
                    armorAndHp -= Mathf.RoundToInt(damage*0.75f);
                    int dmgarm;
                    dmgarm = Mathf.RoundToInt(damage*0.75f) - armor;
                    armor = 0;
                    updatedHealth = armorAndHp;
                    ResetImg();

                    dmgPopOutTMP.text = "- " + dmgarm;
                    dmgPopOutTMP.color = new Color(255, 0, 0);

                    TextMeshProUGUI dmgText;
                    dmgText = Instantiate(dmgPopOutTMP, dmgPopOutBlock.transform);

                    Sequence dmgTextSeq = DOTween.Sequence();
                    dmgTextSeq.Append(dmgText.transform.DOMoveY(8f, 0.5f));
                    dmgTextSeq.Append(dmgText.transform.DOMoveY(-60f, 1f));
                    dmgTextSeq.Insert(0, dmgText.transform.DOMoveX(1f, dmgTextSeq.Duration()));
                    dmgTextSeq.OnComplete(() => { GameObject.Destroy(dmgText); });
                } // enemy ma mniej armora niz my obrazen
            } //enemy ma armora
            else 
            {

                updatedHealth = (int)_currentHealth - Mathf.RoundToInt(damage*0.75f);

                dmgPopOutTMP.text = "- " + damage;
                dmgPopOutTMP.color = new Color(255, 0, 0);
                TextMeshProUGUI dmgText;
                dmgText = Instantiate(dmgPopOutTMP, dmgPopOutBlock.transform);

                Sequence dmgTextSeq = DOTween.Sequence();
                dmgTextSeq.Append(dmgText.transform.DOMoveY(8f, 0.5f));
                dmgTextSeq.Append(dmgText.transform.DOMoveY(-60f, 1f));
                dmgTextSeq.Insert(0, dmgText.transform.DOMoveX(1f, dmgTextSeq.Duration()));
                dmgTextSeq.OnComplete(() => { GameObject.Destroy(dmgText); });
            } //enemy nie ma armora

            OnDeath(updatedHealth > 0 ? updatedHealth : 0);
            healthTxt.text = updatedHealth + "/" + maxHealth;
            sdh.SetHealth(updatedHealth);
            
        } //enemy ma fraila
        else
        {
            if (armor > 0)
            {

                if (armor > damage)
                {
                    armor -= Mathf.RoundToInt(damage);
                    textArmor.text = armor.ToString();

                    dmgPopOutTMP.text = "Blocked " + damage + " dmg";
                    dmgPopOutTMP.color = new Color(0, 0, 255);

                    TextMeshProUGUI dmgText;
                    dmgText = Instantiate(dmgPopOutTMP, dmgPopOutBlock.transform);


                    Sequence dmgTextSeq = DOTween.Sequence();
                    dmgTextSeq.Append(dmgText.transform.DOMoveY(8f, 1f));
                    dmgTextSeq.Insert(0, dmgText.transform.DOMoveX(1f, dmgTextSeq.Duration()));
                    dmgTextSeq.OnComplete(() => { GameObject.Destroy(dmgText); });

                    updatedHealth = _currentHealth;
                } // enemy ma wiecej armora niz my obrazen
                else
                {
                    armorAndHp = armor + _currentHealth;
                    armorAndHp -= Mathf.RoundToInt(damage);
                    int dmgarm;
                    dmgarm = Mathf.RoundToInt(damage) - armor;
                    armor = 0;
                    updatedHealth = armorAndHp;
                    ResetImg();

                    dmgPopOutTMP.text = "- " + dmgarm;
                    dmgPopOutTMP.color = new Color(255, 0, 0);

                    TextMeshProUGUI dmgText;
                    dmgText = Instantiate(dmgPopOutTMP, dmgPopOutBlock.transform);

                    Sequence dmgTextSeq = DOTween.Sequence();
                    dmgTextSeq.Append(dmgText.transform.DOMoveY(8f, 0.5f));
                    dmgTextSeq.Append(dmgText.transform.DOMoveY(-60f, 1f));
                    dmgTextSeq.Insert(0, dmgText.transform.DOMoveX(1f, dmgTextSeq.Duration()));
                    dmgTextSeq.OnComplete(() => { GameObject.Destroy(dmgText); });
                } // enemy ma mniej armora niz my obrazen
            } //enemy ma armora
            else
            {

                updatedHealth = (int)_currentHealth - Mathf.RoundToInt(damage);

                dmgPopOutTMP.text = "- " + damage;
                dmgPopOutTMP.color = new Color(255, 0, 0);
                TextMeshProUGUI dmgText;
                dmgText = Instantiate(dmgPopOutTMP, dmgPopOutBlock.transform);

                Sequence dmgTextSeq = DOTween.Sequence();
                dmgTextSeq.Append(dmgText.transform.DOMoveY(8f, 0.5f));
                dmgTextSeq.Append(dmgText.transform.DOMoveY(-60f, 1f));
                dmgTextSeq.Insert(0, dmgText.transform.DOMoveX(1f, dmgTextSeq.Duration()));
                dmgTextSeq.OnComplete(() => { GameObject.Destroy(dmgText); });
            } //enemy nie ma armora

            OnDeath(updatedHealth > 0 ? updatedHealth : 0);
            healthTxt.text = updatedHealth + "/" + maxHealth;
            sdh.SetHealth(updatedHealth);
        }//enemy nie ma fraila

        if (gm.enemies.Count == 0)
        {
            StartCoroutine(gm.OnBattleWin());
        }
    }

    public void GetArmor(int value)
    {
        
        if (armor == 0)
        {
            Instantiate(shield, new Vector3(GameObject.FindGameObjectWithTag("Enemy").transform.position.x + 2.5f, GameObject.FindGameObjectWithTag("Enemy").transform.position.y + 2.5f, 0), Quaternion.identity, GameObject.Find("Player").transform);
        }
        armor += value;
    }

    public void SetIndicator()
    {
        if (indicatorStringsBool[actionsInt] == true)
        {           
            attackIndicatortxt.enabled = true;            
        }
        else
        {
            attackIndicatortxt.enabled = false;   
        }
        indicatorSpriteRenderer.sprite = gm.indicatorImages[indicatorImagesInt[actionsInt]];

    }
    public void SetAttackString(float iloscAtakow)
    {
        attackIndicatortxt.text = (damage * iloscAtakow).ToString();
    }
    public void ChangeIndicatorTexts(string typ)
    {

        if(typ == "atak")
        {
            attackIndicatortxt.enabled = true;
            otherIndicatortxt.enabled = false;
        }
        else if(typ == "inny")
        {
            attackIndicatortxt.enabled = false;
            otherIndicatortxt.enabled = true;
        }
        else
        {
            Debug.Log("èle wpisany typ");
        }
    }

    public void NextCaseAttack(float nrOfAttacks)
    {
        ChangeIndicatorTexts("atak");
        SetIndicator();
        actionsInt++;
        numberOfAttacks = Mathf.RoundToInt(nrOfAttacks);
    }
    public void NextCaseOther(string value)
    {
        SetIndicator();
        actionsInt++;
        ChangeIndicatorTexts("inny");
        otherIndicatortxt.text = value;
    }
    public void setStatusIndicator(int value,int select,GameObject statusIndicator)
    {
        currentStatus = (statuses)select;
        switch(currentStatus)
        {
            case Enemy.statuses.strengthBuff:
                strength += value;
                if (strengthBuffIndicator == null)
                    strengthBuffIndicator = Instantiate(statusIndicator, statusEffectsTransform.transform);
                var statusValue = strengthBuffIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                statusValue.text = strength.ToString();
                break;
            case Enemy.statuses.vurneable:
                vurneable += value;
                if (vurneableIndicator == null)
                    vurneableIndicator = Instantiate(statusIndicator, statusEffectsTransform.transform);
                var statusValue1 = vurneableIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                statusValue1.text = vurneable.ToString();
                break;

            case Enemy.statuses.bleeding:
                bleed += value;
                if (bleedIndicator == null)
                    bleedIndicator = Instantiate(statusIndicator, statusEffectsTransform.transform);
                var statusValue2 = bleedIndicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                statusValue2.text = bleed.ToString();
                break;
        }
        //strength
    }
    public void OnEndTurn()
    {
        if(bleed >0)
        ReceiveDamage(bleed);
        if (strength > 0) strength--;
        if (vurneable > 0) vurneable--;
        if (bleed > 0) bleed--;
        List<GameObject> temp = new List<GameObject>();
        temp.Clear();
        for (int i = 0; i < statusEffectsTransform.transform.childCount; i++)
            temp.Add(statusEffectsTransform.transform.GetChild(i).gameObject);

        foreach (GameObject ind in temp)
        {
           var statusText = ind.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            int statusAmount;
            int.TryParse(statusText.text.ToString(), out statusAmount);
            statusAmount -= 1;
            if (statusAmount > 0)
                statusText.text = statusAmount.ToString();
            else
                Destroy(ind);
        }
        
        /*
        foreach(GameObject ind in )
        {
            var statusText = ind.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            int statusAmount;
            int.TryParse(statusText.text.ToString(), out statusAmount);
            if (statusAmount > 0)
                statusText.text = statusAmount.ToString();
            else
                Destroy(ind);
        }

        */
    }
    enum statuses
    {
        strengthBuff =0,
        vurneable =1,
        bleeding = 2
    }

    public void ResetImg()
    {
        armorImage.SetActive(false);
        fillArmor.SetActive(false);
        textArmor.enabled = false;
        textArmor.text = armor.ToString();
    }
}