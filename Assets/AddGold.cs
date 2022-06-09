using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGold : MonoBehaviour
{
    public GameplayManager gameplayManager;
    public void addGold()
    {
        gameplayManager.gold += gameplayManager.goldReward;
        this.gameObject.SetActive(false);
    }
}
