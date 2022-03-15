using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDropped : MonoBehaviour
{


    private GameObject go;
    public List<GameObject> temp = new List<GameObject>();
    private string nazwaObiektu;
    public GameplayManager gm;

    private void Awake()
    {
        gm = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
    }

    public virtual void OnDrop()
    {
        go = this.gameObject;
        nazwaObiektu = go.name.Remove(go.name.Length - 7);
        Debug.Log(nazwaObiektu);
        for (int i = 0; i < gm.playerHand.Count; i++)
        {
            if (nazwaObiektu.Equals(gm.playerHand[i].name))
            {
                temp.Add(gm.playerHand[i]);
                gm.discardDeck.Add(temp[0]);
                gm.playerHand.RemoveAt(i);
                temp.RemoveAt(0);
            }
        }

    }
}
