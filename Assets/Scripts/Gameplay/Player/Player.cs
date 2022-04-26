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
    public int maxHealth = 70;
    public int currentHealth;
    public int armor;
    public int armorAndHp;
    public int mana;
    public GameObject armorImage;
    public TMP_Text manaText;
    public TMP_Text healthText;
    public TMP_Text armorText;
    public SliderHealth sdh;
    public TextMeshProUGUI damagePopout;


    public int strenght = 0;
    public int dexterity = 0;
    public int inteligence = 0;
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
    public void TakeDamage(int damage)
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
            damagePopout.text = "-" + damage.ToString();
            damagePopout.gameObject.SetActive(true);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(damagePopout.rectTransform.DOAnchorPosY(damagePopout.GetComponent<RectTransform>().anchoredPosition.y + 200, 0.5f));
            sequence.Append(damagePopout.rectTransform.DOAnchorPosY(damagePopout.GetComponent<RectTransform>().anchoredPosition.y - 500, 1f));
            sequence.Insert(0, damagePopout.rectTransform.DOAnchorPosX(damagePopout.GetComponent<RectTransform>().anchoredPosition.x - 300, sequence.Duration()));
            sequence.OnComplete(() =>
            {
                damagePopout.gameObject.SetActive(false);
            });
            
        }
            
    }
    public void Charmed()
    {
       strenght = strenght - 1;
    }
    public void setHP()
    {
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
    }
}
