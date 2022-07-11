using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject RunPanel;
    public void OnClick()
    {
        RunPanel.SetActive(false);
        playerMovement.state = PlayerMovement.PlayerState.wandering;
    }
}
