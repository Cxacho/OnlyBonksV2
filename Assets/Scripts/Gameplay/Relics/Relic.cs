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
    public string description;
    Transform canvasTransform;
    GridLayout gl;
    RectTransform rect;
    public Player pl;
    Button button;
    public virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
        gl = GetComponent<GridLayout>();
        relicHolder = GameObject.Find("RelicHolder");
        pl = GameObject.Find("Player").GetComponent<Player>();
        gm = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        canvasTransform = gm.canvas.transform;
        relicName = gameObject.name;
        button = GetComponent<Button>();
    }

    public void UponPickup()
    {
        //disolve button 
        Destroy(gameObject.transform.GetChild(0).gameObject);
        button.interactable = false;
        this.transform.SetParent(canvasTransform);
        this.transform.DOMove(relicHolder.transform.position, 1).OnComplete(() => 
        {
            gameObject.transform.SetParent(relicHolder.transform);
            gm.relicsList.Add(gameObject);
            //check czy relic ma taka sama nazwe jak jego klon// usun klona
            //gameplayManager.allRelicsList.Remove()
            

        });

    }
    public void UponDrop()
        {
        //gameplayManager.relicsList.Remove(gameObject);
        }
    public virtual void OnEndTurn()
    {

    }
    //void
}
