using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButtons : MonoBehaviour
{

    public CardCollectionManager1 cardCollectionManager;
    private Button button;

    private void Awake()
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(Onclick);
    }
    public void Onclick()
    {
        cardCollectionManager.currentWeapon = this.name;

        cardCollectionManager.ChooseWeaponCollection();
    }
   
}
