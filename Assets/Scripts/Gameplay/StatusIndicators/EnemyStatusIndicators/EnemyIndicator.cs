using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyIndicator : MonoBehaviour
{
    public GameplayManager gm;
    public Player player;
    public Enemy.statuses myStatus;
    public indiType myIndicatorType;
    public List<Enemy> enemies = new List<Enemy>();
    public int statusValue;
    [HideInInspector]public Enemy myEnemyScript;
    public virtual void Awake()
    {
        gm = FindObjectOfType<GameplayManager>();
        player = gm.player;
        foreach (GameObject obj in gm.enemies)
            enemies.Add(obj.GetComponent<Enemy>());

    }
    public enum indiType
    {
        constant,
        tickingUp,
        tickingDown,
        noValue
    }

    public virtual void checkIfIExist(Enemy.statuses ps, int val,Enemy enemy)
    {
        myEnemyScript = enemy;
        //przypadek dla obiektow bez wartosci
        foreach (Transform obj in transform.parent)
            if (obj.gameObject.GetComponent<EnemyIndicator>().myStatus == myStatus && obj.gameObject != this.gameObject)
            {
                
                obj.gameObject.GetComponent<EnemyIndicator>().statusValue += val;
                obj.gameObject.GetComponent<EnemyIndicator>().UpdateNum(obj.gameObject.GetComponent<EnemyIndicator>().statusValue);
                //nie wiem do konca czy chce zeby na dodaniu sie usuwal obiekt, czy sam ma sobie sprawdzac
                if (obj.gameObject.GetComponent<EnemyIndicator>().statusValue == 0)
                    Destroy(obj.gameObject);
                Destroy(this.gameObject);
            }
            else
                statusValue = val;
        UpdateNum(val);
        if (myIndicatorType == indiType.noValue)
            gameObject.transform.GetChild(0).gameObject.SetActive(false);


    }
    public void UpdateNum(int val)
    {
        if (myIndicatorType != indiType.noValue)
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = val.ToString();
    }
    void TickingUp()
    {
        //wywolaj na koncu tury,
        statusValue += 1;
        UpdateNum(statusValue);
        if (statusValue == 0)
            Destroy(this.gameObject);

    }
    void TickingDown()
    {
        //wywolaj na koncu tury,
        statusValue -= 1;
        UpdateNum(statusValue);
        if (statusValue == 0)
            Destroy(this.gameObject);

    }
    void Constant()
    {
        //?????
    }
}
