using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitShop : MonoBehaviour
{
    public GameObject shopPanel;
    ShopManager sm;
    public GameplayManager gameplayManager;
    private void Awake()
    {
        sm = FindObjectOfType<ShopManager>();
    }
    public void Onclick()
    {
        sm.plsWork();
        Debug.Log("s");
        shopPanel.SetActive(false);
        gameplayManager.StartCoroutine("ChooseNode");
    }
    
}
