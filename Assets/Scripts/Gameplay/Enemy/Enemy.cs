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
    public SliderHealth sdh;
    public GameplayManager gm;
    public TMP_Text healthTxt;
    private void Awake()
    {
        currentHealth = maxHealth;
        armor = 0;
    }

    void Start()
    {
        sdh.SetMaxHealth(maxHealth);
    }
    private void Update()
    {
        
        sdh.SetHealth(currentHealth);
        healthTxt.text = currentHealth + "/" + maxHealth;
    }

}