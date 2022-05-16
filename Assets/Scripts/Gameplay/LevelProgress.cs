using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelProgress : MonoBehaviour
{

    public GameplayManager gm;

    public TextMeshProUGUI currentLvlTxt;
    public TextMeshProUGUI nextLvlTxt;
    public TextMeshProUGUI xpAmountTxt;
    public GameObject progressBar;
    public Image mask;

    private int currentLvl;
    public int nextLvlTreshold;

    private void Update()
    {
        
    }

    public void UpdateLevelProgress()
    {
        int i = Math.Abs(gm.currentXP/(nextLvlTreshold));
        while (i >= 10)
            i /= 10;

        if (gm.currentXP < 100)
        {
            currentLvl = 0;
        }
        else
        {
            currentLvl = i;
        }

        currentLvlTxt.text ="Lvl. " + currentLvl.ToString();
        nextLvlTxt.text = "Lvl. " + (currentLvl+1).ToString();

        if (currentLvl == 0)
        {
            xpAmountTxt.text = gm.currentXP.ToString() + "/" + 100 + "xp";
        }
        else
        {
            xpAmountTxt.text = gm.currentXP.ToString() + "/" + (currentLvl+1) * nextLvlTreshold + "xp";
        }

        if (gm.currentXP >= 0 && gm.currentXP < 100) { 
        float fillAmount = (float)gm.currentXP / (float)100;
        mask.fillAmount = fillAmount;
        }
        else
        {
            float fillAmount = (float)(gm.currentXP - ((nextLvlTreshold)*currentLvl)) / (float)(nextLvlTreshold);
            mask.fillAmount = fillAmount;
        }
        

    }

}
