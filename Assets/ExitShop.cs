using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitShop : MonoBehaviour
{
    public GameObject shopPanel;

    public GameplayManager gameplayManager;

    public void Onclick()
    {
        Debug.Log("s");
        shopPanel.SetActive(false);
        gameplayManager.StartCoroutine("ChooseNode");
    }
    
}
