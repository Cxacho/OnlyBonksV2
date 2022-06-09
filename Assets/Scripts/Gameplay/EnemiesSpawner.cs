using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    public List<List<GameObject>> floorOneEnemies;
    public List<GameObject> listOne = new List<GameObject>();
    public List<GameObject> listTwo = new List<GameObject>();
    public List<GameObject> listThree = new List<GameObject>();
    public List<GameObject> listFour = new List<GameObject>();
    public List<GameObject> listFive = new List<GameObject>();
    public List<GameObject> listSix = new List<GameObject>();
    public List<GameObject> listSeven = new List<GameObject>();
    public List<GameObject> listEight = new List<GameObject>();
    public List<GameObject> listNine = new List<GameObject>();
    public List<GameObject> listTen = new List<GameObject>();
    public List<List<GameObject>> floorOneElites;
    public List<GameObject> eliteListOne = new List<GameObject>();
    //1-3 slabi przeciwnicy , 4-6 silniejsi przeciwnicy w grupie, 7-10 duza grupa silnych przeciwnikow
    void clueless()
    {
        floorOneEnemies = new List<List<GameObject>>
        { listOne, listTwo, listThree, listFour, listFive,listSix,listSeven,listEight,listNine,listTen};
        floorOneElites = new List<List<GameObject>>
        {eliteListOne };
    }

    private void Awake()
    {
        clueless();
    }

}
