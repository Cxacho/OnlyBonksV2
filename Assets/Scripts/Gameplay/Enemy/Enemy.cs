using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class Enemy : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{

    public int maxHealth = 70;
    public int armor;
    public float baseDamage,damage;
    public float numberOfAttacks=1;
    public int _currentHealth;
    public string _name;
    public int xp;
    [HideInInspector]public int actionsInt = 0;
    private Player pl;
    public SliderHealth sdh;
    public RectTransform rect;
    
    public GameplayManager gm;
    public TMP_Text healthTxt;


    public bool targeted, isFirstTarget, isSecondTarget, isThirdTarget;
    [SerializeField] GameObject border;
    public EnemyType EnemyType;
    Vector3 mousePos;
    FollowMouse fm;

    public GameObject indicator;
    public TextMeshProUGUI attackIndicatortxt;
    public TextMeshProUGUI otherIndicatortxt;

    [HideInInspector]public List<string> indicatorStrings = new List<string>();
    [HideInInspector]public List<bool> indicatorStringsBool = new List<bool>();
    public int[] indicatorImagesInt;
    [HideInInspector]
    public Image indicatorSpriteRenderer;

 

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
        
        mousePos = fm.rectPos.anchoredPosition;


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
            updatedHealth = (int)_currentHealth - (int)(damage *0.75f);
            Debug.Log(updatedHealth - _currentHealth);
            Debug.Log("frailed");
        }
        else
        {
            Debug.Log("not Frailed");
            updatedHealth = (int)_currentHealth - (int)damage;
        }

        OnDeath(updatedHealth > 0 ? updatedHealth : 0);
        healthTxt.text = updatedHealth + "/" + maxHealth;
        sdh.SetHealth(updatedHealth);
        if (gm.enemies.Count == 0)
        {
            StartCoroutine(gm.OnBattleWin());
        }
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
            Debug.Log("�le wpisany typ");
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
}