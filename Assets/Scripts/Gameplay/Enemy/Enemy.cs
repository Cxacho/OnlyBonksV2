using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



[System.Serializable]
public class Enemy
{

    public int maxHealth = 70;
    
    public int armor;
    public int damage;
    public SliderHealth sdh;
    public GameplayManager gm;
    public TMP_Text healthTxt;

    public GameObject EnemyPrefab;
    public Vector3 SpawnPos;
    private int _currentHealth;
    public EnemyType EnemyType;
    public void Init()
    {
        Debug.Log("inicjalizacja");
        _currentHealth = maxHealth;
       // healthTxt.text = _currentHealth + "/" + maxHealth;
        armor = 0;
       // sdh.SetMaxHealth(maxHealth);
    }
    public void UpdateHealth(int newHealthValue)
    {
        _currentHealth = newHealthValue;
        Debug.Log(_currentHealth);
    }

    public void ReceiveDamage(int damage)
    {
        int updatedHealth = _currentHealth - damage;
        UpdateHealth(updatedHealth > 0 ? updatedHealth : 0);
        // healthTxt.text = updatedHealth + "/" + maxHealth;
        //sdh.SetHealth(updatedHealth);
    }
    public virtual void Attack(Player player)
    {
        
    }
}
[System.Serializable]
public class EnemyONE : Enemy
{
    int i = 0;
    public override void Attack(Player player)
    {
        
        switch (i)
        {
            case 0:
                player.TakeDamage(damage);
                i++;
                break;
            case 1:
                player.TakeDamage(damage*2);
                i++;
                break;
            case 2:
                player.TakeDamage(damage*3);
                i++;
                break;
            default:
                break;
        }
        

        base.Attack(player);
    }


}
