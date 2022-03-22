using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Enemy1,
    Enemy2,
    Enemy3,
    Enemy4,
    Enemy5,
    Boss
}

public class Enemies : MonoBehaviour
{
    public Enemy[] AllEnemies;
    //Initial Value
    public int NumberOfEnemies = 1;

    private void Start()
    {
        InitEnemies(NumberOfEnemies);
        
    }

    public void InitEnemies(int howManyEnemies)
    {
        for (int i = 0; i < howManyEnemies; i++)
        {
            var randomIndex = Random.Range(0, AllEnemies.Length - 1);

            SpawnEnemy(AllEnemies[0]);
        }
    }

    public void SpawnEnemy(Enemy enemy)
    {
        //Instantiate(enemy.EnemyPrefab, enemy.SpawnPos, Quaternion.identity);
        
    }
}