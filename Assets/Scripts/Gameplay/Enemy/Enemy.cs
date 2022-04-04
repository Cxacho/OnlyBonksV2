using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Enemy : MonoBehaviour
{

    public int maxHealth = 70;
    public int currentHealth;
    public int armor;
    public int damage;
    FollowMouse fm;
    Vector3 mousePos;
    public SliderHealth sdh;
    public RectTransform rect;
    public Vector3[] corners;
    public GameplayManager gm;
    public TMP_Text healthTxt;
    public bool targeted;
    [SerializeField] GameObject border;
    
    private void Awake()
    {
        fm = GameObject.Find("Cursor").GetComponent<FollowMouse>();
        
        corners = new Vector3[4];
        rect = GetComponent<RectTransform>();
        rect.GetLocalCorners(corners);
        currentHealth = maxHealth;
        armor = 0;
    }
    
    void Start()
    {
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
        sdh.SetHealth(currentHealth);
        healthTxt.text = currentHealth + "/" + maxHealth;
        if (mousePos.x> rect.anchoredPosition.x + corners[0].x  && mousePos.x< rect.anchoredPosition.x +corners[3].x)
        {
            //podpisac event na wejscie i wyjscie z lokacji czy cos
            fm.en = GetComponent<Enemy>();
        }


    }

}