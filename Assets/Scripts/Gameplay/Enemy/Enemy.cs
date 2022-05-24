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
    public int damage;
    public int _currentHealth;
    public string _name;
    public int xp;
    [HideInInspector]public int actionsInt = 0;

    public SliderHealth sdh;
    public RectTransform rect;
    
    public GameplayManager gm;
    public TMP_Text healthTxt;


    public bool targeted, isFirstTarget, isSecondTarget, isThirdTarget;
    [SerializeField] GameObject border;
    public EnemyType EnemyType;
    public Vector3[] corners;
    Vector3 mousePos;
    FollowMouse fm;

    public GameObject indicator;
    public TextMeshProUGUI indicatortxt;

    public Sprite[] indicatorImages;
    public List<string> indicatorStrings = new List<string>();
    public List<bool> indicatorStringsBool = new List<bool>();
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
        
        

        corners = new Vector3[4];
        rect = GetComponent<RectTransform>();
        rect.GetLocalCorners(corners);
        


        
    }

    private void Update()
    {
       if (targeted == true)
        {
            border.SetActive(true);
        }
        else
        {
            border.SetActive(false);
        }
        mousePos = fm.rectPos.anchoredPosition;

        Debug.Log(actionsInt);

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

        int updatedHealth = (int)_currentHealth - (int)damage;
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
        indicatortxt.text = indicatorStrings[actionsInt];
        if (indicatorStringsBool[actionsInt] == true) {
            
            indicatortxt.enabled = true;
        }
        else
        {
            indicatortxt.enabled = false;
        }
        indicatorSpriteRenderer.sprite = indicatorImages[indicatorImagesInt[actionsInt]];

    }
}