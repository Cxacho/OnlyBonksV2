using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public GameObject fillArmor;
    public GameObject textHealth;
    public GameObject textArmor;
    public float maxHealth = 70;
    public float currentHealth;
    public float armor;
    public float armorAndHp;
    public float mana;
    public GameObject armorImage;
    public TMP_Text manaText;
    public TMP_Text healthText;
    public TMP_Text armorText;
    public SliderHealth sdh;
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
    public void TakeDamage(float damage)
    {
        Debug.Log(damage);
        if (armor > 0)
        {
            if (armor > damage)
            {
                armor -= damage;
                armorText.text = armor.ToString();
            }
            else
            {
                armorAndHp = armor + currentHealth;
                armorAndHp -= damage;
                armor = 0;
                currentHealth = armorAndHp;
                ResetImg();
                setHP();
            }
        }
        else
        {
            currentHealth -= damage;
            setHP();
        }
            
    }
    public void setHP()
    {
        sdh.SetHealth(currentHealth);
        healthText.text = currentHealth + "/" + maxHealth;
    }
}
