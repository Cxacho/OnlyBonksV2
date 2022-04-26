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
        fm = GameObject.Find("Cursor").GetComponent<FollowMouse>();
        
        corners = new Vector3[4];
        rect = GetComponent<RectTransform>();
        rect.GetLocalCorners(corners);
        indicatorSpriteRenderer = indicator.GetComponent<Image>();


        Debug.Log("inicjalizacja");
        _currentHealth = maxHealth;
        healthTxt.text = _currentHealth + "/" + maxHealth;
        armor = 0;
        sdh.SetMaxHealth(maxHealth);
    }
    
    void Start()
    {
        
    }

    private void Update()
    {
        if(targeted == true)
        {
            border.SetActive(true);
        }
        else
        {
            border.SetActive(false);
        }
        mousePos = fm.rectPos.anchoredPosition;

    }

    public void UpdateHealth(int newHealthValue)
    {
        _currentHealth = newHealthValue;
        if(_currentHealth <= 0)
        {
            for(int i = 0; i < gm.enemies.Count; i++)
            {
                if(gm.enemies[i].name == _name)
                {

                    gm.enemies.RemoveAt(i);
                }
            }
            Destroy(gameObject);
            
        }
        Debug.Log(_currentHealth);
    }

    public void ReceiveDamage(int damage)
    {
        int updatedHealth = _currentHealth - damage;
        UpdateHealth(updatedHealth > 0 ? updatedHealth : 0);
        healthTxt.text = updatedHealth + "/" + maxHealth;
        sdh.SetHealth(updatedHealth);
    }

    
}