using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class Relic : MonoBehaviour
{
    public int value;
    public string relicName;
    GameObject relicHolder;
    [HideInInspector]public GameplayManager gm;
    Canvas canvas;
    GridLayout gl;
    RectTransform rect;
    public Player pl;
    Button button;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        gl = GetComponent<GridLayout>();
        relicHolder = GameObject.Find("RelicHolder");
        pl = GameObject.Find("Player").GetComponent<Player>();
        gm = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        relicName = gameObject.name;
        button = GetComponent<Button>();
    }

    public void UponPickup()
    {
        //disolve button 
        Destroy(gameObject.transform.GetChild(0).gameObject);
        button.interactable = false;
        this.transform.SetParent(canvas.transform);
        this.transform.DOMove(relicHolder.transform.position, 1).OnComplete(() => 
        {
            gameObject.transform.SetParent(relicHolder.transform);
            gm.relicsList.Add(gameObject);
            //check czy relic ma taka sama nazwe jak jego klon// usun klona
            //gm.allRelicsList.Remove()
            

        });

    }
    public void UponDrop()
        {
        //gm.relicsList.Remove(gameObject);
        }
    public virtual void OnEndTurn()
    {

    }
    //void
}
