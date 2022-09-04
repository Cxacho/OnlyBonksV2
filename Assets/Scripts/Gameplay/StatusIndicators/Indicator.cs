using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Indicator : MonoBehaviour
{
    public GameplayManager gm;
    public Player player;
    public Player.playerStatusses myPlayerStatus;
    public indiType myIndicatorType;
    public List<Enemy> enemies = new List<Enemy>();
    public int statusValue;
    private void Awake()
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

    public void checkIfIExist(Player.playerStatusses ps,int val)
    {
        //przypadek dla obiektow bez wartosci
        foreach (Transform obj in transform.parent)
            if (obj.gameObject.GetComponent<Indicator>().myPlayerStatus == myPlayerStatus&& obj.gameObject!=this.gameObject)
            {
                obj.gameObject.GetComponent<Indicator>().statusValue += val;
                obj.gameObject.GetComponent<Indicator>().UpdateNum(obj.gameObject.GetComponent<Indicator>().statusValue);
                //nie wiem do konca czy chce zeby na dodaniu sie usuwal obiekt, czy sam ma sobie sprawdzac
                if (obj.gameObject.GetComponent<Indicator>().statusValue == 0)
                    Destroy(obj.gameObject);
                Destroy(this.gameObject);
            }
        else
        statusValue = val;
        UpdateNum(val);
        if (myIndicatorType == indiType.noValue)
            gameObject.transform.GetChild(0).gameObject.SetActive(false);


    }
    void UpdateNum(int val)
    {
        if(myIndicatorType!=indiType.noValue)
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
