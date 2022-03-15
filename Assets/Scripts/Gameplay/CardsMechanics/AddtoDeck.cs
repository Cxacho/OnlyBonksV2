using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class AddtoDeck : MonoBehaviour
{
    private GameplayManager gm;
    private GameObject go;
    private string nazwaObiektu;
    private void Awake()
    {
        gm = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
    }

    public void Onclick()
    {
        this.gameObject.GetComponent<AddtoDeck>().enabled = false;
        
        go = this.gameObject;
        nazwaObiektu = go.name.Remove(go.name.Length - 7);
        Debug.Log(nazwaObiektu);
        for (int i = 0; i < gm.cards.Count; i++)
        {
            if (nazwaObiektu.Equals(gm.cards[i].name))
            {
                gm.startingDeck.Add(gm.cards[i]);

                Destroy(this.gameObject);
            }
        }
    }
}
