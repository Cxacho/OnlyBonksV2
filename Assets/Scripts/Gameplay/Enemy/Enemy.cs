using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class Enemy : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{

    public float maxHealth = 70;
    public float armor;
    public float damage;
    public float _currentHealth;
    
    
    public SliderHealth sdh;
    public RectTransform rect;
    
    public GameplayManager gm;
    public TMP_Text healthTxt;
    
    
    public bool targeted;
    [SerializeField] GameObject border;
    public EnemyType EnemyType;
    public Vector3[] corners;
    Vector3 mousePos;
    FollowMouse fm;

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
    }
    
    void Start()
    {
        Debug.Log("inicjalizacja");
        _currentHealth = maxHealth;
        healthTxt.text = _currentHealth + "/" + maxHealth;
        armor = 0;
        sdh.SetMaxHealth(maxHealth);
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

    public void UpdateHealth(float newHealthValue)
    {
        _currentHealth = newHealthValue;
        Debug.Log(_currentHealth);
    }

    public void ReceiveDamage(float damage)
    {
        float updatedHealth = _currentHealth - damage;
        UpdateHealth(updatedHealth > 0 ? updatedHealth : 0);
        healthTxt.text = updatedHealth + "/" + maxHealth;
        sdh.SetHealth(updatedHealth);
    }


}