using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShopScript : MonoBehaviour
{
    [SerializeField] GameObject Button;
    public void OnClick()
    {
        Debug.Log("open" + gameObject.name);
    }
    private void OnTriggerEnter(Collider other)
    {
        Button.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        Button.SetActive(false);
    }
}
