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

   // public GameObject indicator;
   // public TextMeshProUGUI indicatortxt;

/*  public Sprite[] indicatorImages;
    [HideInInspector]
    public Image indicatorSpriteRenderer;*/

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
        
        Debug.Log("inicjalizacja");
        _currentHealth = maxHealth;
        healthTxt.text = _currentHealth + "/" + maxHealth;
        armor = 0;
        sdh.SetMaxHealth(maxHealth);
    }
    
    void Start()
    {
        
        fm = GameObject.Find("Cursor").GetComponent<FollowMouse>();

        corners = new Vector3[4];
        rect = GetComponent<RectTransform>();
        rect.GetLocalCorners(corners);
        //indicatorSpriteRenderer = indicator.GetComponent<Image>();


        
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

                    gm.enemies.RemoveAt(i); //usuniêcie danego przeciwnika z listy
                    gm.currentXP += gm.enemies[i].GetComponent<EnemyOne>().xp; //dodanie xpa za przeciwnika
                }
            }
            
            Destroy(gameObject);
            
        }
        
    }

    public void ReceiveDamage(int damage)
    {
        int updatedHealth = _currentHealth - damage;
        OnDeath(updatedHealth > 0 ? updatedHealth : 0);
        healthTxt.text = updatedHealth + "/" + maxHealth;
        sdh.SetHealth(updatedHealth);
    }

    
}