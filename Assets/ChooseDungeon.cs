using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseDungeon : MonoBehaviour
{
    public PlayerMovement playerMovement;
    [SerializeField] GameObject Panel;
    public void OnClick()
    {
        Debug.Log("open" + gameObject.name);
    }
    private void OnTriggerEnter(Collider other)
    {
        Panel.SetActive(true);
        playerMovement.state = PlayerMovement.PlayerState.PanelActive;
    }
   
}
