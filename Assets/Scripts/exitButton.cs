using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class exitButton : MonoBehaviour
{
    public GameObject cardToDestroy;
    public Button button;
    UiActive ui;
    private void Awake()
    {
        ui = GameObject.FindObjectOfType<UiActive>();
    }
    public void onClick()
    {
        Destroy(this.gameObject);
    }
    private void OnDestroy()
    {
        button.interactable = true;
        ui.cardToInspect = null;
        ui.SetButtonsInterractible();
        Destroy(cardToDestroy);
        
    }

}
