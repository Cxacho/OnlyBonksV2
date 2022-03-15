using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawButton : MonoBehaviour
{
    public Text inputValue;
    public GameplayManager gm;
    int DrawAmount, random, shuffle;
    [SerializeField] GameObject discardPile;
    [SerializeField] GameObject drawPile;
    // Start is called before the first frame update
    void Start()
    {
        //discardPile = GameObject.FindWithTag("DiscardDeck");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClick()
    {
        string draw = inputValue.text;
        DrawAmount = int.Parse(draw);
        //DrawCards();
    }

}
