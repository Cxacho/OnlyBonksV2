using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Enemy : MonoBehaviour
{

    public float maxHealth = 70;
    public float armor;
    public float damage;
    public SliderHealth sdh;
    public GameplayManager gm;
    public TMP_Text healthTxt;
    public float _currentHealth;
    public EnemyType EnemyType;


    int i = 0;
    public void Start()
    {
        Debug.Log("inicjalizacja");
        _currentHealth = maxHealth;
       healthTxt.text = _currentHealth + "/" + maxHealth;
       armor = 0;
       sdh.SetMaxHealth(maxHealth);
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

    
    public void Attack(Player player)
    {
        
        switch (i)
        {
            case 0:
                player.TakeDamage(damage);
                i++;
                break;
            case 1:
                armor = 10;
                i++;
                break;
            case 2:
                
                if(armor>0)
                {
                    player.TakeDamage(damage * 3);
                }
                else
                {
                    crippled();
                }
                i++;
                break;
            case 3:
                player.Charmed();
                i++;
                break;
            case 4:
                player.TakeDamage(damage);
                i++;
                break;
            default:
                break;
        }
    }
    public void Phase2(Player player)
    {
        // dwoch ziomkow
    }

    private void crippled()
    {

        damage = (float)(damage * 0.7);
    }
}
